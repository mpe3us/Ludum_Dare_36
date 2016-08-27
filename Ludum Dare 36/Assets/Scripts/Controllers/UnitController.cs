using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	[SerializeField]
	private GameObject equippedTool;

	public Unit UnitData { get; private set; }

	public Vector3 LatestTargetPosition { get; private set; }
	public bool IsMovingTowardsTarget { get; private set; }

	[SerializeField]
	private Animator animController;

	private RaycastHit[] mouseRayHits;

	void Awake() {
		this.IsMovingTowardsTarget = false;
	}

	void Update() {
		if (this.IsMovingTowardsTarget) {
			Vector2 curPos = new Vector2 (this.transform.position.x, this.transform.position.z);
			Vector2 targetPos = new Vector2 (this.LatestTargetPosition.x, this.LatestTargetPosition.z);
			if (Vector2.Distance (curPos, targetPos) <= 0.1f) {
				this.IsMovingTowardsTarget = false;
				this.PlayAnimation ("Idle");
			} else {
				
				float distThisFrame = UnitData.MovementSpeed * Time.deltaTime;

				Vector3 curTargetPos = Vector3.MoveTowards (this.transform.position, this.LatestTargetPosition, distThisFrame);
				Vector3 raycastPos = new Vector3 (curTargetPos.x, 4.0f, curTargetPos.z);

				this.mouseRayHits = Physics.RaycastAll (raycastPos, Vector3.down, 10.0f);
				foreach (RaycastHit curHit in this.mouseRayHits) {
					if (curHit.transform.gameObject.tag == "Cube") {
						curTargetPos = new Vector3 (curHit.point.x, curHit.transform.position.y, curHit.point.z);
						break;
					}
				}

				//this.transform.Translate (direction.normalized * distThisFrame, Space.World);

				this.transform.position = curTargetPos;
				//this.transform.position = Vector3.Lerp (this.transform.position, curTargetPos, Time.deltaTime * 50.0f);

				Vector3 direction = this.LatestTargetPosition - this.transform.position;
				Quaternion rotation = Quaternion.LookRotation (direction);
				float curRotAngle = rotation.eulerAngles.y;
				float oldAngle = this.transform.localEulerAngles.y;

				if (oldAngle - curRotAngle > 180.0f) {
					curRotAngle -= 360.0f;
				} else if (oldAngle - curRotAngle < -180.0f) {
					curRotAngle -= 360.0f;
				}

				Vector3 newRot = new Vector3 (this.transform.localEulerAngles.x, curRotAngle, this.transform.localEulerAngles.z);
				this.transform.localEulerAngles = Vector3.Lerp (this.transform.localEulerAngles, newRot, Time.deltaTime * 5.0f);

				//this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * 5.0f);
			}


		}
	}

	public void SetUnitData(Unit data) {
		this.UnitData = data;
	}

	public void SetNewTargetPosition(Vector3 targetPos) {
		this.LatestTargetPosition = targetPos;
		this.IsMovingTowardsTarget = true;
		this.PlayAnimation ("Walk");
	}

	private void PlayAnimation (string triggerName) {
		this.animController.ResetTrigger ("Idle");
		this.animController.ResetTrigger ("PerformAction");
		this.animController.ResetTrigger ("Walk");
		this.animController.ResetTrigger ("TwoHanded");

		this.animController.SetTrigger (triggerName);
	}



}
