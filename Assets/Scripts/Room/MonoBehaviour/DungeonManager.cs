using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    public PlayerStateManager playerPrefab;
    public RoomManager RoomManager;
    //public MiniMap _miniMap;
    public GameObject miniMap;
    public Dungeon Dungeon { get => dungeon; }
    public Transform DungeonHolder { get => _dungeonHolder; }
    public bool Shifting { get => _shifting; }
    public GameObject mapRoom;
    private GameObject[] _minimapRooms;

    private const int rows = 8;

    // 35 => row 3 col 5
    public int startRoom = 35;

    private Transform _mainCamera;
    private Room _currentRoom;
    private Room _nextRoom;

    private Transform _playerTransform;
    private PlayerStateManager _playerController;

    private bool _shifting;

    private int _offsetY;
    private int _offsetX;
    private readonly float _cameraSpeed = 15.0f;

    private Dungeon dungeon;
    private Dictionary<int, Room> _visitedRooms;
    private Transform _dungeonHolder;

    #region Monobehaviour
    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main.transform;
        Vector2 position = new Vector2(Const.ScreenWitdth / 2.0f, Const.ScreenHeight / 2.0f);
        _playerController = Instantiate(playerPrefab, position, Quaternion.identity);
        _playerTransform = _playerController.transform;
        _offsetX = _offsetY = 0;

        _shifting = false;

        _dungeonHolder = new GameObject("Dungeon").transform;

        _visitedRooms = new Dictionary<int, Room>();
        _minimapRooms = new GameObject[100];
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        Doorway.ShiftRoomEvent += PlayerCollideDoorwayEventHandler;
        Switch.SwitchPressed += SwitchPressedEventHandler;

        dungeon = new Dungeon();
        
        //disable for now the minimap
        if (miniMap.activeSelf)
        {
            DisplayMiniMap();
            HighlightCurrentRoom(startRoom, startRoom);
        }

        List<Position> neighbourRooms = dungeon.GetNeighbours(startRoom);
        _currentRoom = RoomManager.GenerateRoom(_offsetX, _offsetY, neighbourRooms, startRoom);

        _visitedRooms.Add(startRoom, _currentRoom);
    }

    #endregion

    #region Event Listeners
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

    #endregion

    #region Shifting operations
    private IEnumerator ShiftRoom(Doorway doorway)
    {
        _shifting = true;
        _offsetX = _offsetY = 0;
        float doorwayPosX = doorway.transform.position.x;
        float doorwayPosY = doorway.transform.position.y;
        Vector2 doorCenter = Vector2.zero;

        switch (doorway.position)
        {
            case Position.TOP:
                doorCenter = new Vector2(doorwayPosX + Const.UnitSize, _playerTransform.position.y);
                _offsetY = Const.ScreenHeight;
                break;
            case Position.BOTTOM:
                doorCenter = new Vector2(doorwayPosX + Const.UnitSize, _playerTransform.position.y);
                _offsetY = -Const.ScreenHeight;
                break;
            case Position.RIGHT:
                doorCenter = new Vector2(_playerTransform.position.x, doorwayPosY + Const.UnitSize);
                _offsetX = Const.ScreenWitdth;
                break;
            case Position.LEFT:
                doorCenter = new Vector2(_playerTransform.position.x, doorwayPosY + Const.UnitSize);
                _offsetX = -Const.ScreenWitdth;
                break;
        }

        yield return StartCoroutine(CenterPlayerWithDoor(doorCenter));
        _nextRoom = GetNextRoom(doorway.position);
        yield return StartCoroutine(PerformShifting());

        // Close all doors only if we haven't visited the new room yet
        if (!_visitedRooms.ContainsKey(_nextRoom.ID))
        {
            _nextRoom.OpenAllDoors(false);
            _visitedRooms.Add(_nextRoom.ID, _nextRoom);
        }

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
            playerTarget.y = _offsetY + Const.MapRenderOffsetY + Const.UnitSize + 0.55f;
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
    }

    // Resets a few variables needed to perform a camera shift and swaps the next and
    // current room.
    private void FinishShifting()
    {
        int previousRoomID = _currentRoom.ID;
        _currentRoom.Holder.gameObject.SetActive(false);
        _currentRoom = _nextRoom;
        _nextRoom = null;

        if (miniMap.activeSelf)
            HighlightCurrentRoom(_currentRoom.ID, previousRoomID);

        // Once shifted, reposition current room to 0,0
        _currentRoom.Holder.Translate(new Vector3(-_offsetX, -_offsetY));
        // Reset player to the correct location in the room
        _playerTransform.Translate(new Vector2(-_offsetX, -_offsetY));
        _mainCamera.GetComponent<CameraOffset>().ResetCameraToCenter();

        _shifting = false;
    }

    private Room GetNextRoom(Position direction)
    {
        Room nextRoom;
        int nextRoomId = dungeon.GetNextRoomId(_currentRoom.ID, direction);
        Debug.LogFormat("Next Room Id = \"{0}\" ", nextRoomId);
        if (_visitedRooms.ContainsKey(nextRoomId))
        {
            Debug.LogFormat("Next Room \"{0}\" has been visited already => reposition!", nextRoomId);
            nextRoom = _visitedRooms[nextRoomId];
            nextRoom.Reposition(_offsetX, _offsetY);
            nextRoom.Display();
        }
        else
        {
            Debug.LogFormat("Next Room \"{0}\" hasn't been visited yet", nextRoomId);
            List<Position> neighbours = dungeon.GetNeighbours(nextRoomId);
            //DebugNeighbours(neighbours);
            nextRoom = RoomManager.GenerateRoom(_offsetX, _offsetY, neighbours, nextRoomId);
        }

        return nextRoom;

    }

    #endregion

    #region Mini Map
    private void DisplayMiniMap()
    {
        float SpriteOffsetX = (50f - 88f) / 88f;
        float SpriteOffsetY = (44f - 88f) / 88f;

        int[] roomsGrid = dungeon.RoomsGrid;

        for (int i = roomsGrid.Length -1; i >= 0; i--)
        {
            if (roomsGrid[i] == 0) continue;

            float x = i % 10;
            float y = (i - x) / 10;

            // We need to flip Y as unity coord start at bottomleft and the grid is top-left
            float yCoord = (rows - 1) - y;
            Vector2 coord = new Vector2(Const.MapRenderOffsetX + x + (x * SpriteOffsetX), Const.MapRenderOffsetY + yCoord + (yCoord * SpriteOffsetY));

            _minimapRooms[i] = Instantiate(mapRoom, coord, Quaternion.identity, miniMap.transform);
            _minimapRooms[i].name = "MapRoom_" + i;
        }
    }

    public void HighlightCurrentRoom(int currRoomId, int previousRoomId)
    {
        // Unhighlight previous room
        _minimapRooms[previousRoomId].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        // Highlight current room
        _minimapRooms[currRoomId].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
    }

    #endregion

    #region Debug methods
    static void DebugNeighbours(List<Position> neighbours)
    {
        Debug.Log("Next Room Neighbours are: ");
        foreach (var item in neighbours)
        {
            switch (item)
            {
                case Position.TOP:
                    Debug.Log("TOP");
                    break;
                case Position.RIGHT:
                    Debug.Log("RIGHT");
                    break;
                case Position.BOTTOM:
                    Debug.Log("BOTTOM");
                    break;
                case Position.LEFT:
                    Debug.Log("LEFT");
                    break;
                default:
                    break;
            }
        }
    }

    #endregion
}
