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

    public static int Map_render_offset_x { get; private set; }
    public static int Map_render_offset_y { get; private set; }
    public static int RoomWidth { get; private set; }
    public static int RoomHeight { get; private set; }
    public static int ScreenWidth { get; private set; }
    public static int ScreenHight { get; private set; }

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
        int pixelsPerUnit = pixPerfCam.assetsPPU;
        ScreenWidth = (pixPerfCam.refResolutionX / pixelsPerUnit);
        ScreenHight = pixPerfCam.refResolutionY / pixelsPerUnit;

        // Room will be rendered one unit off the edges
        RoomWidth = ScreenWidth - 2;
        RoomHeight = ScreenHight - 2;

        Map_render_offset_x = (ScreenWidth - RoomWidth) / 2;
        Map_render_offset_y = (ScreenHight - RoomHeight) / 2;

        _doorways = new List<Doorway>();
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

        for (int y = 0; y < RoomHeight; y++)
        {
            for (int x = 0; x < RoomWidth; x++)
            {
                GameObject toInstantiate;

                // Corner tiles
                if (x == 0 && y == 0)
                {
                    toInstantiate = bottomLeftCornerTile;
                }
                else if (x == 0 && y == RoomHeight - 1)
                {
                    toInstantiate = topLeftCornerTile;
                }
                else if (x == RoomWidth -1 && y == 0)
                {
                    toInstantiate = bottomRightCornerTile;
                }
                else if (x == RoomWidth - 1 && y == RoomHeight - 1)
                {
                    toInstantiate = topRightCornerTile;
                }
                //random left - hand walls, right walls, top, bottom
                else if (x == 0)
                {
                    toInstantiate = leftWallsTiles[Random.Range(0, leftWallsTiles.Length)];
                }
                else if (x == RoomWidth -1)
                {
                    toInstantiate = rightWallsTiles[Random.Range(0, rightWallsTiles.Length)];
                }
                else if (y == 0)
                {
                    toInstantiate = bottomWallsTiles[Random.Range(0, topWallsTiles.Length)];
                }
                else if (y == RoomHeight -1)
                {
                    toInstantiate = topWallsTiles[Random.Range(0, bottomWallsTiles.Length)];
                }
                // if it's not a corner or a wall tile, be it a floor tile
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                Vector3 offsetPos = new Vector3(x + Map_render_offset_x, y + Map_render_offset_y, 0f);
                GameObject instance = Instantiate(toInstantiate, offsetPos, Quaternion.identity);
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