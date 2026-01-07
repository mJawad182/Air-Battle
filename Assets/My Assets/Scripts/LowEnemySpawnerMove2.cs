using UnityEngine;

public class LowEnemySpawnerMove2 : MonoBehaviour
{
   public float targetY;
    public float speed;
    
    [Tooltip("How long the enemy stays on screen before moving back")]
    public float stayDuration = 10f;

    private bool reachedTarget = false;
    private bool movingBack = false;
    private float stayTimer = 0f;
    private float startX;  // Store original X position
    private float exitY;
    private FinalEnemyHealth healthScript;

    private void Start()
    {
        // Store the starting position
        startX = transform.position.x;  // Keep original X position
        exitY = transform.position.y;
        healthScript = GetComponent<FinalEnemyHealth>();
    }

    private void Update()
    {
        // Phase 1: Moving down to target (keep same X position)
        if (!reachedTarget)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(startX, targetY, 0f),  // Use startX instead of 0f
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

        // Phase 3: Moving back up and off screen (keep same X position)
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(startX, exitY, 0f),  // Use startX instead of 0f
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
