using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceInfoPanel : MonoBehaviour {

	private ResourceController resourceData;

	[SerializeField]
	private Text remainingAmount;

	public void SetResourceData(ResourceController data) {
		this.resourceData = data;

		this.remainingAmount.text = "Quantity\n" + data.Amount;

	}




}
