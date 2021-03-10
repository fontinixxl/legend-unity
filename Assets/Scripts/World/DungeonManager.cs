using System.Collections;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    public PlayerStateManager playerPrefab;

    private Transform _mainCamera;

    public RoomManager RoomManager;

    private Room _currentRoom;
    private Room _nextRoom;

    private Transform _playerTransform;
    private PlayerStateManager _playerController;

    private bool _shifting;
    public bool Shifting { get => _shifting; }

    private int _offsetY;
    private int _offsetX;
    private readonly float _cameraSpeed = 15.0f;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main.transform;
        Vector2 position = new Vector2(Const.ScreenWitdth / 2.0f, Const.ScreenHeight / 2.0f);
        _playerController = Instantiate(playerPrefab, position, Quaternion.identity);
        _playerTransform = _playerController.transform;
        _offsetX = _offsetY = 0;
        _shifting = false;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        Doorway.PlayerCollideDoorway += PlayerCollideDoorwayEventHandler;
        Switch.SwitchPressed += SwitchPressedEventHandler;

        _currentRoom = RoomManager.GenerateRoom(_offsetX, _offsetY);
    }

    // Triggered when player press the switch.
    // Open all dors on the current room
    private void SwitchPressedEventHandler()
    {
        _currentRoom.OpenAllDoors(true);
    }

    // Trigger camera translation and adjustment of rooms whenever the player triggers a shift
    // via a doorway collision
    private void PlayerCollideDoorwayEventHandler(Doorway doorway)
    {
        StartCoroutine(ShiftRoom(doorway));
    }

    private IEnumerator ShiftRoom(Doorway doorway)
    {
        //Debug.Log("Callback recieved: Starting shifting process");
        _shifting = true;
        _offsetX = _offsetY = 0;
        float doorwayPosX = doorway.transform.position.x;
        float doorwayPosY = doorway.transform.position.y;
        Vector2 doorCenter = Vector2.zero;

        switch (doorway.Direction)
        {
            case Direction.TOP:
                doorCenter = new Vector2(doorwayPosX + Const.UnitSize, _playerTransform.position.y);
                _offsetY = Const.ScreenHeight;
                break;
            case Direction.BOTTOM:
                doorCenter = new Vector2(doorwayPosX + Const.UnitSize, _playerTransform.position.y);
                _offsetY = -Const.ScreenHeight;
                break;
            case Direction.RIGHT:
                doorCenter = new Vector2(_playerTransform.position.x, doorwayPosY + Const.UnitSize);
                _offsetX = Const.ScreenWitdth;
                break;
            case Direction.LEFT:
                doorCenter = new Vector2(_playerTransform.position.x, doorwayPosY + Const.UnitSize);
                _offsetX = -Const.ScreenWitdth;
                break;
        }

        yield return StartCoroutine(CenterPlayerWithDoor(doorCenter));

        _nextRoom = RoomManager.GenerateRoom(_offsetX, _offsetY);

        yield return StartCoroutine(PerformShifting());

        FinishShifting();
    }

    // Tween the player position so they move through the doorway
    IEnumerator CenterPlayerWithDoor(Vector2 target)
    {
        //Debug.Log("Centering Player With Door");
        while (Vector3.Distance(_playerTransform.position, target) > 0.0001f)
        {
            float step = _playerController.WalkSpeed * Time.deltaTime;
            _playerTransform.transform.position = Vector3.MoveTowards(_playerTransform.position, target, step);
            yield return null;
        }

        // We want the player to be looking at the doorway it is about to cross,
        // in case is not comming straight to the door
        _playerController.Animator.SetFloat("MoveX", _offsetX);
        _playerController.Animator.SetFloat("MoveY", _offsetY);
    }

    // Tween the camera in whichever direction the new room is in, as well as the player to be
    // at the opposite door in the next room, walking through the wall(which is stenciled)
    IEnumerator PerformShifting()
    {
        //Debug.Log("Performing Shifting");
        Vector3 playerTarget = _playerTransform.position;
        if (_offsetX < 0)
        {
            // LEFT
            playerTarget.x = _offsetX + Const.MapRenderOffsetX + Const.MapWitdth - Const.UnitSize - 0.5f;
        }
        else if (_offsetX > 0)
        {
            // RIGHT
            playerTarget.x = _offsetX + Const.MapRenderOffsetX + Const.UnitSize + 0.5f;
        }
        else if (_offsetY < 0)
        {
            // BOTTOM
            playerTarget.y = _offsetY + Const.MapRenderOffsetY + Const.MapHeight - Const.UnitSize - 0.5f;
        }
        else
        {
            // TOP
            playerTarget.y = _offsetY + Const.MapRenderOffsetY + Const.UnitSize + 0.5f;
        }


        // Calculate the time (in seconds) it will take the camera to reach the target position
        float cameraDistance = (_offsetY == 0) ? Const.ScreenWitdth : Const.ScreenHeight;
        float cameraTimeToTarget = cameraDistance / _cameraSpeed;

        // playerVelocity is calculated based on the time it will take for the camera
        // to reach the target position. So both the camera and player will finish shifting at the same time.
        float playerVelocity = Vector2.Distance(_playerTransform.position, playerTarget) / cameraTimeToTarget;

        // Calculate the target camera position
        Vector3 target = _mainCamera.position;
        target.x += _offsetX;
        target.y += _offsetY;

        _nextRoom.OpenAllDoors(true);

        while (Vector3.Distance(_mainCamera.position, target) > 0.0001f)
        {
            float step = _cameraSpeed * Time.deltaTime;
            _mainCamera.position = Vector3.MoveTowards(_mainCamera.position, target, step);
            _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, playerTarget,  playerVelocity * Time.deltaTime);
            yield return null;
        }

        _nextRoom.OpenAllDoors(false);
    }

    // Resets a few variables needed to perform a camera shift and swaps the next and
    // current room.
    private void FinishShifting()
    {
        Destroy(_currentRoom.Holder.gameObject);
        _currentRoom = _nextRoom;
        _nextRoom = null;

        _currentRoom.Holder.Translate(new Vector3(-_offsetX, -_offsetY));
        // Reset player to the correct location in the room
        _playerTransform.Translate(new Vector2(-_offsetX, -_offsetY));
        _mainCamera.GetComponent<CameraOffset>().ResetCameraToCenter();

        _shifting = false;
    }
}
