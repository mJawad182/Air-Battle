using UnityEngine;

/// <summary>
/// This script attaches to ‘Background’ object, and moves it up if it goes below the viewport border. 
/// Used for creating an infinite movement effect.
/// </summary>
public class RepeatingBackground : MonoBehaviour
{
    [Tooltip("Vertical size of the sprite in the world space. Attach BoxCollider2D to get the exact size.")]
    public float verticalSize;

    private void Update()
    {
        // Only reposition background while game time is under 110 seconds
        if (Time.time < 120f)
        {
            if (transform.position.y < -verticalSize)
            {
                RepositionBackground();
            }
        }
    }

    void RepositionBackground()
    {
        Vector2 groundOffset = new Vector2(0, verticalSize * 2f);
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
