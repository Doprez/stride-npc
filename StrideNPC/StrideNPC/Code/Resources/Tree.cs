using System.ComponentModel;
using Stride.Engine;
using Stride.Engine.Design;

namespace Code.Resources;

public class Tree : StartupScript
{

	public int GatherableAmount { get; set; } = 5;

	private WorldData _worldData;

	public override void Start()
	{
		base.Start();

		_worldData = Services.GetService<IGameSettingsService>()?
		.Settings.Configurations.Get<WorldData>();
		_worldData.RegisterTree(this);
	}

	public bool GatherWood()
	{
		GatherableAmount--;

		if(GatherableAmount > -1)
			return true;
		return false;
	}
}