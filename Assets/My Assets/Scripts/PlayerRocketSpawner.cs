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

    private void Awake()
    {
        if (instance == null)
            instance = this;

        currentFireRate = baseFireRate; // initialize with base rate
    }

    private void Update()
    {
        // Stop spawning rockets after 120 seconds
        if (Time.time >= 120f) return;

        if (Time.time >= nextFireTime)
        {
            SpawnRockets();
            nextFireTime = Time.time + currentFireRate;
        }
    }




    void SpawnRockets()
    {
        AudioManager.instance.PlaySound(6);
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
}

// Rocket movement script

