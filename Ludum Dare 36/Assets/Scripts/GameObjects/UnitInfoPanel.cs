using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour {

	public Unit UnitData { get; private set; }

	[SerializeField]
	private Text hp;

	[SerializeField]
	private Text attackPower;

	[SerializeField]
	private Text gatheringSkill;

	[SerializeField]
	private Text armor;

	[SerializeField]
	private Text movement;

	[SerializeField]
	private Text tool;

	[SerializeField]
	private Text apparel;

	private int latestHP;

	public void SetUnitData(Unit data) {
		this.UnitData = data;

		Destroy (this.tool.gameObject);
		Destroy (this.apparel.gameObject);

		this.SetText ();
	}

	private void SetText() {
		this.hp.text = "HP\n" + UnitData.CurrentHitPoints.ToString() + "/" + UnitData.MaxHitPoints.ToString();
		latestHP = UnitData.CurrentHitPoints;
		this.attackPower.text = "Attack Power\n" + UnitData.AttackPower.ToString();
		this.gatheringSkill.text = "Gathering Skill\n" + UnitData.GatheringSkill.ToString();
		this.armor.text = "Armor Rating\n" + UnitData.Armor.ToString();
		this.movement.text = "Move. Speed\n" + UnitData.MovementSpeed.ToString();



//		if (UnitData.EquippedTool == null) {
//			this.tool.text = "Tool\nNone";
//		}
//
//		if (UnitData.EquippedApparel == null) {
//			this.apparel.text = "Apparal\nNone";
//		}
	}

	void Update() {
		if (latestHP != UnitData.CurrentHitPoints) {
			this.SetText ();
		}
	}

}
