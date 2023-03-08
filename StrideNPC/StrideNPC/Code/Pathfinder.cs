using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Design;
using Stride.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Navigation;

[ComponentCategory("Navigation")]
public class AsyncPathfinder : AsyncScript
{

	public float MovementSpeed { get; set; } = 10;
	public Vector3 TargetPosition;
	public string NavGroupName { get; set; }
	public bool HasPath { get; private set; }

	private GameSettings _gameSettings;
	private NavigationComponent _navigationComponent = new NavigationComponent();
	private List<Vector3> _waypoints = new();
	private int waypointIndex = 0;

	public float DistanceToTarget
	{
		get
		{
			return Vector3.Distance(Entity.Transform.WorldMatrix.TranslationVector, TargetPosition);
		}
	}

	/// <summary>
	/// not the most efficient method to use per frame but logically is better than DistanceToTarget
	/// </summary>
	public float CurrentPathDistance
	{
		get
		{
			float distance = 0;
			if(_waypoints.Count == 0)
			{
				return distance;
			}
			for (int i = 0; i < _waypoints.Count - 1; i++)
			{
				distance += Vector3.Distance(_waypoints[i], _waypoints[i+1]);
			}
			return distance;
		}
	}

	private void InitializePathFinder()
	{
		_gameSettings = Services.GetService<IGameSettingsService>()?.Settings;

		var navSettings = _gameSettings.Configurations.Get<NavigationSettings>();

		_navigationComponent.GroupId = navSettings.Groups.FirstOrDefault(x => x.Name == NavGroupName).Id;

		Entity.Add(_navigationComponent);
	}

	public override async Task Execute()
	{
		InitializePathFinder();
		while (Game.IsRunning)
		{
			Move();
			Rotate();

			await Script.NextFrame();
		}
	}
	
	/// <summary>
	/// Stops the pathfinder and resets some base values
	/// </summary>
	public void Reset()
	{
		ClearGoal();
	}

	public void SetWaypoint(Vector3 targetWaypoint)
	{
		ClearGoal();
		TargetPosition = targetWaypoint;
		if (_navigationComponent.TryFindPath(TargetPosition, _waypoints))
		{
			waypointIndex = 0;
			HasPath = true;
		}
	}

	public async Task SetWaypointAsync(Vector3 targetWaypoint)
	{
		ClearGoal();
		TargetPosition = targetWaypoint;
		var foundPath = await Task.FromResult(_navigationComponent.TryFindPath(TargetPosition, _waypoints));
		if (foundPath)
		{
			waypointIndex = 0;
		}
	}

	private void Move()
	{
		if (_waypoints.Count == 0)
		{
			return;
		}

		var deltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
		var curPosition = Entity.Transform.WorldMatrix.TranslationVector;
		var nextWaypointPosition = _waypoints[waypointIndex];
		var distanceToWaypoint = Vector3.Distance(curPosition, nextWaypointPosition);

		// When the distance between the character and the next waypoint is large enough, move closer to the waypoint
		if (distanceToWaypoint > 0.1)
		{
			var direction = nextWaypointPosition - curPosition;
			direction.Normalize();
			direction *= MovementSpeed * deltaTime;

			Entity.Transform.Position += direction;
		}
		else
		{
			// If we are close enough to the waypoint, set the next waypoint or we are done and we do a final cleanup 
			if (waypointIndex + 1 < _waypoints.Count)
			{
				waypointIndex++;
			}
			else
			{
				ClearGoal();
			}
		}
	}

	private void Rotate()
	{
		if (_waypoints.Count > 0)
		{
			var test = Entity.GetYAngleToTarget(_waypoints[waypointIndex]);
			Entity.Transform.Rotation = Quaternion.RotationY(-test);
		}
	}

	private void ClearGoal()
	{
		_waypoints.Clear();
		HasPath = false;
	}
}
