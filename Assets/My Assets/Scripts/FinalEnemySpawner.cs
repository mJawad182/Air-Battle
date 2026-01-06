using UnityEngine;

public class FinalEnemySpawner : MonoBehaviour
{
     [Header("Final Enemy Settings")]
    public GameObject rareEnemyPrefab;
    public float spawnYOffScreen = 2f;
    public float stopYPosition = 2f;
    public float moveSpeed = 2f;
    public float spawnTime = 85f;

    
    private bool spawnedAt90;
    private float levelTimer;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        levelTimer = 0f;
        
        spawnedAt90 = false;
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;

        

        if (levelTimer >= spawnTime && !spawnedAt90)
        {
            SpawnRareEnemy();
            spawnedAt90 = true;
        }
    }

    void SpawnRareEnemy()
    {
        Vector3 spawnPos = GetTopOffScreenPosition();
        GameObject enemy = Instantiate(rareEnemyPrefab, spawnPos, Quaternion.identity);

        FinalEnemyMove move = enemy.AddComponent<FinalEnemyMove>();
        move.targetY = stopYPosition;
        move.speed = moveSpeed;
    }

    Vector3 GetTopOffScreenPosition()
    {
        float camHalfHeight = mainCam.orthographicSize;
        float camHalfWidth = camHalfHeight * mainCam.aspect;

        float randomX = Random.Range(-camHalfWidth + 1f, camHalfWidth - 1f);
        float spawnY = camHalfHeight + spawnYOffScreen;

        return new Vector3(0f, spawnY, 0f);
    }
}