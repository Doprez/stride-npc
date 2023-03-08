using System.Threading.Tasks;
using Code;
using Code.Resources;
using Doprez.Stride.AI.FSMs;
using Stride.Core.Mathematics;
using Stride.Engine;

namespace NPC.States;

public class GatherWoodState : FSMState
{

	private float _elapsedtime = 0;

	private readonly MoveToState _move;
	private readonly WorldData _worldData;
	private readonly NPCData _npcData;
	private readonly Game _game;

	public GatherWoodState(MoveToState move, WorldData worldData, NPCData npcData, Game game)
	{
		_move = move;
		_worldData = worldData;
		_npcData = npcData;
		_game = game;

		Name = "Going to get some wood";
	}

	public override void EnterState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	public override void UpdateState()
	{
		// Need a method to find the closest tree to target but this is fine for demo
		var targetTree = _worldData.TreesInScene[0];

		var distance = Vector3.Distance(targetTree.Entity.Transform.Position, FiniteStateMachine.Entity.Transform.Position);

		if(distance > 1)
		{
			_move.Target = targetTree.Entity;
			FiniteStateMachine.SetCurrentState((int)AvailableStates.Walk);
		}
		else
		{
			GetWood(targetTree);
		}
	}

	private void GetWood(Tree targetTree)
	{  
		_elapsedtime += (float)_game.UpdateTime.Elapsed.TotalSeconds;

		if(_elapsedtime >= 1)
		{
			//check if wood was able to be gathered
			if (targetTree.GatherWood())
			{
				_npcData.WoodGathered++;
				_elapsedtime = 0;
			}
			else
			{
				FiniteStateMachine.SetCurrentState((int)AvailableStates.StoreWood);
			}
		}
	}

}
