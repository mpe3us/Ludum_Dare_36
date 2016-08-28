using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public bool GameOver;

	private bool gameInit;

	[SerializeField]
	private GameObject BasePrefab;

	public GameObject BaseGO { get; private set; }

	public Game GameData { get; private set; }

	[SerializeField]
	private GameObject villagerPrefab;

	public Dictionary<int, GameObject> VillagerGameObjects { get; private set; }

	public List<GameObject> resourceObjects;

	[SerializeField]
	private GameObject foodResourcePrefab;
	[SerializeField]
	private GameObject foodResourceCubePrefab;

	[SerializeField]
	private GameObject woodResourcePrefab;
	[SerializeField]
	private GameObject woodResourceCubePrefab;

	[SerializeField]
	private GameObject barbarianPrefab;

	[SerializeField]
	private GameObject coalResource;
	[SerializeField]
	private GameObject coalCube;
	[SerializeField]
	private GameObject goldResource;
	[SerializeField]
	private GameObject goldCube;
	[SerializeField]
	private GameObject ironResource;
	[SerializeField]
	private GameObject ironCube;

	private float barbSpawnTime;
	private float curBarbTime;
	private int barbsInNextWave;

	void Awake() {

		if (Instance != null) {
			Debug.LogError ("There is already a GameController");
		}

		Instance = this;

		this.VillagerGameObjects = new Dictionary<int, GameObject> ();
		this.gameInit = false;

		this.barbSpawnTime = 20.0f;
		this.curBarbTime = 15.0f;
		this.barbsInNextWave = 1;

		this.GameOver = false;
	}

	void Update() {
		if (!gameInit) {
			return;
		}

		this.curBarbTime += Time.deltaTime;

		if (this.curBarbTime >= this.barbSpawnTime) {

			List<GameObject> remainingVillagers = new List<GameObject> (this.VillagerGameObjects.Values);

			for (int i = 0; i < this.barbsInNextWave; i++) {
				Vector3 spawnPos = resourceObjects [Random.Range (0, resourceObjects.Count)].transform.position;
				spawnPos = new Vector3 (spawnPos.x + Random.Range(-2.0f, 2.0f), spawnPos.y, spawnPos.z + Random.Range(-2.0f, 2.0f));
				GameObject curGO = Instantiate (this.barbarianPrefab, spawnPos, Quaternion.identity) as GameObject;
				curGO.transform.SetParent (this.transform);
				Barbarian barb = new Barbarian ();
				curGO.GetComponent<UnitController> ().SetUnitData (barb, this.BaseGO);

				int number = 1;

				// Set attack target
				if (number == 0) {
					curGO.GetComponent<UnitController> ().SetNewTargetPosition (this.BaseGO.transform.position, UnitController.ActionMode.ATTACK, this.BaseGO);
				} else {
					Vector3 targetPos = Vector3.zero;
					float latestDist = 1000.0f;
					GameObject targetVillager = null;
					foreach (GameObject villager in remainingVillagers) {
						float curDist = Vector3.Distance (curGO.transform.position, villager.transform.position);
						if (curDist < latestDist) {
							targetPos = villager.transform.position;
							latestDist = curDist;
							targetVillager = villager;
						}
					}
					if (targetPos == Vector3.zero || targetVillager == null) {
						return;
					} else {
						curGO.GetComponent<UnitController> ().SetNewTargetPosition (targetVillager.transform.position, UnitController.ActionMode.ATTACK, targetVillager);
					}
				}
			}

			this.barbSpawnTime += 5.0f;
			this.curBarbTime = 0.0f;
			this.barbsInNextWave = Random.Range(1, 3);

		}

	}

	public void RequestNewTarget(UnitController unitCon) {
		List<GameObject> remainingVillagers = new List<GameObject> (this.VillagerGameObjects.Values);
		foreach (GameObject villager in remainingVillagers) {
			if (villager.activeInHierarchy) {
				unitCon.SetNewTargetPosition (villager.transform.position, UnitController.ActionMode.ATTACK, villager);
				break;
			}
		}
	}

	// Called by the cubemap controlller after the map has been initialized
	public void InitGame() {

		// Init new game 
		this.GameData = new Game ();

		this.InitBaseAtPosition ();
		this.InitVillagers ();
		ResourcePoolController.Instance.SetResourceData (this.GameData.ResourcePoolData);
		this.SpawnResources ();

		this.gameInit = true;
	}

	public void InitBaseAtPosition() {
		CubeMap cubeMap = CubeMapController.Instance.CubeMapData;
		GameObject cubeController = CubeMapController.Instance.GetCubeGO (cubeMap.Width / 2, cubeMap.Depth / 2);
		this.BaseGO = Instantiate (this.BasePrefab, this.transform) as GameObject;
		this.BaseGO.transform.position = cubeController.transform.position;

		this.BaseGO.GetComponent<HomeBaseController> ().SetHomeBaseData (this.GameData.HomeBaseData);
	}

	public void InitVillagers () {
		foreach (Unit curUnit in this.GameData.VillagersDict.Values) {
			GameObject curGO = Instantiate (this.villagerPrefab, this.transform) as GameObject;
			curGO.GetComponent<UnitController> ().SetUnitData (curUnit, this.BaseGO);
			this.VillagerGameObjects.Add (curUnit.UnitID, curGO);

			float randX;
			float randZ;

			if (Random.Range (0, 2) == 0) {
				randX = Random.Range (0.7f, 2.2f);
			} else {
				randX = Random.Range (-0.7f, -2.2f);
			}
			if (Random.Range (0, 2) == 0) {
				randZ = Random.Range (0.7f, 2.2f);
			} else {
				randZ = Random.Range (-0.7f, -2.2f);
			}

			curGO.transform.position = new Vector3 (
				this.BaseGO.transform.position.x + randX, 
				this.BaseGO.transform.position.y, 
				this.BaseGO.transform.position.z + randZ);
		}
	}

	public void SpawnResources() {

		foreach (GameObject go in this.resourceObjects) {
			Destroy (go);
		}

		int foodResources = Random.Range (5, 15);
	
		//Debug.Log ("Number of food resources: " + foodResources);

		for (int i = 0; i < foodResources; i++) {
			GameObject curResource = Instantiate (this.foodResourcePrefab, this.transform) as GameObject;
			curResource.transform.position = CubeMapController.Instance.GetRandomCubeGO ().transform.position;
			curResource.transform.Rotate (0, Random.Range (0, 360), 0);
			curResource.GetComponent<ResourceController> ().SetResourceData (ResourcePool.ResourceTypes.FOOD, Random.Range (1, 7), "Food", ResourcePoolController.foodColor, this.foodResourceCubePrefab);
			this.resourceObjects.Add (curResource);
		}

		int woodResources = Random.Range (15, 20);

		for (int i = 0; i < woodResources; i++) {
			GameObject curResource = Instantiate (this.woodResourcePrefab, this.transform) as GameObject;
			curResource.transform.position = CubeMapController.Instance.GetRandomCubeGO ().transform.position;
			curResource.transform.Rotate (0, Random.Range (0, 360), 0);
			curResource.GetComponent<ResourceController> ().SetResourceData (ResourcePool.ResourceTypes.WOOD, Random.Range (1, 5), "Wood", ResourcePoolController.woodColor, this.woodResourceCubePrefab);
			this.resourceObjects.Add (curResource);
		}


		int curResoruce = Random.Range (3, 7);
		for (int i = 0; i < curResoruce; i++) {
			GameObject curResource = Instantiate (this.goldResource, this.transform) as GameObject;
			curResource.transform.position = CubeMapController.Instance.GetRandomCubeGO ().transform.position;
			curResource.transform.Rotate (0, Random.Range (0, 360), 0);
			curResource.GetComponent<ResourceController> ().SetResourceData (ResourcePool.ResourceTypes.GOLD, Random.Range (1, 5), "Gold", ResourcePoolController.goldColor, goldCube);
			this.resourceObjects.Add (curResource);
		}
		curResoruce = Random.Range (3, 7);
		for (int i = 0; i < curResoruce; i++) {
			GameObject curResource = Instantiate (this.coalResource, this.transform) as GameObject;
			curResource.transform.position = CubeMapController.Instance.GetRandomCubeGO ().transform.position;
			curResource.transform.Rotate (0, Random.Range (0, 360), 0);
			curResource.GetComponent<ResourceController> ().SetResourceData (ResourcePool.ResourceTypes.COAL, Random.Range (1, 5), "Coal", ResourcePoolController.coalColor, coalCube);
			this.resourceObjects.Add (curResource);
		}
		curResoruce = Random.Range (3, 7);
		for (int i = 0; i < curResoruce; i++) {
			GameObject curResource = Instantiate (this.ironResource, this.transform) as GameObject;
			curResource.transform.position = CubeMapController.Instance.GetRandomCubeGO ().transform.position;
			curResource.transform.Rotate (0, Random.Range (0, 360), 0);
			curResource.GetComponent<ResourceController> ().SetResourceData (ResourcePool.ResourceTypes.IRON, Random.Range (1, 5), "Iron", ResourcePoolController.ironColor, ironCube);
			this.resourceObjects.Add (curResource);
		}

	}

	public void VillagerDied() {
		bool allDead = true;

		foreach (Villager vil in this.GameData.VillagersDict.Values) {
			if (vil.CurrentHitPoints > 0) {
				allDead = false;
				break;
			}
		}

		if (allDead) {
			this.GameOver = true;
			GameUIController.Instance.GameOver ();
		}

	}

	public void IncrementResourceOfType(ResourcePool.ResourceTypes type, int value) {
		//Debug.Log ("Incrementing Resource pool of Type: " + type);
		this.GameData.ResourcePoolData.IncrementPool (type, value);
		ResourcePoolController.Instance.UpdateTextFields ();

		bool allresourcesCollected = true;

		if (this.GameData.ResourcePoolData.CoalPool < this.GameData.ResourcePoolData.coalToWin) {
			allresourcesCollected = false;
		}
		if (this.GameData.ResourcePoolData.WoodPool < this.GameData.ResourcePoolData.woodToWin) {
			allresourcesCollected = false;
		}
		if (this.GameData.ResourcePoolData.goldToWin < this.GameData.ResourcePoolData.goldToWin) {
			allresourcesCollected = false;
		}
		if (this.GameData.ResourcePoolData.IronPool < this.GameData.ResourcePoolData.ironToWin) {
			allresourcesCollected = false;
		}
		if (this.GameData.ResourcePoolData.FoodPool < this.GameData.ResourcePoolData.foodToWin) {
			allresourcesCollected = false;
		}

		if (allresourcesCollected) {
			this.GameOver = true;
			GameUIController.Instance.GameWon ();
		}

	}


	public void OnReturnToMenu() {
		SceneManager.LoadScene ("Menu");
	}


}
