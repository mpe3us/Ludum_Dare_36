using UnityEngine;
using System.Collections;

public abstract class Unit {

	private static int CurrentID;

	public int UnitID { get; private set; }

	public string Name { get; protected set; }

	public int AttackPower { get; protected set; }
	public float BaseAttackRange { get; protected set; }

	public int GatheringSkill { get; protected set; }
	public float BaseGatheringRange { get; protected set; }

	public int MaxHitPoints { get; protected set; }
	public int CurrentHitPoints { get; protected set; }

	public int Armor { get; protected set; }

	public int MovementSpeed { get; protected set; }

	public Tool EquippedTool { get; protected set; }
	public Apparel EquippedApparel { get; protected set; }

	protected void SetId() {
		CurrentID++;

		this.UnitID = CurrentID;
	}

	protected void SetStandardValues() {

		this.Name = "Unnamed";

		this.BaseAttackRange = 1.0f;
		this.BaseGatheringRange = 1.0f;
		this.Armor = 0;

		this.AttackPower = Random.Range (1, 3);

		this.GatheringSkill = Random.Range (1, 3);

		this.MaxHitPoints = Random.Range (10, 15);
		this.CurrentHitPoints = this.MaxHitPoints;

		this.MovementSpeed = Random.Range(1,2);

	}

}
