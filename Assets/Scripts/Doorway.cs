using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    public Sprite spriteDoorOpen;
    public Sprite spriteDoorClosed;

    public enum Direction
    {
        Top, Right, Bottom, Left
    }
    public Direction direction;
    private bool doorsOpen;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorsOpen = false;
    }

    private void Start()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        float x, y;
        if (direction == Direction.Bottom)
        {
            x = RoomManager.RoomWidth * 0.5f;
            y = 0f;
        }
        else if (direction == Direction.Left)
        {
            x = 0;
            y = RoomManager.RoomHeight * 0.5f;
        }
        else if (direction == Direction.Top)
        {
            x = RoomManager.RoomWidth * 0.5f;
            y = RoomManager.RoomHeight;
        }
        else
        {
            // Right
            x = RoomManager.RoomWidth;
            y = RoomManager.RoomHeight * 0.5f;
        }

        transform.position = new Vector3(x, y, 0f);
    }

    private void Update()
    {
        // TODO: The logic must be moved to an event Listener
        if (Input.GetKeyDown(KeyCode.O))
        {
            doorsOpen = !doorsOpen;
            spriteRenderer.sprite = !doorsOpen ? spriteDoorClosed : spriteDoorOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && doorsOpen)
        {
            collision.transform.position = new Vector3( RoomManager.RoomWidth/2 +1, RoomManager.RoomHeight/2 +1, 0f);
        }
    }
}
