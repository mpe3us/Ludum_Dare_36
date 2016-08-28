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
	[SerializeField]
	private GameObject selectedResourcePrefab;
	[SerializeField]
	private GameObject selectedBasePrefab;

	private GameObject curSelectedObject;

	[SerializeField]
	private GameObject resourcePoolPanel;

	[SerializeField]
	private GameObject overlayUI;
	[SerializeField]
	private GameObject defeatPanel;
	[SerializeField]
	private GameObject victoryPanel;

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
			this.selectedObjectNameText.text = curController.UnitData.Name.ToUpper ();
			this.selectedObjectNameText.color = Color.white; 
			this.currentObjectInfo = Instantiate (this.selectedUnitPrefab, this.selectedObjectParent.transform) as GameObject;
			this.currentObjectInfo.GetComponent<UnitInfoPanel> ().SetUnitData (curController.UnitData);
			this.currentObjectInfo.transform.position = this.selectedObjectParent.transform.position;
			this.selectedObjectPanel.SetActive (true);
		} else if (go.GetComponentInParent<ResourceController> () != null) {
			ResourceController curController = go.GetComponentInParent<ResourceController> ();
			this.selectedObjectNameText.text = curController.Name.ToUpper ();
			this.selectedObjectNameText.color = curController.ResColor;
			this.currentObjectInfo = Instantiate (this.selectedResourcePrefab, this.selectedObjectParent.transform) as GameObject;
			this.currentObjectInfo.GetComponent<ResourceInfoPanel> ().SetResourceData (curController);
			this.currentObjectInfo.transform.position = this.selectedObjectParent.transform.position;
			this.selectedObjectPanel.SetActive (true);
		} else if (go.GetComponentInParent<HomeBaseController> () != null) {
			HomeBaseController curController = go.GetComponentInParent<HomeBaseController> ();
			this.selectedObjectNameText.text = "ANCIENT WAGON";
			this.selectedObjectNameText.color = Color.white; 
			this.currentObjectInfo = Instantiate (this.selectedBasePrefab, this.selectedObjectParent.transform) as GameObject;
			this.currentObjectInfo.GetComponent<BaseInfoPanel> ().SetData (curController);
			this.currentObjectInfo.transform.position = this.selectedObjectParent.transform.position;
			this.selectedObjectPanel.SetActive (true);
		}
	}


	public void ClearSelection() {
		this.selectedObjectPanel.SetActive (false);
		this.curSelectedObject = null;
		Destroy (this.currentObjectInfo);
	}

	public void GameOver() {
		this.overlayUI.SetActive (true);
		this.defeatPanel.SetActive (true);
	}

	public void GameWon() {
		this.overlayUI.SetActive (true);
		this.victoryPanel.SetActive (true);
	}


}
