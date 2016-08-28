using UnityEngine;
using System.Collections;

public class Barbarian : Unit {

	public Barbarian() {

		// Standard values
		this.SetId();
		this.SetStandardValues ();

		this.Name = "Barbarian";

		this.GatheringSkill = 0;

		this.AttackPower += Random.Range (0, 7);

		this.MaxHitPoints += Random.Range (0, 10);
		this.CurrentHitPoints = this.MaxHitPoints;

		this.MovementSpeed += Random.Range (0, 3);

	}
}
