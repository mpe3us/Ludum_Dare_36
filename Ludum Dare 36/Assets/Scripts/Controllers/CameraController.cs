using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController Instance;

	[SerializeField]
	private Camera mainCam;

	[SerializeField]
	private float cameraSpeed = 100.0f;

	private float distThisFrame;

	[SerializeField]
	private float rotationSpeed = 50.0f;

	void Awake() {
		if (Instance != null) {
			Debug.LogError ("There should only be one CameraController");
		}

		Instance = this;
	}

	// Update is called once per frame
	void Update () {

		this.distThisFrame = this.cameraSpeed * Time.deltaTime;

		this.transform.Translate(Input.GetAxis("Vertical") * this.distThisFrame, 0, -Input.GetAxis("Horizontal") * this.distThisFrame);

//		this.transform.Translate (Input.GetAxis("Horizontal") * this.distThisFrame, 
//			Input.GetAxis ("Vertical") * Mathf.Sin(this.transform.rotation.eulerAngles.x * (3.14f / 180 )) * this.distThisFrame,
//			Input.GetAxis("Vertical") * Mathf.Cos(this.transform.rotation.eulerAngles.x * (3.14f / 180)) * this.distThisFrame,
//			Space.Self);

		float rotThisFrame = this.rotationSpeed * Time.deltaTime;
		if (Input.GetKey ("q")) {
			//this.mainCam.transform.RotateAround (this.transform.position, Vector3.up, rotThisFrame);
			this.transform.Rotate (0, rotThisFrame, 0);
		}
		else if (Input.GetKey("e")) {
			//this.mainCam.transform.RotateAround (this.transform.position, Vector3.up, -rotThisFrame);
			this.transform.Rotate (0, -rotThisFrame, 0);
		}

		Vector3 relativePos = this.transform.position - this.mainCam.transform.position;
		Quaternion rotation = Quaternion.LookRotation (relativePos);

		float curCameraAngle = rotation.eulerAngles.x;

		this.mainCam.transform.localEulerAngles = new Vector3 (curCameraAngle, this.mainCam.transform.localEulerAngles.y, this.mainCam.transform.localEulerAngles.z);
	}

	public void SetInitialPos(int middleX, int middleZ) {

		this.transform.position = new Vector3 ((float)middleX, 0, (float)middleZ);
		this.transform.Rotate (0, -90, 0);

	}
}
