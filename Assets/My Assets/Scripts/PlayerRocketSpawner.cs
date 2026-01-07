using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocketSpawner : MonoBehaviour
{
    public static PlayerRocketSpawner instance;

    [Header("Rocket Settings")]
    public GameObject rocketPrefab;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public float baseFireRate = 1f; // seconds between spawns
    public float rocketSpeed = 10f;
    public float divergenceAngle = 10f; // angle for outward movement

    [HideInInspector] public float currentFireRate;

    private float nextFireTime = 0f;
    private float lastSpawnPointCheck = 0f;
    private const float SPAWN_POINT_CHECK_INTERVAL = 1f; // Check every second

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentFireRate = baseFireRate; // initialize with base rate
        nextFireTime = Time.time + currentFireRate; // Initialize next fire time
    }
    
    private void Start()
    {
        // Find spawn points if not assigned
        FindSpawnPoints();
    }
    
    private void FindSpawnPoints()
    {
        // If spawn points are null or destroyed, try to find them
        bool needLeft = leftSpawnPoint == null || leftSpawnPoint.gameObject == null;
        bool needRight = rightSpawnPoint == null || rightSpawnPoint.gameObject == null;
        
        if (needLeft || needRight)
        {
            // First try to find in player's children (most reliable)
            if (Player.instance != null && Player.instance.gameObject != null)
            {
                FindSpawnPointsInHierarchy(Player.instance.transform, ref needLeft, ref needRight);
            }
            
            // If still not found, search all transforms in scene
            if (needLeft || needRight)
            {
                Transform[] allTransforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
                foreach (Transform t in allTransforms)
                {
                    if (t == null || t.gameObject == null) continue;
                    
                    string name = t.name.ToLower();
                    if (needLeft && (name.Contains("left") || name.Contains("rocket")))
                    {
                        // Prefer transforms that are children of Player
                        if (t.IsChildOf(Player.instance != null ? Player.instance.transform : null))
                        {
                            leftSpawnPoint = t;
                            needLeft = false;
                        }
                        else if (leftSpawnPoint == null)
                        {
                            leftSpawnPoint = t;
                        }
                    }
                    if (needRight && (name.Contains("right") || name.Contains("rocket")))
                    {
                        // Prefer transforms that are children of Player
                        if (t.IsChildOf(Player.instance != null ? Player.instance.transform : null))
                        {
                            rightSpawnPoint = t;
                            needRight = false;
                        }
                        else if (rightSpawnPoint == null)
                        {
                            rightSpawnPoint = t;
                        }
                    }
                }
            }
        }
    }
    
    private void FindSpawnPointsInHierarchy(Transform parent, ref bool needLeft, ref bool needRight)
    {
        if (parent == null) return;
        
        foreach (Transform child in parent)
        {
            if (child == null || child.gameObject == null) continue;
            
            string name = child.name.ToLower();
            
            if (needLeft && (name.Contains("left") || name.Contains("rocket")))
            {
                // Check if it has children that might be spawn points
                if (child.childCount > 0)
                {
                    foreach (Transform subChild in child)
                    {
                        if (subChild != null && subChild.name.ToLower().Contains("left"))
                        {
                            leftSpawnPoint = subChild;
                            needLeft = false;
                            break;
                        }
                    }
                }
                if (needLeft)
                {
                    leftSpawnPoint = child;
                    needLeft = false;
                }
            }
            
            if (needRight && (name.Contains("right") || name.Contains("rocket")))
            {
                // Check if it has children that might be spawn points
                if (child.childCount > 0)
                {
                    foreach (Transform subChild in child)
                    {
                        if (subChild != null && subChild.name.ToLower().Contains("right"))
                        {
                            rightSpawnPoint = subChild;
                            needRight = false;
                            break;
                        }
                    }
                }
                if (needRight)
                {
                    rightSpawnPoint = child;
                    needRight = false;
                }
            }
            
            // Recursively search children
            if (needLeft || needRight)
            {
                FindSpawnPointsInHierarchy(child, ref needLeft, ref needRight);
            }
        }
    }

    private void Update()
    {
        // Stop spawning rockets after 120 seconds
        if (Time.time >= 190f) return;
        
        // Only spawn if rockets GameObject is active
        if (RocketController.instance == null || 
            RocketController.instance.rockets == null ||
            !RocketController.instance.rockets.activeSelf)
        {
            return;
        }
        
        // Periodically check spawn points
        if (Time.time - lastSpawnPointCheck >= SPAWN_POINT_CHECK_INTERVAL)
        {
            FindSpawnPoints();
            lastSpawnPointCheck = Time.time;
        }
        
        // Spawn rockets based on fire rate
        if (Time.time >= nextFireTime)
        {
            if (leftSpawnPoint != null && rightSpawnPoint != null && rocketPrefab != null)
            {
                SpawnRockets();
                nextFireTime = Time.time + currentFireRate;
            }
            else
            {
                // If spawn points are invalid, try to find them and reset timer
                FindSpawnPoints();
                nextFireTime = Time.time + currentFireRate;
            }
        }
    }

    void SpawnRockets()
    {
        if (leftSpawnPoint == null || rightSpawnPoint == null || 
            rocketPrefab == null || 
            leftSpawnPoint.gameObject == null || rightSpawnPoint.gameObject == null)
        {
            return;
        }
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySound(6);
        }
        
        // Left rocket: rotated slightly outward
        GameObject leftRocket = Instantiate(rocketPrefab, leftSpawnPoint.position,
            Quaternion.Euler(0, 0, divergenceAngle));
        
        // Right rocket: rotated slightly outward
        GameObject rightRocket = Instantiate(rocketPrefab, rightSpawnPoint.position,
            Quaternion.Euler(0, 0, -divergenceAngle));
    }

    // Call this method from collectables to increase spawn speed
    public void IncreaseSpawnSpeed(float decrement)
    {
        currentFireRate = Mathf.Max(0.1f, currentFireRate - decrement);
    }
    
    // Public method to activate rockets (can be called externally)
    public void ActivateRockets()
    {
        if (RocketController.instance != null && RocketController.instance.rockets != null)
        {
            RocketController.instance.rockets.SetActive(true);
        }
    }
}
