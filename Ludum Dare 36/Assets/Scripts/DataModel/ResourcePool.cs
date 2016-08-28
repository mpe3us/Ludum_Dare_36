using UnityEngine;
using System.Collections;

public class ResourcePool {

	public enum ResourceTypes
	{
		UNDEFINED, FOOD, COAL, GOLD, WOOD, IRON
	}

	public int FoodPool { get; private set; }
	public int CoalPool { get; private set; }
	public int GoldPool { get; private set; }
	public int WoodPool { get; private set; }
	public int IronPool { get; private set; } 

	public ResourcePool() {

		this.FoodPool = 15;
		this.CoalPool = 10;
		this.GoldPool = 5;
		this.WoodPool = 0;
		this.IronPool = 0;

	}

	public void IncrementPool(ResourceTypes type, int value) {

		switch (type) {
		case (ResourceTypes.FOOD):
			this.FoodPool += value;
			break;
		case (ResourceTypes.COAL):
			this.CoalPool += value;
			break;
		case (ResourceTypes.GOLD):
			this.GoldPool += value;
			break;
		case (ResourceTypes.WOOD):
			this.WoodPool += value;
			break;
		case (ResourceTypes.IRON):
			this.IronPool += value;
			break;
		default:
			Debug.LogError("unsupported resource type");
			break;
		}

	}

	// Returns true if decremented succesfully
	public bool decrementPool(ResourceTypes type, int value) {
		
		bool returnValue = false;

		switch (type) {
		case (ResourceTypes.FOOD):
			if (value <= this.FoodPool) {
				this.FoodPool -= value;
				returnValue = true;
			}
			break;
		case (ResourceTypes.COAL):
			if (value <= this.CoalPool) {
				this.FoodPool -= value;
				returnValue = true;
			}
			break;
		case (ResourceTypes.GOLD):
			if (value <= this.GoldPool) {
				this.FoodPool -= value;
				returnValue = true;
			}
			break;
		case (ResourceTypes.WOOD):
			if (value <= this.WoodPool) {
				this.FoodPool -= value;
				returnValue = true;
			}
			break;
		case (ResourceTypes.IRON):
			if (value <= this.IronPool) {
				this.FoodPool -= value;
				returnValue = true;
			}
			break;
		default:
			Debug.LogError("unsupported resource type");
			break;
		}

		return returnValue;
	}

}
