using UnityEngine;

public class RareEnemyShooting : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;

    [Header("Shooting Settings")]
    [Tooltip("Shots per second")]
    public float fireRate = 1f;

    [Tooltip("Delay before shooting starts")]
    public float startDelay = 0.5f;

    private float nextFireTime;
    private bool canShoot = false;

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

        Invoke(nameof(EnableShooting), startDelay);
    }

    private void Update()
    {
        if (!canShoot || player == null)
            return;

        if (Time.time >= nextFireTime)
        {
            ShootAtPlayer();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void EnableShooting()
    {
        canShoot = true;
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
