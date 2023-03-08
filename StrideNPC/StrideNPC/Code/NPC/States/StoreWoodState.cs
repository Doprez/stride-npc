using Code;
using Doprez.Stride.AI.FSMs;
using Stride.Core.Mathematics;

namespace NPC.States;

public class StoreWoodState : FSMState
{

	private readonly MoveToState _move;
	private readonly NPCData _npcData;
	private readonly Shed _shed;

	public StoreWoodState(MoveToState move, NPCData npcData)
	{
		_move = move;
		_npcData = npcData;
		_shed = _npcData.NPCStorage;

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
		var distance = Vector3.Distance(_shed.Entity.Transform.Position, FiniteStateMachine.Entity.Transform.Position);

		if (distance > 1)
		{
			_move.Target = _shed.Entity;
			FiniteStateMachine.SetCurrentState((int)AvailableStates.Walk);
		}
		else if(_shed.StoreWood(_npcData.WoodGathered))
		{
			_npcData.WoodGathered = 0;
			FiniteStateMachine.SetCurrentState((int)AvailableStates.Idle);
		}
	}
}