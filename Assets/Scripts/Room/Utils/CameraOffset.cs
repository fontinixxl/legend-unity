using UnityEngine;

static class Const
{
    public const int TileSize = 16;
    public const int UnitSize = 1;

    // We divide between TILE_SIZE to work with Units instead of Pixels
    public const int ScreenWitdth = 384 / TileSize;
    public const int ScreenHeight = 216 / TileSize;

    public const int MapWitdth = ScreenWitdth - 2;
    public const int MapHeight = ScreenHeight - 2;

    public const int MapRenderOffsetX = (ScreenWitdth - MapWitdth) / 2;
    public const int MapRenderOffsetY = (ScreenHeight - MapHeight) / 2;
}

// Offset the GameObject's position the script is attached to, by half the room size
// so the pivot is right on the center of the Room (gamearea)
public class CameraOffset : MonoBehaviour
{
    private void Start()
    {
        ResetCameraToCenter();
    }

    public void ResetCameraToCenter()
    {
        transform.position = new Vector3(Const.ScreenWitdth / 2, Const.ScreenHeight / 2.0f, transform.position.z);
    }
}
