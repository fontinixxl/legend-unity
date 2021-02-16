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

    // Arrays of tile prefabs
    public GameObject[] floorTiles;
    public GameObject[] topWallsTiles;
    public GameObject[] bottomWallsTiles;
    public GameObject[] leftWallsTiles;
    public GameObject[] rightWallsTiles;
    public GameObject topLeftCornerTile;
    public GameObject topRightCornerTile;
    public GameObject bottomLeftCornerTile;
    public GameObject bottomRightCornerTile;

    public List<Doorway> DoorwayPrefabs;

    // A list of possible locations to place tiles.
    //private List<Vector3> gridPositions = new List<Vector3>();

    private float _adjacentOffsetX;
    private float _adjacentOffsetY;

    private Room room;

    public Room GenerateRoom(float offsetX, float offsetY)
    {
        _adjacentOffsetX = offsetX;
        _adjacentOffsetY = offsetY;

        room = new Room();
        GenerateWallsAndFloors();
        GenerateDoorways();

        return room;
    }

	// Generate the walls and floor of the room, randomizing the various varieties
    void GenerateWallsAndFloors()
    {
        for (int y = 0; y < Const.MapHeight; y++)
        {
            for (int x = 0; x < Const.MapWitdth; x++)
            {
                GameObject toInstantiate;

                // Corner tiles
                if (x == 0 && y == 0)
                {
                    toInstantiate = bottomLeftCornerTile;
                }
                else if (x == 0 && y == Const.MapHeight - 1)
                {
                    toInstantiate = topLeftCornerTile;
                }
                else if (x == Const.MapWitdth - 1 && y == 0)
                {
                    toInstantiate = bottomRightCornerTile;
                }
                else if (x == Const.MapWitdth - 1 && y == Const.MapHeight - 1)
                {
                    toInstantiate = topRightCornerTile;
                }
                //random left - hand walls, right walls, top, bottom
                else if (x == 0)
                {

                    toInstantiate = leftWallsTiles[Random.Range(0, leftWallsTiles.Length)];
                }
                else if (x == Const.MapWitdth - 1)
                {

                    toInstantiate = rightWallsTiles[Random.Range(0, rightWallsTiles.Length)];
                }
                else if (y == 0)
                {
                    toInstantiate = bottomWallsTiles[Random.Range(0, topWallsTiles.Length)];
                }
                else if (y == Const.MapHeight - 1)
                {
                    toInstantiate = topWallsTiles[Random.Range(0, bottomWallsTiles.Length)];
                }
                // if it's not a corner or a wall tile, be it a floor tile
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                Vector3 position = new Vector3(x + Const.MapRenderOffsetX + _adjacentOffsetX, 
                    y + Const.MapRenderOffsetY + _adjacentOffsetY, 0f);

                GameObject instance = Instantiate(toInstantiate, position, Quaternion.identity);

                instance.transform.SetParent(room.RoomHolder);
            }
        }
    }
    void GenerateDoorways()
    {
        foreach (var doorway in DoorwayPrefabs)
        {
            Doorway instance = Instantiate(doorway);
            instance.OffsetDoorways(_adjacentOffsetX, _adjacentOffsetY);
            instance.transform.SetParent(room.DoorwayHolder);

            room.Doorways.Add(instance);
        }
    }
}

public class Room
{
    private readonly Transform roomHolder;
    private readonly Transform doorwayHolder;
    public Transform RoomHolder { get => roomHolder; }
    public Transform DoorwayHolder { get => doorwayHolder; }
    private List<Doorway> doorways;
    public List<Doorway> Doorways { get => doorways; set => doorways = value; }

    public Room()
    {
        roomHolder = new GameObject("Room").transform;
        doorwayHolder = new GameObject("Doorways").transform;
        doorways = new List<Doorway>();
        
        doorwayHolder.SetParent(roomHolder);
    }

    public void OpenAllDoors(bool open)
    {
        foreach(var doorway in doorways)
        {
            doorway.Open(open);
        }
    }
}
