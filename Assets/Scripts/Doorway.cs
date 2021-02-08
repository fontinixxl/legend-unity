using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Doorway : MonoBehaviour
{
    public Sprite spriteDoorOpen;
    public Sprite spriteDoorClosed;

    public enum Direction
    {
        Top, Right, Bottom, Left
    }
    public Direction direction;
    private bool isOpen;

    private SpriteRenderer spriteRenderer ;
    private int _horizontalTiles;
    private int _verticalTiles;

    private void Awake()
    {
        // TODO: Delete! Get it from the RoomManager
        PixelPerfectCamera pixPerfCam = Camera.main.GetComponent<PixelPerfectCamera>();
        _horizontalTiles = (pixPerfCam.refResolutionX / pixPerfCam.assetsPPU) - 2;
        _verticalTiles = (pixPerfCam.refResolutionY / pixPerfCam.assetsPPU) - 2;
        // End delete

        spriteRenderer = GetComponent<SpriteRenderer>();
        isOpen = false;
        SetPosition();
    }

    private void SetPosition()
    {
        float x, y;
        if (direction == Direction.Bottom)
        {
            x = _horizontalTiles * 0.5f;
            y = 0f;
        }
        else if (direction == Direction.Left)
        {
            x = 0;
            y = _verticalTiles * 0.5f;
        }
        else if (direction == Direction.Top)
        {
            x = _horizontalTiles * 0.5f;
            y = _verticalTiles;
        }
        else
        {
            // Right
            x = _horizontalTiles;
            y = _verticalTiles * 0.5f;
        }

        transform.position = new Vector3(x, y, 0f);
    }

    private void Update()
    {
        // TODO: The logic must be moved to an event Listener
        if (Input.GetKeyDown(KeyCode.O))
        {
            isOpen = !isOpen;
            spriteRenderer.sprite = !isOpen ? spriteDoorClosed : spriteDoorOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOpen)
        {
            collision.transform.position = new Vector3( _horizontalTiles/2 + 1, _verticalTiles/2 + 1, 0f);
        }
    }
}
