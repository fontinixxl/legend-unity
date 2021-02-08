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

    [HideInInspector]
    public int HorizontalTiles { get; private set; }
    [HideInInspector]
    public int VerticalTiles { get; private set; }

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
    public List<Doorway> DoorwayPrefabs;
    private List<Doorway> _doorways;

    //A variable to store a reference to the transform of our Room object.
    private Transform roomHolder;

    //A list of possible locations to place tiles.
    private List<Vector3> gridPositions = new List<Vector3>();

    private void Awake()
    {
        PixelPerfectCamera pixPerfCam = Camera.main.GetComponent<PixelPerfectCamera>();
        virtualWith = pixPerfCam.refResolutionX;
        virtualHight = pixPerfCam.refResolutionY;
        HorizontalTiles = (virtualWith / pixPerfCam.assetsPPU) - 2;
        VerticalTiles = (virtualHight / pixPerfCam.assetsPPU) - 2;

        _doorways = new List<Doorway>();
        //Debug.LogFormat("horizontalTiles {0} verticalTiles {1}", horizontalTiles, verticalTiles);
    }

    private void Start()
    {
        GenerateWallsAndFloors();
        GenerateDoorways();
        Instantiate(player, player.transform.position, Quaternion.identity);
    }

    /*
	 * Generate the walls and floor of the room, randomizing the various varieties
	 * of said tiles for visual variety.
	 */
    void GenerateWallsAndFloors()
    {
        //Instantiate Board and set boardHolder to its transform.
        roomHolder = new GameObject("Room").transform;

        for (int y = 1; y <= VerticalTiles; y++)
        {
            for (int x = 1; x <= HorizontalTiles; x++)
            {
                GameObject toInstantiate;

                // Corner tiles
                if (x == 1 && y == 1)
                {
                    toInstantiate = bottomLeftCornerTile;
                }
                else if (x == 1 && y == VerticalTiles)
                {
                    toInstantiate = topLeftCornerTile;
                }
                else if (x == HorizontalTiles && y == 1)
                {
                    toInstantiate = bottomRightCornerTile;
                }
                else if (x == HorizontalTiles && y == VerticalTiles)
                {
                    toInstantiate = topRightCornerTile;
                }
                //random left - hand walls, right walls, top, bottom
                else if (x == 1)
                {
                    toInstantiate = leftWallsTiles[Random.Range(0, leftWallsTiles.Length)];
                }
                else if (x == HorizontalTiles)
                {
                    toInstantiate = rightWallsTiles[Random.Range(0, rightWallsTiles.Length)];
                }
                else if (y == 1)
                {
                    toInstantiate = bottomWallsTiles[Random.Range(0, topWallsTiles.Length)];
                }
                else if (y == VerticalTiles)
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
    void GenerateDoorways()
    {
        Transform doorWayHolder = new GameObject("DoorWays").transform;
        doorWayHolder.SetParent(roomHolder);

        foreach (var doorway in DoorwayPrefabs)
        {
            Doorway instance = Instantiate(doorway);
            instance.transform.SetParent(doorWayHolder);

            _doorways.Add(instance);
        }
    }
}