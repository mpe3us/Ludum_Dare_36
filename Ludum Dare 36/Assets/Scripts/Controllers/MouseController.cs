using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	void Awake() {

		if (Instance != null) {
			Debug.LogError ("There should only be on MouseController");
		}

		Instance = this;

	}

	void Update() {

		this.mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		this.mouseRayHits = Physics.RaycastAll (this.mouseRay, 100.0f);

		this.rayAlreadyHitCubeThisUpdate = false;

		foreach (RaycastHit hit in this.mouseRayHits) {
			this.MouseOverNewObject (hit.transform.gameObject, hit);
		}

		if (Input.GetMouseButtonDown (0)) {
			this.HandleLeftClick ();
		}
		else if (Input.GetMouseButtonUp(1)) {
			this.HandleRightClick ();
		}
	}

	private void HandleRightClick() {

		// TODO: Check that we don't have the mouse over another gameobject which has HandleRight click behavior

		if (this.MouseOverCube != null) {
			bool firstMove = true;
			foreach (GameObject go in this.SelectedVillagers) {
				
				go.GetComponentInParent<UnitController> ().SetNewTargetPosition (this.MousePositionOnMap);

				if (firstMove) {
					GameObject temp = (GameObject) Instantiate (MovementActionIndicator, this.MousePositionOnMap, Quaternion.identity);
					Destroy (temp, 0.3f);
				}

				firstMove = false;
			}
		}

	}

	private void HandleLeftClick(bool clearEarlierSelections = true) {

		if (this.MouseOverGameObject == null) {
			this.ClearSelectedVillagers();
			return;
		}

		if (this.MouseOverGameObject.tag == "Villager") {
			if (clearEarlierSelections) {
				this.ClearSelectedVillagers ();
				this.SelectedVillagers.Add (this.MouseOverGameObject);
			} else {
				this.SelectedVillagers.Add (this.MouseOverGameObject);
			}
		} else {
			// Clear selected objects
			this.ClearSelectedVillagers();
		}
	}

	private void ClearSelectedVillagers() {
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
		if (this.MouseOverGameObject == go) {
			return;
		}

		this.MouseOverGameObject = go;
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
