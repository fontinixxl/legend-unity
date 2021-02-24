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

    public Direction _direction;
    private bool _open;
    //public bool IsOpen { get { return _open; } }

    private SpriteRenderer spriteRenderer;

    public static event Action<Direction, Transform> PlayerCollideDoorway;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _open = false;
        spriteRenderer.sprite = !_open ? spriteDoorClosed : spriteDoorOpen;
    }

    public void OffsetDoorways(float offsetX, float offsetY)
    {
        float x, y;
        if (_direction == Direction.BOTTOM)
        {
            x = Const.MapWitdth * 0.5f;
            y = 0f;
        }
        else if (_direction == Direction.LEFT)
        {
            x = 0;
            y = Const.MapHeight * 0.5f;
        }
        else if (_direction == Direction.TOP)
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

    private void Update()
    {
        //TODO: Delete it; just for testing the open / close mechanism
        if (Input.GetKeyDown(KeyCode.O))
        {
            _open = !_open;
            spriteRenderer.sprite = !_open ? spriteDoorClosed : spriteDoorOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!DungeonManager.Instance.Shifting && collision.CompareTag("Player") && _open)
        {
            // shift player to center of door to avoid phasing through wall
            PlayerCollideDoorway?.Invoke(_direction, transform);
        }
    }

    public void Open(bool open)
    {
        _open = open;
        spriteRenderer.sprite = !open ? spriteDoorClosed : spriteDoorOpen;
    }
}
