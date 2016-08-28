using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	public const float ActionRange = 1.1f;
	public const float TimeBetweenActions = 1.0f;

	private float curActionTime;

	public enum ActionMode {
		MOVE,
		GATHER,
		STORE,
		ATTACK
	}

	public ActionMode CurActionMode { get; private set; }

	[SerializeField]
	private GameObject equippedTool;

	public Unit UnitData { get; private set; }

	public Vector3 LatestTargetPosition { get; private set; }
	public bool IsMovingTowardsTarget { get; private set; }

	[SerializeField]
	private Animator animController;

	private RaycastHit[] mouseRayHits;

	public GameObject SelectionIndicator { get; set; }

	public GameObject HomeBase { get; private set; }

	private bool actionModeActivated; // True when we want to attack a certain target or gather a certain resource

	public GameObject CurrentTargetObject { get; private set; }

	[SerializeField]
	private GameObject toolSlot;
	[SerializeField]
	private GameObject resourceSlot;
	private GameObject currentResource;

	private ResourceController.ReturnResourceStruct CurrentRes;

	void Awake() {
		this.IsMovingTowardsTarget = false;
		this.actionModeActivated = false;
		this.curActionTime = 1.0f;
	}

	void Update() {

		// What action to perform
		switch (this.CurActionMode) {
		case ActionMode.MOVE:
			break;
		case ActionMode.GATHER:
			if (this.CurrentTargetObject.GetComponentInParent<ResourceController>() != null) {
				if (!this.IsMovingTowardsTarget && this.CurrentTargetObject.tag == "Resource" && currentResource == null && this.CurrentTargetObject.GetComponentInParent<ResourceController>().Amount > 0) {
					this.curActionTime += Time.deltaTime;
					if (this.curActionTime >= TimeBetweenActions) {
						this.PlayAnimation ("PerformAction");
						this.curActionTime = 0;
						this.CurrentRes = this.CurrentTargetObject.GetComponentInParent<ResourceController> ().TickResource (this);
						if (this.CurrentRes.resPrefab != null) {
							this.currentResource = Instantiate (this.CurrentRes.resPrefab, this.resourceSlot.transform.position, Quaternion.identity) as GameObject;
							this.currentResource.transform.SetParent (this.resourceSlot.transform, true);
							this.currentResource.transform.localPosition = Vector3.zero;
							this.currentResource.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);
						}
					}
				}
				else if (this.currentResource != null) {
					this.SetNewTargetPosition (HomeBase.transform.position, ActionMode.STORE, this.HomeBase);
				}
			}
			else if (this.currentResource != null) {
				this.SetNewTargetPosition (HomeBase.transform.position, ActionMode.STORE, this.HomeBase);
			}
			break;
		case ActionMode.STORE:
			if (!this.IsMovingTowardsTarget) {
				if (this.currentResource != null) {
					GameController.Instance.IncrementResourceOfType (CurrentRes.resType, 1);
					Destroy (this.currentResource);
				} else {
					if (CurrentRes.resReference != null) {
						if (CurrentRes.resReference.GetComponent<ResourceController>().Amount > 0) {
							this.SetNewTargetPosition (CurrentRes.resReference.transform.position, ActionMode.GATHER, CurrentRes.resReference);
							return;
						}
					}
					this.CurActionMode = ActionMode.MOVE;
				}
			}
			break;
		case ActionMode.ATTACK:
			break;
		default:
			break;
		}

		// Moving towards target
		if (this.IsMovingTowardsTarget) {
			Vector2 curPos = new Vector2 (this.transform.position.x, this.transform.position.z);
			Vector2 targetPos = new Vector2 (this.LatestTargetPosition.x, this.LatestTargetPosition.z);
			if (Vector2.Distance (curPos, targetPos) <= 0.1f || (this.actionModeActivated && Vector2.Distance (curPos, targetPos) <= ActionRange)) {
				this.IsMovingTowardsTarget = false;
				this.PlayAnimation ("Idle");
				this.DestinationReached ();
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

				curRotAngle += 180.0f; // FIXME: Needed because of wrong model rotation

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

	private void DestinationReached() {

		switch (this.CurActionMode) {
		case ActionMode.MOVE:
			break;
		case ActionMode.GATHER:
			break;
		case ActionMode.ATTACK:
			break;
		default:
			break;
		}
	}

	public void SetUnitData(Unit data, GameObject homeBaseGO) {
		this.UnitData = data;
		this.HomeBase = homeBaseGO;
	}

	public void SetNewTargetPosition(Vector3 targetPos, ActionMode newActionMode, GameObject targetObject) {
		this.LatestTargetPosition = targetPos;
		this.IsMovingTowardsTarget = true;
		this.CurActionMode = newActionMode;
		this.CurrentTargetObject = targetObject;
		this.PlayAnimation ("Walk");

		switch (this.CurActionMode) {
		case ActionMode.MOVE:
			this.actionModeActivated = false;
			break;
		case ActionMode.GATHER:
			this.actionModeActivated = true;
			break;
		case ActionMode.ATTACK:
			this.actionModeActivated = true;
			break;
		case ActionMode.STORE:
			this.actionModeActivated = true;
			break;
		default:
			break;
		}
	}

	private void PlayAnimation (string triggerName) {
		this.animController.ResetTrigger ("Idle");
		this.animController.ResetTrigger ("PerformAction");
		this.animController.ResetTrigger ("Walk");
		this.animController.ResetTrigger ("TwoHanded");

		this.animController.SetTrigger (triggerName);
	}



}
