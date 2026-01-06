using UnityEngine;

public class FinalEnemyMove : MonoBehaviour
{
    public float targetY;
    public float speed;

    private bool reachedTarget = false;

    private void Update()
    {
        if (reachedTarget) return;

        // Keep same X position, only move down on Y axis
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(0f, targetY, 0f),
            speed * Time.deltaTime
        );

        if (transform.position.y <= targetY + 0.01f)
        {
            reachedTarget = true;
        }
    }
}
