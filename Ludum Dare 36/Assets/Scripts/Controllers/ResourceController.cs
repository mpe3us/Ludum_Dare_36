using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour {

	public string Name { get; private set; }

	public ResourcePool.ResourceTypes ResType { get; private set; }
	public int Amount { get; private set; }

	public Color ResColor { get; private set; }

	public const float requiredProgress = 1.0f;

	public float curProgress;

	public GameObject ResourcePrefab { get; private set; }

	public struct ReturnResourceStruct {
		public GameObject resPrefab;
		public ResourcePool.ResourceTypes resType;
		public GameObject resReference;
	}

	public void SetResourceData(ResourcePool.ResourceTypes type, int amount, string name, Color col, GameObject resourcePrefab) {
		this.Amount = amount;
		this.ResType = type;

		this.Name = name;

		this.ResColor = col;

		this.curProgress = 0;

		this.ResourcePrefab = resourcePrefab;
	}

	// Returns the resource prefab if this tick reached to 100%
	public ReturnResourceStruct TickResource(UnitController unitCon) {
		ReturnResourceStruct returnObject = new ReturnResourceStruct();

		curProgress += unitCon.UnitData.GatheringSkill * Time.deltaTime;

		//Debug.Log ("Current resource progress for " + Name + ": " + curProgress);

		if (curProgress >= requiredProgress) {
			returnObject.resPrefab = this.ResourcePrefab;
			returnObject.resType = this.ResType;
			returnObject.resReference = this.gameObject;
			this.Amount -= 1;
			this.curProgress = 0;
		}

		return returnObject;
	}

	void Update() {
		if (this.Amount <= 0) {
			this.gameObject.SetActive (false);
		}
	}

}
