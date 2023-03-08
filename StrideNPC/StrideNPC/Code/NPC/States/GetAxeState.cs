using Stride.Engine;
using Doprez.Stride.AI.FSMs;
using Stride.Core.Mathematics;

namespace NPC.States;

public class GetAxeState : FSMState
{

	private MoveToState _move;
	private NPCData _npcData;

	public GetAxeState(MoveToState move, NPCData npcData)
	{
		_move = move;
		_npcData = npcData;

		Name = "Going to find an axe";
	}

	public override void EnterState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	public override void UpdateState()
	{
		var distance = Vector3.Distance(_npcData.NPCHome.Entity.Transform.Position, FiniteStateMachine.Entity.Transform.Position);

		if(distance > 0.7f)
		{
			_move.Target = _npcData.NPCHome.Entity;
			FiniteStateMachine.SetCurrentState(_move);
		}
		else
		{
			//simulate adding the axe to the NPC inventory
			_npcData.NPCHome.HasAxe = false;
			_npcData.HasAxe = true;

			// go back to IDle to see what needs to be done next
			FiniteStateMachine.SetCurrentState((int)AvailableStates.Idle);
		}
	}
}