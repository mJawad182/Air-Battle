using UnityEngine;

public class FinalEnemyMove : MonoBehaviour
{
    public float targetY;
    public float speed;
    
    [Tooltip("How long the enemy stays on screen before moving back")]
    public float stayDuration = 35f;

    private bool reachedTarget = false;
    private bool movingBack = false;
    private float stayTimer = 0f;
    private float exitY;
    private FinalEnemyHealth healthScript;

    private void Start()
    {
        // Store the starting Y position to move back to
        exitY = transform.position.y;
        healthScript = GetComponent<FinalEnemyHealth>();
    }

    private void Update()
    {
        // Phase 1: Moving down to target
        if (!reachedTarget)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(0f, targetY, 0f),
                speed * Time.deltaTime
            );

            if (transform.position.y <= targetY + 0.01f)
            {
                reachedTarget = true;
                
                // Activate battle when enemy reaches position
                if (healthScript != null)
                {
                    healthScript.ActivateBattle();
                }
            }
            return;
        }

        // Phase 2: Staying on screen for stayDuration
        if (!movingBack)
        {
            stayTimer += Time.deltaTime;
            if (stayTimer >= stayDuration)
            {
                movingBack = true;
            }
            return;
        }

        // Phase 3: Moving back up and off screen
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(0f, exitY, 0f),
            speed * Time.deltaTime
        );

        // Destroy when off screen
        if (transform.position.y >= exitY - 0.01f)
        {
            // Revert everything before destroying
            if (healthScript != null)
            {
                healthScript.RevertEverything();
            }
            GetComponent<Enemy>().Destruction();
        }
    }
}
