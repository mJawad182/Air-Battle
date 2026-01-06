using UnityEngine;

public class FinalEnemyShooting : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;

    [Header("Shooting Settings")]
    [Tooltip("Initial shots per second")]
    public float fireRate = 1f;

    [Tooltip("How much fire rate increases per second")]
    public float fireRateIncreasePerSecond = 0.1f;

    [Tooltip("Maximum fire rate (shots per second)")]
    public float maxFireRate = 5f;

    [Tooltip("Delay before shooting starts")]
    public float startDelay = 0.5f;

    private float nextFireTime;
    private bool canShoot = false;
    private float currentFireRate;
    private float shootingStartTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Auto-find player by tag if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        currentFireRate = fireRate;
        Invoke(nameof(EnableShooting), startDelay);
    }

    private void Update()
    {
        if (!canShoot || player == null)
            return;

        // Increase fire rate over time
        float elapsedTime = Time.time - shootingStartTime;
        currentFireRate = Mathf.Min(fireRate + (fireRateIncreasePerSecond * elapsedTime), maxFireRate);

        if (Time.time >= nextFireTime)
        {
            ShootAtPlayer();
            nextFireTime = Time.time + (1f / currentFireRate);
        }
    }

    void EnableShooting()
    {
        canShoot = true;
        shootingStartTime = Time.time;
    }

    void ShootAtPlayer()
    {
        Vector2 direction = firePoint.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        Instantiate(
            bulletPrefab,
            firePoint.position,
            rotation
        );
        AudioManager.instance.PlaySound(1);
    }
}
