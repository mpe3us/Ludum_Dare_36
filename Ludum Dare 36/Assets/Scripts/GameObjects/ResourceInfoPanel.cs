using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceInfoPanel : MonoBehaviour {

	private ResourceController resourceData;

	[SerializeField]
	private Text remainingAmount;

	private int curAmount;

	public void SetResourceData(ResourceController data) {
		this.resourceData = data;

		this.remainingAmount.text = "Quantity\n" + data.Amount;

		this.curAmount = data.Amount;

	}

	void Update() {
		if (curAmount != this.resourceData.Amount) {
			this.remainingAmount.text = "Quantity\n" + this.resourceData.Amount;
			this.curAmount = this.resourceData.Amount;
		}
	}


}
