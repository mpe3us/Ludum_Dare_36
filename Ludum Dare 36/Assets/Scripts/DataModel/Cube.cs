using UnityEngine;
using System.Collections;

public class Cube {

	private static int CurrentID;

	public enum CubeTerrainType {
		GRASS
	}

	public CubeTerrainType TerrainType { get; private set; } 

	public bool Traversable { get; private set; }

	public int CubeID { get; private set; }

	public int Width { get; private set; } // World x
	public int Depth { get; private set; }  // world z
	public int Height { get; private set; } // world y

	public Cube(int width, int depth, int height, CubeTerrainType terrainType, bool traversable = true) {
		CurrentID++;

		this.Traversable = traversable;

		this.TerrainType = terrainType;

		this.CubeID = CurrentID;

		this.Width = width;
		this.Depth = depth;
		this.Height = height;

	}

	public void SetTraversable(bool value) {
		this.Traversable = value;
	}

}
