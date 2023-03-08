using Code;
using Doprez.Stride.AI.FSMs;

namespace NPC.States;

public class IdleState : FSMState
{

	private NPCData _npcData {get;set;}
	private WorldData _worldData { get; set; }

	public IdleState(NPCData npcData, WorldData worldData)
	{
		_npcData = npcData;
		_worldData = worldData;

		Name = "Thinking of what I can do";
	}

	public override void EnterState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	public override void UpdateState()
	{
		if(_npcData.HasAxe && _worldData.TreesInScene[0].GatherableAmount > 0)
		{
			FiniteStateMachine.SetCurrentState((int)AvailableStates.GatherWood);
		}
		else if(!_npcData.HasAxe)
		{
			FiniteStateMachine.SetCurrentState((int)AvailableStates.GetAxe);
		}
		else
		{
			Name = "There is nothing left to do!";
		}
	}
}