using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {

	public static GameUIController Instance;

	[SerializeField]
	private GameObject selectedObjectPanel;
	[SerializeField]
	private Text selectedObjectNameText;
	[SerializeField]
	private GameObject selectedObjectParent;

	private GameObject currentObjectInfo;

	[SerializeField]
	private GameObject selectedUnitPrefab;

	private GameObject curSelectedObject;

	void Awake() {
		if (Instance != null) {
			Debug.LogError ("There should only be one GameUIController");
		}

		Instance = this;

		this.selectedObjectPanel.SetActive (false);
	}


	public void MouseOverNewObject(GameObject go) {

		if (this.curSelectedObject == go) {
			return;
		}

		this.curSelectedObject = go;
		Destroy (this.currentObjectInfo);

		if (go.GetComponentInParent<UnitController> () != null) {
			UnitController curController = go.GetComponentInParent<UnitController> ();
			this.selectedObjectNameText.text = curController.UnitData.Name;
			this.currentObjectInfo = Instantiate (this.selectedUnitPrefab, this.selectedObjectParent.transform) as GameObject;
			this.currentObjectInfo.GetComponent<UnitInfoPanel> ().SetUnitData (curController.UnitData);
			this.currentObjectInfo.transform.position = this.selectedObjectParent.transform.position;
			this.selectedObjectPanel.SetActive (true);
		}
	}


	public void ClearSelection() {
		this.selectedObjectPanel.SetActive (false);
		this.curSelectedObject = null;
		Destroy (this.currentObjectInfo);
	}

}
