using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.U2D;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

public class RoomManager : MonoBehaviour
{
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	private int horizontalTiles;
	private int verticalTiles;

	//Arrays of tile prefabs.
	public GameObject[] floorTiles;
	public GameObject[] topWallsTiles;
	public GameObject[] bottomWallsTiles;
	public GameObject[] leftWallsTiles;
	public GameObject[] rightWallsTiles;
	public GameObject topLeftCornerTile;
	public GameObject topRightCornerTile;
	public GameObject bottomLeftCornerTile;
	public GameObject bottomRightCornerTile;

	//A variable to store a reference to the transform of our Board object.
	private Transform boardHolder;
	//A list of possible locations to place tiles.
	private List<Vector3> gridPositions = new List<Vector3>();

    private void Awake()
    {
		PixelPerfectCamera pixPerfCam = Camera.main.GetComponent<PixelPerfectCamera>();

		float virtualWith = pixPerfCam.refResolutionX;
		float virtualHight = pixPerfCam.refResolutionY;
		horizontalTiles = (int)(virtualWith / pixPerfCam.assetsPPU) - 2;
		verticalTiles = (int)(virtualHight / pixPerfCam.assetsPPU) - 2;
		//Debug.LogFormat("columns {0} Rows {1}", horizontalTiles, verticalTiles);

		GenerateWallsAndFloors();
    }

/*
 * Generates the walls and floors of the room, randomizing the various varieties
 * of said tiles for visual variety.
 */
void GenerateWallsAndFloors()
    {
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject("Room").transform;

        for (int y = 1; y <= verticalTiles; y++)
        {
            for (int x = 1; x <= horizontalTiles; x++)
            {
                GameObject toInstantiate;

				// Corner tiles
                if (x == 1 && y == 1)
                {
					toInstantiate = bottomLeftCornerTile;
                }
				else if (x == 1 && y == verticalTiles)
                {
					toInstantiate = topLeftCornerTile;
                }
				else if (x == horizontalTiles && y == 1)
                {
					toInstantiate = bottomRightCornerTile;
                }
				else if ( x == horizontalTiles && y == verticalTiles)
                {
					toInstantiate = topRightCornerTile;
                }
				//random left - hand walls, right walls, top, bottom
				else if (x == 1)
                {
					toInstantiate = leftWallsTiles[Random.Range(0, leftWallsTiles.Length)];
                }
				else if (x == horizontalTiles)
                {
					toInstantiate = rightWallsTiles[Random.Range(0, rightWallsTiles.Length)];
                }
				else if (y == 1)
                {
					toInstantiate = bottomWallsTiles[Random.Range(0, topWallsTiles.Length)];
                }
				else if (y == verticalTiles)
                {
					toInstantiate = topWallsTiles[Random.Range(0, bottomWallsTiles.Length)];
                }
				// if it's not a corner or a wall tile, be it a floor tile
                else
                {
					toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

				// TODO: Consider adjusting camera position (ONCE and forall!) instead of each tile

				// Adjust the offset for each tile's position so the result board is camera centered.
				// First substracting 1 to each component x,y to be 0 based;
				// Ex: first bottomLeftCorner tile position was (1,1) and we want it to be (0,0) and so on.
				// Finally we substract half of the total tiles.
				float offsetX = (float)x - 1 - ((float)horizontalTiles / 2);
                float offsetY = (float)y - 1 - ((float)verticalTiles / 2);
				
                GameObject instance = Instantiate(toInstantiate, new Vector3(offsetX, offsetY, 0f), Quaternion.identity);

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent(boardHolder);
			}
		}
	}
}
