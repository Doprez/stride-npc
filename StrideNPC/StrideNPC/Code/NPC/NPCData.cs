using Code;
using Stride.Engine;

namespace NPC.States;

public class NPCData : StartupScript
{

	public House NPCHome { get; set; }
	public Shed NPCStorage { get; set; }
	public bool HasAxe { get; set; }
	public int WoodGathered { get; set; }

	public override void Start()
	{
		base.Start();
	}
}