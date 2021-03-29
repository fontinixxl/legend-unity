using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    // Because PPU is set to 44 (cell height) and the width is 50px, when instantiating cell prefabs, they will be overlaping in the X-coord,
    // as 1 Unit = 44px, and the width is 50px, there will be 6px overlaping. By dividing (50-44)/44 we get the equivalent in units of those 6px.
    // This will be the offset we will add to each cell in the X-Coord.
    //private const float SpriteOffsetX = 6.0f / 44.0f;  // aprox 0.136

    private int[] rooms = new int[100];
    public int[] RoomsGrid { get => rooms; }

    private int roomCount;
    private Queue<int> roomQueue = new Queue<int>();
    private Queue<int> endRooms = new Queue<int>();
    private const int maxRooms = 15;
    private const int minRooms = 7;
    private readonly int firstRoom = 35;

    public Dungeon()
    {
        GenerateDungeon();
    }

    #region Procedural Dungeon
    private void GenerateDungeon()
    {
        roomCount = 0;
        roomQueue = new Queue<int>();
        endRooms = new Queue<int>();

        for (int i = 0; i < 100; i++)
            rooms[i] = 0;

        Visit(firstRoom);
        Loop();
    }

    private void Loop()
    {
        while (roomQueue.Count > 0)
        {
            int i = roomQueue.Dequeue();
            int x = i % 10;
            var created = false;

            if (x > 1) created |= Visit(i - 1);
            if (x < 9) created |= Visit(i + 1);
            if (i > 20) created |= Visit(i - 10);
            if (i < 70) created |= Visit(i + 10);

            if (!created)
                endRooms.Enqueue(i);
        }

        if (roomCount < minRooms)
        {
            Debug.Log("Start Over, not enought rooms (" + roomCount + ")");
            // Start over
            GenerateDungeon();
        }
    }

    private bool Visit(int i)
    {
        // If the cell is occupied, give up
        if (rooms[i] == 1)
            return false;

        // If the neighbour cell itself has more than one filled neighbour, give up.
        int neighbours = Ncount(i);
        if (neighbours > 1)
            return false;

        // If we already have enough rooms, give up
        if (roomCount >= maxRooms)
            return false;

        // Random 50% chance, give up
        if (Random.value < 0.5f && i != firstRoom)
            return false;

        roomQueue.Enqueue(i);
        rooms[i] = 1;
        roomCount++;

        //Debug.Log("Room " + i);
        return true;
    }

    private int Ncount(int i)
    {

        return rooms[i - 10] + rooms[i - 1] + rooms[i + 1] + rooms[i + 10];
    }

    #endregion

    public List<Position> GetNeighbours(int roomid)
    {
        int x = roomid % 10;
        List<Position> neighbours = new List<Position>();

        if (x > 1 && rooms[roomid - 1] == 1)
            neighbours.Add(Position.LEFT);
        if (x < 9 && rooms[roomid + 1] == 1)
            neighbours.Add(Position.RIGHT);
        if (roomid > 20 && rooms[roomid - 10] == 1)
            neighbours.Add(Position.TOP);
        if (roomid < 70 && rooms[roomid + 10] == 1)
            neighbours.Add(Position.BOTTOM);

        return neighbours;

    }

    public int GetNextRoomId(int roomId, Position direction)
    {
        int index;
        switch (direction)
        {
            case Position.TOP:
                index = -10;
                break;
            case Position.RIGHT:
                index = 1;
                break;
            case Position.BOTTOM:
                index = 10;
                break;
            case Position.LEFT:
                index = -1;
                break;
            default:
                return 0;
        }

        return roomId + index;

    }

    //private void DrawSprite(int i, GameObject prebaf)
    //{
    //    // Ex: i = 35; x=5 y=3
    //    float x = i % 10;
    //    float y = (i - x) / 10;

    //    Vector2 coord = new Vector2((x - 1) + (x - 1) * SpriteOffsetX, y);
    //    //DrawSprite(new Vector2(x + (x * SpriteOffsetX), y), cell);
    //    Instantiate(prebaf, coord, Quaternion.identity, cellParent);
    //}
}
