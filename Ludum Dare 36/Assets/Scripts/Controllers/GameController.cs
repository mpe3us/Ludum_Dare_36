﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	[SerializeField]
	private GameObject BasePrefab;

	private GameObject baseGO;

	public Game GameData { get; private set; }

	[SerializeField]
	private GameObject villagerPrefab;

	public Dictionary<int, GameObject> VillagerGameObjects { get; private set; }

	public List<GameObject> resourceObjects;

	[SerializeField]
	private GameObject foodResourcePrefab;

	void Awake() {

		if (Instance != null) {
			Debug.LogError ("There is already a GameController");
		}

		Instance = this;

		this.VillagerGameObjects = new Dictionary<int, GameObject> ();

	}

	// Called by the cubemap controlller after the map has been initialized
	public void InitGame() {

		// Init new game 
		this.GameData = new Game ();

		this.InitBaseAtPosition ();
		this.InitVillagers ();
		ResourcePoolController.Instance.SetResourceData (this.GameData.ResourcePoolData);
		this.SpawnResources ();
	}

	public void InitBaseAtPosition() {
		CubeMap cubeMap = CubeMapController.Instance.CubeMapData;
		GameObject cubeController = CubeMapController.Instance.GetCubeGO (cubeMap.Width / 2, cubeMap.Depth / 2);
		this.baseGO = Instantiate (this.BasePrefab, this.transform) as GameObject;
		this.baseGO.transform.position = cubeController.transform.position;

		this.baseGO.GetComponent<HomeBaseController> ().SetHomeBaseData (this.GameData.HomeBaseData);
	}

	public void InitVillagers () {
		foreach (Unit curUnit in this.GameData.VillagersDict.Values) {
			GameObject curGO = Instantiate (this.villagerPrefab, this.transform) as GameObject;
			curGO.GetComponent<UnitController> ().SetUnitData (curUnit);
			this.VillagerGameObjects.Add (curUnit.UnitID, curGO);

			float randX;
			float randZ;

			if (Random.Range (0, 2) == 0) {
				randX = Random.Range (0.5f, 2.0f);
			} else {
				randX = Random.Range (-0.5f, -2.0f);
			}
			if (Random.Range (0, 2) == 0) {
				randZ = Random.Range (0.5f, 2.0f);
			} else {
				randZ = Random.Range (-0.5f, -2.0f);
			}

			curGO.transform.position = new Vector3 (
				this.baseGO.transform.position.x + randX, 
				this.baseGO.transform.position.y, 
				this.baseGO.transform.position.z + randZ);
		}
	}

	public void SpawnResources() {

		foreach (GameObject go in this.resourceObjects) {
			Destroy (go);
		}

		int foodResources = Random.Range (0, this.GameData.CurrentLevel+2);
	
		Debug.Log ("Number of food resources: " + foodResources);

		for (int i = 0; i < foodResources; i++) {
			GameObject curResource = Instantiate (this.foodResourcePrefab, this.transform) as GameObject;
			curResource.transform.position = CubeMapController.Instance.GetRandomCubeGO ().transform.position;
			curResource.transform.Rotate (0, Random.Range (0, 360), 0);
			curResource.GetComponent<ResourceController> ().SetResourceData (ResourcePool.ResourceTypes.FOOD, Random.Range (1, 7), "Food", ResourcePoolController.foodColor);
			this.resourceObjects.Add (curResource);
		}

	}


}
