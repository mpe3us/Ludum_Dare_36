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

	public void SetUnitData(Unit data) {
		this.UnitData = data;

		this.hp.text = "HP\n" + data.CurrentHitPoints.ToString() + "/" + data.MaxHitPoints.ToString();
		this.attackPower.text = "Attack Power\n" + data.AttackPower.ToString();
		this.gatheringSkill.text = "Gathering Skill\n" + data.GatheringSkill.ToString();
		this.armor.text = "Armor Rating\n" + data.Armor.ToString();
		this.movement.text = "Move. Speed\n" + data.MovementSpeed.ToString();

		if (data.EquippedTool == null) {
			this.tool.text = "Tool\nNone";
		}

		if (data.EquippedApparel == null) {
			this.apparel.text = "Apparal\nNone";
		}

	}



}
