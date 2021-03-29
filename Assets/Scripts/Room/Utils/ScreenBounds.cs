using UnityEngine;

/// <summary>
/// Contains static methods and fields for determining screen bounds
/// </summary>
public class ScreenBounds : MonoBehaviour
{
    private static Vector3 bounds;
    private static float spriteBorder = 1.4f;

    public static float Left { get { return -bounds.x + spriteBorder; } }
    public static float Right { get { return bounds.x - spriteBorder; } }
    public static float Top { get { return bounds.y - spriteBorder; } }
    public static float Bottom { get { return -bounds.y + spriteBorder; } }

    public static float Width { get { return bounds.x; } }
    public static float Height { get { return bounds.y; } }

    // Start is called before the first frame update
    private void Start()
    {
        bounds = GetScreenBounds();
    }

    private static Vector3 GetScreenBounds()
    {
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, Mathf.Abs(Camera.main.transform.position.z));

        return Camera.main.ScreenToWorldPoint(screenVector);
    }

    public static Vector2 RandomTopPosition()
    {
        float horizontalPosition = Random.Range(Left, Right);
        return new Vector2(horizontalPosition, Top);
    }

    public static Vector2 GetRandomPosition()
    {
        float targetVerticalPos = Random.Range(-bounds.y + (spriteBorder * 2), bounds.y - spriteBorder);
        float targetHorizontalPos = Random.Range(-bounds.x + spriteBorder, bounds.x - spriteBorder);

        return new Vector2(targetHorizontalPos, targetVerticalPos);
    }
    public static Vector2 GetRandomPositionFullScreen()
    {
        float targetVerticalPos = Random.Range(-bounds.y, bounds.y);
        float targetHorizontalPos = Random.Range(-bounds.x, bounds.x);

        return new Vector2(targetHorizontalPos, targetVerticalPos);
    }

    public static bool OutOfBounds(Vector2 position)
    {
        float x = Mathf.Abs(position.x);
        float y = Mathf.Abs(position.y);

        return (x > bounds.x || y > bounds.y);
    }
}
