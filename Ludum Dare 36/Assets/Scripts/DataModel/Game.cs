using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game  {

	public HomeBase HomeBaseData { get; private set; }

	public Dictionary<int, Villager> VillagersDict { get; private set; }

	public ResourcePool ResourcePoolData { get; private set; }

	public int CurrentLevel;

	public Game() {

		this.CurrentLevel = 1;

		this.HomeBaseData = new HomeBase ();

		this.ResourcePoolData = new ResourcePool ();

		this.VillagersDict = new Dictionary<int, Villager> ();

		// Generate the starting villagers
		for (int i = 0; i < 5; i++) {
			Villager curVillager = new Villager ();
			VillagersDict.Add (curVillager.UnitID, curVillager);
		}

	}

	public void LoadNextLevel() {
		this.CurrentLevel++;
	}

}
