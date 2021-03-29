using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Delte this script (migrated to DungeonManager.cs)
public class MiniMap : MonoBehaviour
{
    public GameObject roomPrefab;
    private GameObject[] rooms;
    private int startX;
    private int startY;
    // Grid of 9x8 rooms
    private const int columns = 9;
    private const int rows = 8;
    // Because PPU is set to 44 (cell height) and the width is 50px, when instantiating cell prefabs, they will be overlaping in the X-coord,
    // as 1 Unit = 44px, and the width is 50px, there will be 6px overlaping. By dividing (50-44)/44 we get the equivalent in units of those 6px.
    // This will be the offset we will add to each cell in the X-Coord.
    private const float SpriteOffsetX = (50f - 88f) / 88f;
    private const float SpriteOffsetY = (44f - 88f) / 88f;
    

    private void Awake()
    {
        rooms = new GameObject[100];
    }

    void Start()
    {
        Debug.Log("MiniMap Start() executed");
        startX = Const.MapRenderOffsetX;
        startY = Const.MapRenderOffsetY;
    }

    public void Render(int[] roomsGrid)
    {
        Debug.Log(startX + " " + startY);
        for (int i = roomsGrid.Length - 1; i >= 0; i--)
        {
            if (roomsGrid[i] == 0) continue;

            float x = i % 10;
            float y = (i - x) / 10;

            // We need to flip Y as unity coord start at bottomleft and the grid is top-left
            float yCoord = (rows - 1) - y;
            Vector2 coord = new Vector2(startX + x + (x * SpriteOffsetX), startY + yCoord + (yCoord * SpriteOffsetY));
            rooms[i] = Instantiate(roomPrefab, coord, Quaternion.identity, transform);
            rooms[i].name = "Room" + i;
        }

    }

    public void HighlightCurrentRoom(int currRoomId, int previousRoomId)
    {
        // Unhighlight previous room
        rooms[previousRoomId].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        // Highlight current room
        rooms[currRoomId].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
    }
}
