using UnityEngine;

public enum SpawnCount
{
    One = 1,
    Two = 2
}

public enum MoveScriptType
{
    LowFinalEnemyMove,
    LowEnemySpawnerMove2
}

public class LowFinalEnemySpawner : MonoBehaviour
{
    [Header("Final Enemy Settings")]
    public GameObject EnemyPrefab;
    public float spawnYOffScreen = 2f;
    public float stopYPosition = 2f;
    public float moveSpeed = 2f;
    public float spawnTime = 80f;
    public float stayDuration = 35f;
    
    [Header("Spawn Settings")]
    [Tooltip("Choose to spawn 1 or 2 enemies")]
    public SpawnCount enemiesToSpawn = SpawnCount.Two;
    
    [Tooltip("Which move script to use")]
    public MoveScriptType moveScript = MoveScriptType.LowFinalEnemyMove;
    
    [Tooltip("Distance from center for left/right enemies (only used when spawning 2)")]
    public float horizontalOffset = 2f;

    
    private bool hasSpawned;
    private float levelTimer;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        levelTimer = 0f;
        
        hasSpawned = false;
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;

        if (levelTimer >= spawnTime && !hasSpawned)
        {
            SpawnEnemies();
            hasSpawned = true;
        }
    }

    void SpawnEnemies()
    {
        float spawnY = mainCam.orthographicSize + spawnYOffScreen;
        
        if (enemiesToSpawn == SpawnCount.One)
        {
            // Spawn single enemy at center
            Vector3 centerSpawnPos = new Vector3(0f, spawnY, 0f);
            SpawnEnemy(centerSpawnPos);
        }
        else
        {
            // Spawn left enemy
            Vector3 leftSpawnPos = new Vector3(-horizontalOffset, spawnY, 0f);
            SpawnEnemy(leftSpawnPos);
            
            // Spawn right enemy
            Vector3 rightSpawnPos = new Vector3(horizontalOffset, spawnY, 0f);
            SpawnEnemy(rightSpawnPos);
        }
    }
    
    void SpawnEnemy(Vector3 spawnPos)
    {
        GameObject enemy = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);

        // Calculate target Y position - lower on screen (multiply offset to place lower)
        float topY = mainCam.orthographicSize - (stopYPosition * 2.5f);

        if (moveScript == MoveScriptType.LowFinalEnemyMove)
        {
            LowFinalEnemyMove move = enemy.AddComponent<LowFinalEnemyMove>();
            move.targetY = topY;
            move.speed = moveSpeed;
            move.stayDuration = stayDuration;
        }
        else
        {
            LowEnemySpawnerMove2 move = enemy.AddComponent<LowEnemySpawnerMove2>();
            move.targetY = topY;
            move.speed = moveSpeed;
            move.stayDuration = stayDuration;
        }
    }
}
