using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionIndicator : MonoBehaviour {

	[SerializeField]
	private bool followMouseOverSelectableObject = false; // If true, then this object follows the mouse over object

	public GameObject ObjectToFollow { get; private set; }

	private bool objectToFollowIsSet;

	public Image selectionImage { get; private set; }
	private RectTransform rt;

	void Awake() {
		this.selectionImage = this.GetComponent<Image> ();
		this.rt = this.GetComponent<RectTransform> ();

		this.selectionImage.enabled = false;

		this.objectToFollowIsSet = false;
	}

	void LateUpdate() {
		if (this.followMouseOverSelectableObject) {
			this.SetObjectToFollow(MouseController.Instance.MouseOverGameObject, Color.white);	
		}
		else if (this.objectToFollowIsSet && this.ObjectToFollow == null && !this.followMouseOverSelectableObject) {
			Destroy (this.gameObject);
			return;
		}

		this.SetIndicatorToObject ();
	}

	public void SetObjectToFollow(GameObject go, Color color) {
		
		if (go == this.ObjectToFollow) {
			return;
		}
			
		if (go != null) {

			this.ObjectToFollow = go;
			this.objectToFollowIsSet = true;
			this.selectionImage.color = color;

			this.selectionImage.enabled = true;
		} else {
			this.ObjectToFollow = null;
			this.objectToFollowIsSet = false;

			this.selectionImage.enabled = false;
		}
	}

	public void SetIndicatorToObject() {
		if (this.ObjectToFollow == null) {
			return;
		}

		if (this.ObjectToFollow.GetComponent<BoxCollider> () == null) {
			Debug.LogError ("GameObject: " + this.ObjectToFollow.name + " does not have a box collider");
			return;
		}

		Rect visualRect = BoundsInScreenSpace (this.ObjectToFollow.GetComponent<BoxCollider> ().bounds);
		this.SetRectTransformToVisualRect (visualRect);
	}

	static Vector3[] screenSpaceCorners;
	public static Rect BoundsInScreenSpace(Bounds worldBounds) {
		// This is the space occupied by the object's visuals
		// in WORLD space.
		Bounds curBounds = worldBounds;

		if(screenSpaceCorners == null)
			screenSpaceCorners = new Vector3[8];

		Camera theCamera = Camera.main;

		// For each of the 8 corners of our renderer's world space bounding box,
		// convert those corners into screen space.
		screenSpaceCorners[0] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x + curBounds.extents.x, curBounds.center.y + curBounds.extents.y, curBounds.center.z + curBounds.extents.z ) );
		screenSpaceCorners[1] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x + curBounds.extents.x, curBounds.center.y + curBounds.extents.y, curBounds.center.z - curBounds.extents.z ) );
		screenSpaceCorners[2] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x + curBounds.extents.x, curBounds.center.y - curBounds.extents.y, curBounds.center.z + curBounds.extents.z ) );
		screenSpaceCorners[3] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x + curBounds.extents.x, curBounds.center.y - curBounds.extents.y, curBounds.center.z - curBounds.extents.z ) );
		screenSpaceCorners[4] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x - curBounds.extents.x, curBounds.center.y + curBounds.extents.y, curBounds.center.z + curBounds.extents.z ) );
		screenSpaceCorners[5] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x - curBounds.extents.x, curBounds.center.y + curBounds.extents.y, curBounds.center.z - curBounds.extents.z ) );
		screenSpaceCorners[6] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x - curBounds.extents.x, curBounds.center.y - curBounds.extents.y, curBounds.center.z + curBounds.extents.z ) );
		screenSpaceCorners[7] = theCamera.WorldToScreenPoint( new Vector3( curBounds.center.x - curBounds.extents.x, curBounds.center.y - curBounds.extents.y, curBounds.center.z - curBounds.extents.z ) );

		// Now find the min/max X & Y of these screen space corners.
		float min_x = screenSpaceCorners[0].x;
		float min_y = screenSpaceCorners[0].y;
		float max_x = screenSpaceCorners[0].x;
		float max_y = screenSpaceCorners[0].y;

		for (int i = 1; i < 8; i++) {
			if(screenSpaceCorners[i].x < min_x) {
				min_x = screenSpaceCorners[i].x;
			}
			if(screenSpaceCorners[i].y < min_y) {
				min_y = screenSpaceCorners[i].y;
			}
			if(screenSpaceCorners[i].x > max_x) {
				max_x = screenSpaceCorners[i].x;
			}
			if(screenSpaceCorners[i].y > max_y) {
				max_y = screenSpaceCorners[i].y;
			}
		}

		return Rect.MinMaxRect( min_x, min_y, max_x, max_y );
	}

	private void SetRectTransformToVisualRect(Rect visualRect) {
		// Center the position between the maxs and mins
		this.rt.position = new Vector2 (visualRect.xMin + ((visualRect.xMax - visualRect.xMin) / 2), visualRect.yMin + ((visualRect.yMax - visualRect.yMin) / 2));
		// Now set the size
		this.rt.sizeDelta = new Vector2 (visualRect.width / this.rt.lossyScale.x, visualRect.height / this.rt.lossyScale.y);
	}


}
