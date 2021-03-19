using System;
using UnityEngine;

public enum Direction
{
    TOP, RIGHT, BOTTOM, LEFT
}

public class Doorway : MonoBehaviour
{
    public Sprite spriteDoorOpen;
    public Sprite spriteDoorClosed;

    public Direction Direction;

    public bool _open;
    public bool IsOpen
    {
        get => _open;
        set
        {
            spriteRenderer.sprite = value ? spriteDoorOpen : spriteDoorClosed;
            _open = value;
        }
    }

    private SpriteRenderer spriteRenderer;

    public static event Action<Doorway> ShiftRoomEvent;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsOpen = false;
    }

    public void OffsetDoorway(float offsetX, float offsetY)
    {
        float x, y;
        if (Direction == Direction.BOTTOM)
        {
            x = Const.MapWitdth * 0.5f;
            y = 0f;
        }
        else if (Direction == Direction.LEFT)
        {
            x = 0;
            y = Const.MapHeight * 0.5f;
        }
        else if (Direction == Direction.TOP)
        {
            x = Const.MapWitdth * 0.5f;
            y = Const.MapHeight;
        }
        else
        {
            // Right
            x = Const.MapWitdth;
            y = Const.MapHeight * 0.5f;
        }

        transform.position = new Vector3(x + offsetX, y + offsetY, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DungeonManager.Instance.Shifting)
            return;

        if (collision.CompareTag("Player") && IsOpen)
        {
            // Shift player to the center of door to avoid phasing through wall
            ShiftRoomEvent?.Invoke(this);
        }
    }
}
