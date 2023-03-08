using Stride.Engine;

namespace Code;

public class Shed : StartupScript
{
	public int StoredWood { get; set; }

	public bool StoreWood(int amountToStore)
	{
		StoredWood += amountToStore;
		//return bool in case we want to add a storage limit later
		return true;
	}
}