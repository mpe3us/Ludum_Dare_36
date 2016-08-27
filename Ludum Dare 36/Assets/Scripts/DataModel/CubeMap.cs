using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeMap {

	public int Width { get; private set; } // x-coordinate in world coordinates
	public int Depth { get; private set; } // z-coordinate in world coordinates

	public int NontraversableArea { get; private set; } // Space which can not be traversed by units

	public Cube[,] Cubes { get; private set; }

	public CubeMap(int width, int depth, int nontraversableArea = 16) {

		this.Width = width + nontraversableArea * 2;
		this.Depth = depth + nontraversableArea * 2;
		this.NontraversableArea = nontraversableArea;

		this.Cubes = new Cube[this.Width, this.Depth];

		this.GenerateMap ();

	}

	private void GenerateMap() {

		Cube curCube = null;

		for (int x = 0; x < this.Width; x++) {
			for (int y = 0; y < this.Depth; y++) {
				curCube = new Cube (x, y, 0, Cube.CubeTerrainType.GRASS);

				if (x < this.NontraversableArea || x >= this.Width - this.NontraversableArea) {
					curCube.SetTraversable (false);
				}
				if (y < this.NontraversableArea || x >= this.Depth - this.NontraversableArea) {
					curCube.SetTraversable (false);
				}

				Cubes [x, y] = curCube;
			}
		}	
	}



	public Cube GetCube(int width, int depth) {

		Cube returnCube = null;

		if (width >= this.Width || width < 0 || depth >= this.Depth || depth < 0) {
			Debug.LogError ("CubeMap::GetCube -- The given coordinates: (" + width + "," + depth + ") are out of bounds");
			return returnCube;
		}

		returnCube = this.Cubes [width, depth];

		if (returnCube == null) {
			Debug.LogError ("CubeMap::GetCube -- The given coordinates: (" + width + "," + depth + ") does not contain a Cube");
		}

		return returnCube;

	}





}
