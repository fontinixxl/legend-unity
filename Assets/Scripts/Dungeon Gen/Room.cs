using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private readonly Transform roomHolder;
    private readonly Transform doorwayHolder;
    public Transform Holder { get => roomHolder; }
    public Transform DoorwayHolder { get => doorwayHolder; }
    private List<Doorway> doorways;
    public List<Doorway> Doorways { get => doorways; set => doorways = value; }
    public GameObject Switch { get => _switch; set => _switch = value; }
    private GameObject _switch;
    private int id;
    public int ID { get => id; set => id = value; }

    public Room(int roomId)
    {
        id = roomId;
        roomHolder = new GameObject("Room" + roomId.ToString()).transform;
        roomHolder.SetParent(DungeonManager.Instance.DungeonHolder);

        // Create composite collider for the walls
        roomHolder.gameObject.AddComponent<Rigidbody2D>();
        roomHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        roomHolder.gameObject.AddComponent<CompositeCollider2D>();

        doorwayHolder = new GameObject("Doorways").transform;
        doorways = new List<Doorway>();

        doorwayHolder.SetParent(roomHolder);
    }

    public void OpenAllDoors(bool open)
    {
        foreach (var doorway in doorways)
        {
            doorway.IsOpen = open;
        }
    }

    public void Reposition(int x, int y)
    {
        roomHolder.Translate(new Vector2(x, y));
    }

    public void Display()
    {
        roomHolder.gameObject.SetActive(true);
        _switch.GetComponent<Switch>().IsActive = false;
    }
}
