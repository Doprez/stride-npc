using Code;
using Doprez.Stride.AI.FSMs;
using Navigation;
using NPC.States;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Design;

namespace NPC;

public class NPCStateMachine : FSM
{

	private WorldData _worldData;
	private AsyncPathfinder _pathfinder;
	private NPCData _npcData;

	//states
	private IdleState _idle;
	private MoveToState _move;
	private GatherWoodState _gatherWood;
	private GetAxeState _getAxe;
	private StoreWoodState _storeWood;
	

	public override void Start()
	{
		_worldData = Services.GetService<IGameSettingsService>()?
			.Settings.Configurations.Get<WorldData>();
		_pathfinder = Entity.Get<AsyncPathfinder>();

		_npcData = Entity.Get<NPCData>();

		InititalizeStates();

		SetCurrentState(_idle);
	}

	public override void UpdateFSM()
	{
		DebugText.Print($"running State: {currentState.Name}", new Int2(50, 50));
		DebugText.Print($"Wood in inventory: {_npcData.WoodGathered}", new Int2(50, 70));
		DebugText.Print($"Wood in NPC shed: {_npcData.NPCStorage.StoredWood}", new Int2(50, 90));
	}

	private void InititalizeStates()
	{
		_idle = new IdleState(_npcData, _worldData)
		{
			FiniteStateMachine = this,
		};
		Add((int)AvailableStates.Idle, _idle);

		_move = new MoveToState(_pathfinder)
		{
			FiniteStateMachine = this,
		};
		Add((int)AvailableStates.Walk, _move);

		_gatherWood = new GatherWoodState(_move, _worldData, _npcData, (Game)Game)
		{
			FiniteStateMachine = this,
		};
		Add((int)AvailableStates.GatherWood, _gatherWood);

		_getAxe = new GetAxeState(_move, _npcData)
		{
			FiniteStateMachine = this,
		};
		Add((int)AvailableStates.GetAxe, _getAxe);

		_storeWood = new StoreWoodState(_move, _npcData)
		{
			FiniteStateMachine = this,
		};
		Add((int)AvailableStates.StoreWood, _storeWood);
	}

}