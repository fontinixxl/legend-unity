using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

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

    private int virtualWith;
    private int virtualHight;

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

	// PlayerPrefab
	public GameObject player;

	// DoorWays
	public GameObject doorWayLeft;
	public GameObject doorWayRight;
	public GameObject doorWayTop;
	public GameObject doorWayBottom;
	private readonly string[] doorWayDirections = new string[] { "Left", "Right", "Top", "Bottom" };

	//A variable to store a reference to the transform of our Room object.
	private Transform roomHolder;

	//A list of possible locations to place tiles.
	private List<Vector3> gridPositions = new List<Vector3>();


    private void Awake()
    {
		PixelPerfectCamera pixPerfCam = Camera.main.GetComponent<PixelPerfectCamera>();
		virtualWith = pixPerfCam.refResolutionX;
		virtualHight = pixPerfCam.refResolutionY;
		horizontalTiles = (virtualWith / pixPerfCam.assetsPPU) - 2;
		verticalTiles = (virtualHight / pixPerfCam.assetsPPU) - 2;
        //Debug.LogFormat("horizontalTiles {0} verticalTiles {1}", horizontalTiles, verticalTiles);

	}

    private void Start()
    {
        GenerateWallsAndFloors();
		GenerateDoorWays();
		Instantiate(player, player.transform.position, Quaternion.identity);
    }
    /*
	 * Generates the walls and floors of the room, randomizing the various varieties
	 * of said tiles for visual variety.
	 */
    void GenerateWallsAndFloors()
    {
		//Instantiate Board and set boardHolder to its transform.
		roomHolder = new GameObject("Room").transform;

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

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
				instance.transform.SetParent(roomHolder);
			}
		}
	}

	// TODO: Extract to DoorWay Class
	void GenerateDoorWays()
    {
		float x, y;
		GameObject instance;
		GameObject doorWay;

		Transform doorWayHolder = new GameObject("DoorWay").transform;
		doorWayHolder.SetParent(roomHolder);

		foreach (var direction in doorWayDirections)
        {
			switch (direction)
			{
				case "Left":
					x = 0;
					y = verticalTiles * 0.5f;
					doorWay = doorWayLeft;
					break;
				case "Right":
					x = horizontalTiles;
					y = verticalTiles * 0.5f;
					doorWay = doorWayRight;
					break;
				case "Top":
					x = horizontalTiles * 0.5f;
					y = verticalTiles;
					doorWay = doorWayTop;
					break;
				case "Bottom":
					x = horizontalTiles * 0.5f;
					y = 0;
					doorWay = doorWayBottom;
					break;
				default:
					Debug.LogErrorFormat("DoorWay ''{0}'' Prefab doesn't exist!", direction);
					return;
			}

			instance = Instantiate(doorWay, new Vector3(x, y, 0f), Quaternion.identity);
			instance.transform.SetParent(doorWayHolder);
		}
    }
}