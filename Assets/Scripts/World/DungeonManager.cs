using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class DungeonManager : Singleton<DungeonManager>
{
    public PlayerController playerPrefab;

    private Transform _mainCamera;

    public RoomManager RoomManager;

    private Room _currentRoom;
    private Vector3 _adjacentOffset;
    private Room _nextRoom;

    private Transform _player;
    private PlayerController _playerController;

    public bool Shifting;
    private readonly float _cameraSpeed = 15.0f;

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main.transform;

        Vector2 position = new Vector2(Const.ScreenWitdth / 2.0f, Const.ScreenHeight / 2.0f);
        _playerController = Instantiate(playerPrefab, position, Quaternion.identity);
        _player = _playerController.transform;

        Shifting = false;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Doorway.PlayerCollideDoorway += OnDoorWayTriggered;

        _adjacentOffset = new Vector2(0f, 0f);
        _currentRoom = RoomManager.GenerateRoom(_adjacentOffset.x, _adjacentOffset.y);
    }

    private void OnDoorWayTriggered(Direction direction, Transform doorway)
    {
        StartCoroutine(PrepareForShifting(direction, doorway));
    }

    private IEnumerator PrepareForShifting(Direction direction, Transform doorway)
    {
        int offsetX, offsetY;
        offsetX = offsetY = 0;
        Vector2 doorCenter = Vector2.zero;
        switch (direction)
        {
            case Direction.TOP:
                doorCenter = new Vector2( doorway.position.x + Const.UnitSize, _player.position.y);
                offsetY = Const.ScreenHeight;
                break;
            case Direction.BOTTOM:
                doorCenter = new Vector2( doorway.position.x + Const.UnitSize, _player.position.y);
                offsetY = -Const.ScreenHeight;
                break;
            case Direction.RIGHT:
                doorCenter = new Vector2(_player.position.x, doorway.position.y + Const.UnitSize);
                offsetX = Const.ScreenWitdth;
                break;
            case Direction.LEFT:
                doorCenter = new Vector2(_player.position.x, doorway.position.y + Const.UnitSize);
                offsetX = -Const.ScreenWitdth;
                break;
        }

        _adjacentOffset.x += offsetX;
        _adjacentOffset.y += offsetY;

        // Shift player to center of door to avoid phasing through wall
        yield return StartCoroutine(CenterPlayerWithDoor(doorCenter));

        // Generate Adjacent room and keep the doors opened until player has crossed.
        _nextRoom = RoomManager.GenerateRoom(_adjacentOffset.x, _adjacentOffset.y);
        _nextRoom.OpenAllDoors(true);
        Shifting = true;
        
        yield return StartCoroutine(Shift(offsetX, offsetY));

        _nextRoom.OpenAllDoors(false);
        // Point to transitioned room as the new active room, pointing to an empty room next
        Destroy(_currentRoom.RoomHolder.gameObject);
        _currentRoom = _nextRoom;
        _nextRoom = null;
        Shifting = false;
    }

    IEnumerator CenterPlayerWithDoor(Vector2 target)
    {
        while (Vector3.Distance(_player.position, target) > 0.0001f)
        {
            float step = _playerController.speed * Time.deltaTime;
            _player.transform.position = Vector3.MoveTowards(_player.position, target, step);
            yield return new WaitForEndOfFrame();
        }
    }

    // Shift the camera and the player smoothly towards the adjacent Room
    IEnumerator Shift(int offsetX, int offsetY)
    {
        Vector3 playerTarget = _player.position;

        if (offsetX < 0)
        {
            // LEFT
            playerTarget.x = _adjacentOffset.x + Const.MapRenderOffsetX + Const.MapWitdth - Const.UnitSize - 0.5f;
        }
        else if (offsetX > 0)
        {
            // RIGHT
            playerTarget.x = _adjacentOffset.x + Const.MapRenderOffsetX + Const.UnitSize + 0.5f;
        }
        else if (offsetY < 0)
        {
            // BOTTOM
            playerTarget.y = _adjacentOffset.y + Const.MapRenderOffsetY + Const.MapHeight - Const.UnitSize;
        }
        else
        {
            // TOP
            playerTarget.y = _adjacentOffset.y + Const.MapRenderOffsetY + Const.UnitSize + 0.5f;
        }

        // Get the Units the camera will have to move on the offset
        // If the offsetY is zero, means we are moving Horizontaly
        float cameraDistance = (offsetY == 0) ? Const.ScreenWitdth : Const.ScreenHeight;
        float cameraTimeToTarget = cameraDistance / _cameraSpeed;

        float playerDistance = Vector2.Distance(_player.position, playerTarget);
        // playerSpeed is calculated based on the time it will take the camera
        // to reach the target position, so both the camera and player will finish at the same time.
        float playerSpeed = playerDistance / cameraTimeToTarget;

        // Calculate the new camera position
        Vector3 target = _mainCamera.position;
        target.x += offsetX;
        target.y += offsetY;

        // Enable Player kinematic so the player can be move throught the walls and doors
        _playerController.EnableKinematic(true);

        while (Vector3.Distance(_mainCamera.position, target) > 0.0001f)
        {
            float step = _cameraSpeed * Time.deltaTime;
            _mainCamera.position = Vector3.MoveTowards(_mainCamera.position, target, step);
            _player.position = Vector3.MoveTowards(_player.position, playerTarget,  playerSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _playerController.EnableKinematic(false);
    }
}
