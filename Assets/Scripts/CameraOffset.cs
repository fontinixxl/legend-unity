using UnityEngine;

// Offset the GameObject's position the script is attached to, by half the room size
// so the pivot is right on the center of the Room (gamearea)
public class CameraOffset : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(RoomManager.ScreenWidth / 2, RoomManager.ScreenHight / 2.0f, transform.position.z);
    }
}
