using UnityEngine;

public class RareEnemyMove : MonoBehaviour
{
    public float targetY;
    public float speed;

    private bool reachedTarget = false;

    private void Update()
    {
        if (reachedTarget) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(0, targetY, 0f),
            speed * Time.deltaTime
        );

        if (transform.position.y <= targetY + 0.01f)
        {
            reachedTarget = true;
        }
    }
}
