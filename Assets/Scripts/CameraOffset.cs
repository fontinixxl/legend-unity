using UnityEngine;
using UnityEngine.U2D;

// Offset the GameObject's position the script is attached to, by half the room size
// so the pivot is right on the center of the Room (gamearea)
public class CameraOffset : MonoBehaviour
{
    private void Awake()
    {
        // TODO: Once RoomManager is Singleton, we will Get the values from there!
        // In that case though, the code will have to be moved to the Start to be sure
        // the RoomManager has initialized the properties
        PixelPerfectCamera pixPerfCam = Camera.main.GetComponent<PixelPerfectCamera>();
        float roomWidth = (pixPerfCam.refResolutionX / pixPerfCam.assetsPPU);
        float roomHeight = (pixPerfCam.refResolutionY / pixPerfCam.assetsPPU);
        float offsetX = roomWidth * 0.5f;
        float offsetY = roomHeight * 0.5f;

        transform.position = new Vector3(offsetX, offsetY, transform.position.z);
    }
}
