using System.Collections.Generic;
using Code.Resources;
using Stride.Core;

namespace Code;

[DataContract("WorldData")]
[Display("World Data")]
public class WorldData : Stride.Data.Configuration
{
	public List<Tree> TreesInScene { get; private set; } = new();

	public void RegisterTree(Tree treeToAdd)
	{
		TreesInScene.Add(treeToAdd);
	}
}