using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourcePoolController : MonoBehaviour {

	public static ResourcePoolController Instance;

	public ResourcePool ResourcePoolData { get; private set; }

	[SerializeField]
	private Text food;

	[SerializeField]
	private Text coal;

	[SerializeField]
	private Text gold;

	[SerializeField]
	private Text wood;

	[SerializeField]
	private Text iron;

	public static Color foodColor;
	public static Color coalColor;
	public static Color goldColor;
	public static Color woodColor;
	public static Color ironColor;

	void Awake() {
		if (Instance != null) {
			Debug.LogError ("There should only be one resource pool controller");
		}

		Instance = this;

		foodColor = food.color;
		coalColor = coal.color;
		goldColor = gold.color;
		woodColor = wood.color;
		ironColor = iron.color;
	}

	public void SetResourceData(ResourcePool data) {
		this.ResourcePoolData = data;
		this.UpdateTextFields ();
	}

	public void UpdateTextFields() {
	
		food.text = "Food: " + this.ResourcePoolData.FoodPool;
		coal.text = "Coal: " + this.ResourcePoolData.CoalPool;
		gold.text = "Gold: " + this.ResourcePoolData.GoldPool;
		wood.text = "Wood: " + this.ResourcePoolData.WoodPool;
		iron.text = "Iron: " + this.ResourcePoolData.IronPool;
	
	}

}
