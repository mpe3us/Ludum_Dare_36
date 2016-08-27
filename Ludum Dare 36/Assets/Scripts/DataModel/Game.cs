using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game  {

	public HomeBase HomeBaseData { get; private set; }

	public Dictionary<int, Villager> VillagersDict { get; private set; }

	public Game() {

		this.HomeBaseData = new HomeBase ();

		this.VillagersDict = new Dictionary<int, Villager> ();

		// Generate the starting villagers
		for (int i = 0; i < 5; i++) {
			Villager curVillager = new Villager ();
			VillagersDict.Add (curVillager.UnitID, curVillager);
		}

	}

}
