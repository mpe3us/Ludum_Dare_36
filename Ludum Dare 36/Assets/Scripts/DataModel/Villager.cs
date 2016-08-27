using UnityEngine;
using System.Collections;

public class Villager : Unit {

	public Villager() {

		// Standard values
		this.SetId();
		this.SetStandardValues ();

		this.Name = "Villager";

		if (Random.Range (0, 2) == 0) {
			this.AttackPower += Random.Range (3, 11);
			this.GatheringSkill += Random.Range (0, 3);
		} else {
			this.AttackPower += Random.Range (0, 3);
			this.GatheringSkill += Random.Range (3, 11);
		}

		this.MaxHitPoints += Random.Range (0, 16);
		this.CurrentHitPoints = this.MaxHitPoints;

		this.MovementSpeed = Random.Range (1, 4);

	}

}
