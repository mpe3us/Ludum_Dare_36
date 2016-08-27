using UnityEngine;
using System.Collections;

public class CubeMapController : MonoBehaviour {

	public static CubeMapController Instance;

	[SerializeField]
	private GameObject baseCubePrefab;

	public CubeMap CubeMapData { get; private set; }

	public GameObject[,] CubeGameObjects { get; private set; }

	[SerializeField]
	private GameObject surroundingPlane;

	void Awake() {
		if (Instance != null) {
			Debug.LogError ("There should only be one instance of the CubeMapController");
		}

		Instance = this;

		this.CubeMapData = new CubeMap (50, 50);
		this.CubeGameObjects = new GameObject[this.CubeMapData.Width, this.CubeMapData.Depth];

		GameObject curCubeGO = null;
		Cube curCubeData = null;

		for (int x = 0; x < this.CubeMapData.Width; x++) {
			for (int y = 0; y < this.CubeMapData.Depth; y++) {
				curCubeData = this.CubeMapData.GetCube (x, y);
				curCubeGO = Instantiate (this.baseCubePrefab, this.transform) as GameObject;
				curCubeGO.transform.position = new Vector3 (curCubeData.Width, Random.Range(0.0f, 0.2f), curCubeData.Depth);
				this.CubeGameObjects [x, y] = curCubeGO;
			}
		}

	}

	public GameObject GetCubeGO(int width, int depth) {

		GameObject returnCube = null;

		if (width >= this.CubeMapData.Width || width < 0 || depth >= this.CubeMapData.Depth || depth < 0) {
			Debug.LogError ("CubeMapController::GetCubeGO -- The given coordinates: (" + width + "," + depth + ") are out of bounds");
			return returnCube;
		}

		returnCube = this.CubeGameObjects [width, depth];

		if (returnCube == null) {
			Debug.LogError ("CubeMapController::GetCubeGO -- The given coordinates: (" + width + "," + depth + ") does not contain a Cube");
		}

		return returnCube;



	}

	// Use this for initialization
	void Start () {
	
		// Set initial position of the camera
		CameraController.Instance.SetInitialPos (this.CubeMapData.Width / 2, this.CubeMapData.Depth / 2);
		this.surroundingPlane.transform.position = new Vector3 ((float)this.CubeMapData.Width / 2, -0.2f, (float)this.CubeMapData.Depth / 2);
		this.surroundingPlane.transform.localScale = new Vector3 ((float)this.CubeMapData.Width * 2, 1.0f, (float)this.CubeMapData.Depth * 2);

		// After the map has initialized init all GameController related stuff
		GameController.Instance.InitGame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
