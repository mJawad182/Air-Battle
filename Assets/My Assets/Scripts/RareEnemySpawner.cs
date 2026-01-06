using UnityEngine;

public class RareEnemySpawner : MonoBehaviour
{
    [Header("Rare Enemy Settings")]
    public GameObject rareEnemyPrefab;
    public float spawnYOffScreen = 2f;
    public float stopYPosition = 3f;
    public float moveSpeed = 2f;

    private bool spawnedAt40;
    private bool spawnedAt80;
    private float levelTimer;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        levelTimer = 0f;
        spawnedAt40 = false;
        spawnedAt80 = false;
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;

        if (levelTimer >= 40f && !spawnedAt40)
        {
            SpawnRareEnemy();
            spawnedAt40 = true;
        }

        if (levelTimer >= 80f && !spawnedAt80)
        {
            SpawnRareEnemy();
            spawnedAt80 = true;
        }
    }

    void SpawnRareEnemy()
    {
        Vector3 spawnPos = GetTopOffScreenPosition();
        GameObject enemy = Instantiate(rareEnemyPrefab, spawnPos, Quaternion.identity);

        RareEnemyMove move = enemy.AddComponent<RareEnemyMove>();
        move.targetY = stopYPosition;
        move.speed = moveSpeed;
    }

    Vector3 GetTopOffScreenPosition()
    {
        float camHalfHeight = mainCam.orthographicSize;
        float camHalfWidth = camHalfHeight * mainCam.aspect;

        float randomX = Random.Range(-camHalfWidth + 1f, camHalfWidth - 1f);
        float spawnY = camHalfHeight + spawnYOffScreen;

        return new Vector3(randomX, spawnY, 0f);
    }
}
