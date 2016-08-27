using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour {

	public string Name { get; private set; }

	public ResourcePool.ResourceTypes ResType { get; private set; }
	public int Amount { get; private set; }

	public Color ResColor { get; private set; }

	public void SetResourceData(ResourcePool.ResourceTypes type, int amount, string name, Color col) {
		this.Amount = amount;
		this.ResType = type;

		this.Name = name;

		this.ResColor = col;
	}

}
