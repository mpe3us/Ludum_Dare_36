using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseInfoPanel : MonoBehaviour {

	[SerializeField]
	private Text hpText;

	public void SetData(HomeBaseController baseData) {

		hpText.text = "HP\n" + baseData.HomeBaseData.CurrentHitPoints + "/" + baseData.HomeBaseData.MaxHitPoints;

	}


}
