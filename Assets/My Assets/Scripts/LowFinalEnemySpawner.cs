using UnityEngine;

public class LowFinalEnemySpawner : MonoBehaviour
{
    [Header("Final Enemy Settings")]
    public GameObject EnemyPrefab;
    public float spawnYOffScreen = 2f;
    public float stopYPosition = 2f;
    public float moveSpeed = 2f;
    public float spawnTime = 80f;
    
    [Header("Parallel Spawn Settings")]
    [Tooltip("Distance from center for left/right enemies")]
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
            SpawnParallelEnemies();
            hasSpawned = true;
        }
    }

    void SpawnParallelEnemies()
    {
        float spawnY = mainCam.orthographicSize + spawnYOffScreen;
        
        // Spawn left enemy
        Vector3 leftSpawnPos = new Vector3(-horizontalOffset, spawnY, 0f);
        SpawnEnemy(leftSpawnPos);
        
        // Spawn right enemy
        Vector3 rightSpawnPos = new Vector3(horizontalOffset, spawnY, 0f);
        SpawnEnemy(rightSpawnPos);
    }
    
    void SpawnEnemy(Vector3 spawnPos)
    {
        GameObject enemy = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);

        LowFinalEnemyMove move = enemy.AddComponent<LowFinalEnemyMove>();
        move.targetY = stopYPosition;
        move.speed = moveSpeed;
    }
}
