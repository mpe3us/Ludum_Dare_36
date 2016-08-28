using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

	public static MouseController Instance;

	public List<GameObject> SelectedVillagers;

	public GameObject MouseOverGameObject { get; private set; }
	public GameObject MouseOverCube { get; private set; }

	private Ray mouseRay;
	private RaycastHit[] mouseRayHits;

	[SerializeField]
	private GameObject MovementActionIndicator;

	public Vector3 MousePositionOnMap { get; private set;}
	bool rayAlreadyHitCubeThisUpdate;

	private bool newGameObjectSelectedThisUpdate;

	[SerializeField]
	private GameObject selectionIndicatorPrefab;

	[SerializeField]
	private Color activeSelectionColor;

	void Awake() {

		if (Instance != null) {
			Debug.LogError ("There should only be on MouseController");
		}

		Instance = this;

	}

	void Update() {

		if (GameController.Instance.GameOver) {
			return;
		}

		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

		this.mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		this.mouseRayHits = Physics.RaycastAll (this.mouseRay, 100.0f);

		this.rayAlreadyHitCubeThisUpdate = false;
		this.newGameObjectSelectedThisUpdate = false;

		foreach (RaycastHit hit in this.mouseRayHits) {
			this.MouseOverNewObject (hit.transform.gameObject, hit);
		}

		if (Input.GetMouseButtonDown (0)) {
			this.HandleLeftClick ();
		}
		else if (Input.GetMouseButtonUp(1)) {
			this.HandleRightClick ();
		}

		// Used to reset mouse over indicator
		if (!this.newGameObjectSelectedThisUpdate && this.MouseOverGameObject != null) {
			this.MouseOverGameObject = null;
		}

	}

	private void HandleRightClick() {

		// TODO: Check that we don't have the mouse over another gameobject which has HandleRight click behavior

		bool actionPerformed = false;

		if (this.MouseOverGameObject != null) {
			if (this.MouseOverGameObject.tag == "Resource") {
				actionPerformed = true;

				bool firstMove = true;
				foreach (GameObject go in this.SelectedVillagers) {

					go.GetComponentInParent<UnitController> ().SetNewTargetPosition (this.MouseOverGameObject.transform.position, UnitController.ActionMode.GATHER, this.MouseOverGameObject);

					if (firstMove) {
						GameObject temp = (GameObject) Instantiate (MovementActionIndicator, this.MousePositionOnMap, Quaternion.identity);
						Destroy (temp, 0.3f);
					}

					firstMove = false;
				}
			}
			else if (this.MouseOverGameObject.tag == "Base") {
				actionPerformed = true;

				bool firstMove = true;
				foreach (GameObject go in this.SelectedVillagers) {

					go.GetComponentInParent<UnitController> ().SetNewTargetPosition (GameController.Instance.BaseGO.transform.position, UnitController.ActionMode.STORE, GameController.Instance.BaseGO);

					if (firstMove) {
						GameObject temp = (GameObject) Instantiate (MovementActionIndicator, this.MousePositionOnMap, Quaternion.identity);
						Destroy (temp, 0.3f);
					}

					firstMove = false;
				}
			}
			else if (this.MouseOverGameObject.tag == "Barbarian") {
				actionPerformed = true;

				bool firstMove = true;
				foreach (GameObject go in this.SelectedVillagers) {

					go.GetComponentInParent<UnitController> ().SetNewTargetPosition (this.MouseOverGameObject.transform.position, UnitController.ActionMode.ATTACK, this.MouseOverGameObject);

					if (firstMove) {
						GameObject temp = (GameObject) Instantiate (MovementActionIndicator, this.MousePositionOnMap, Quaternion.identity);
						Destroy (temp, 0.3f);
					}

					firstMove = false;
				}
			}
		}

		if (this.MouseOverCube != null && !actionPerformed) {
			bool firstMove = true;
			foreach (GameObject go in this.SelectedVillagers) {
				
				go.GetComponentInParent<UnitController> ().SetNewTargetPosition (this.MousePositionOnMap, UnitController.ActionMode.MOVE, this.MouseOverCube);

				if (firstMove) {
					GameObject temp = (GameObject) Instantiate (MovementActionIndicator, this.MousePositionOnMap, Quaternion.identity);
					Destroy (temp, 0.3f);
				}

				firstMove = false;
			}
		}

	}

	private void HandleLeftClick(bool clearEarlierSelections = true) {

		if (this.MouseOverGameObject == null || !this.newGameObjectSelectedThisUpdate) {
			this.ClearSelectedVillagers();
			GameUIController.Instance.ClearSelection ();
			return;
		}

		if (this.MouseOverGameObject.tag == "Villager") {
			if (clearEarlierSelections) {
				this.ClearSelectedVillagers ();
				this.AddSelectedVillagerObject (this.MouseOverGameObject);
			} else {
				this.AddSelectedVillagerObject (this.MouseOverGameObject);
			}
		} else {
			// Clear selected objects
			this.ClearSelectedVillagers();
		}
	}

	private void AddSelectedVillagerObject(GameObject go) {
		this.SelectedVillagers.Add (go);
		go.GetComponentInParent<UnitController> ().SelectionIndicator = Instantiate (this.selectionIndicatorPrefab, GameUIController.Instance.transform) as GameObject;
		go.GetComponentInParent<UnitController> ().SelectionIndicator.GetComponent<SelectionIndicator> ().SetObjectToFollow (go, this.activeSelectionColor);
	}

	private void ClearSelectedVillagers() {
		foreach (GameObject go in this.SelectedVillagers) {
			if (go.GetComponentInParent<UnitController> ().SelectionIndicator != null) {
				Destroy (go.GetComponentInParent<UnitController> ().SelectionIndicator);
			}
		}
		this.SelectedVillagers.Clear ();
		this.SelectedVillagers = new List<GameObject> ();
	}

	private void MouseOverNewObject(GameObject go, RaycastHit hitInfo) {
		
		if (go.tag == "Cube") {
			this.MouseOverNewCube (go, hitInfo);
		}

		// Selectable object layer
		if (go.layer == 8) {
			this.MouseOverNewSelectableObject (go);
		}

	}

	private void MouseOverNewSelectableObject(GameObject go) {
		this.newGameObjectSelectedThisUpdate = true;

		if (this.MouseOverGameObject == go) {
			return;
		}

		this.MouseOverGameObject = go;
		GameUIController.Instance.MouseOverNewObject (go);
	}

	private void MouseOverNewCube(GameObject go, RaycastHit hitInfo) {
		if (this.MouseOverCube == go || this.rayAlreadyHitCubeThisUpdate) {
			return;
		}

		this.MousePositionOnMap = hitInfo.point;
		this.rayAlreadyHitCubeThisUpdate = true;
		this.MouseOverCube = go;
	}

}
