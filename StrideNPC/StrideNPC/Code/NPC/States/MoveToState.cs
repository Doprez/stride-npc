using Doprez.Stride.AI.FSMs;
using Stride.Core.Mathematics;
using Stride.Engine;
using Navigation;

namespace NPC.States;

public class MoveToState : FSMState
{
    public Entity Target;

    private readonly AsyncPathfinder _pathfinder;

    private Vector3 _originalTargetPoint;

    public MoveToState(AsyncPathfinder pathfinder)
    {
        _pathfinder = pathfinder;
        Name = "I am going places";
    }

    public override void EnterState()
    {
		_originalTargetPoint = Target.WorldPosition();
        _pathfinder.SetWaypoint(_originalTargetPoint);
    }

    public override void ExitState()
	{
		//reset forces the pathfinder to stop moving the unit
		_pathfinder.Reset();
    }

    public override void UpdateState()
    {
        if(!_pathfinder.HasPath || _originalTargetPoint != Target.WorldPosition())
        {
            _originalTargetPoint = Target.WorldPosition();
            _pathfinder.SetWaypoint(_originalTargetPoint);
        }

        if(_pathfinder.CurrentPathDistance < .7f)
        {
            FiniteStateMachine.SetCurrentState(FiniteStateMachine.GetState((int)AvailableStates.Idle));
        }

    }
}