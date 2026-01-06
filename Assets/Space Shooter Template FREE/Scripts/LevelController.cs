using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Serializable classes
[System.Serializable]
public class EnemyWaves 
{
    [Tooltip("time for wave generation from the moment the game started")]
    public float timeToStart;

    [Tooltip("Enemy wave's prefab")]
    public GameObject wave;
}

#endregion

public class LevelController : MonoBehaviour {

    //Serializable classes implements
    public EnemyWaves[] enemyWaves; 

    public GameObject powerUp;
    public GameObject powerUp2;
    public float timeForNewPowerup;
    public GameObject[] planets;
    public float timeBetweenPlanets;
    public float planetsSpeed;
    List<GameObject> planetsList = new List<GameObject>();

    [Header("Life Pickup")]
    public GameObject lifePowerUp;
    public float lifeSpawnTimeMin = 15f;
    public float lifeSpawnTimeMax = 45f;

    Camera mainCamera;

    public Wave[] waves;

    [Header("Shooting Control")]
    public float shotTimeMin = 1.5f;
    public float shotTimeMax = 4f;
    
    [Header("Difficulty Ramp")]
    [Tooltip("Starting value for difficulty ramp (decreases to 0)")]
    public float difficultyShotTimeMin = 1f;
    [Tooltip("Time in seconds to decrease difficultyShotTimeMin from 1 to 0")]
    public float difficultyRampDuration = 110f;
    
    private float currentDifficultyShotTime;
    private float elapsedTime = 0f;

    private void Awake()
    {
        currentDifficultyShotTime = difficultyShotTimeMin; // Start at initial value (1)
        
        foreach (Wave wave in waves)
        {
            wave.shooting.shotTimeMin = currentDifficultyShotTime;
            wave.shooting.shotTimeMax = shotTimeMax;
        }
    }
    
    private void Update()
    {
        // Gradually decrease currentDifficultyShotTime from 1 to 0 over 80 seconds
        if (elapsedTime < difficultyRampDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // Linear interpolation: goes from difficultyShotTimeMin (1) to 0
            float t = Mathf.Clamp01(elapsedTime / difficultyRampDuration);
            currentDifficultyShotTime = Mathf.Lerp(difficultyShotTimeMin, 0f, t);
            
            // Update all waves' shooting parameters so new enemies get the updated value
            foreach (Wave wave in waves)
            {
                if (wave != null && wave.shooting != null)
                {
                    wave.shooting.shotTimeMin = currentDifficultyShotTime;
                }
            }
        }
    }
    private void Start()
    {
        mainCamera = Camera.main;
        //for each element in 'enemyWaves' array creating coroutine which generates the wave
        for (int i = 0; i<enemyWaves.Length; i++) 
        {
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave));
        }
        StartCoroutine(PowerupBonusCreation());
        StartCoroutine(PlanetsCreation());
        StartCoroutine(SpawnLifePowerUpOnce());
    }
    
    //Create a new wave after a delay
    IEnumerator CreateEnemyWave(float delay, GameObject Wave) 
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        if (Player.instance != null)
            Instantiate(Wave);
    }

    //endless coroutine generating 'levelUp' bonuses. 
    IEnumerator PowerupBonusCreation() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeForNewPowerup);
            int randomPower = Random.Range(0, 5);
            if (randomPower == 3)
            {
                Instantiate(
                powerUp2,
                //Set the position for the new bonus: for X-axis - random position between the borders of 'Player's' movement; for Y-axis - right above the upper screen border 
                new Vector2(
                    Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX),
                    mainCamera.ViewportToWorldPoint(Vector2.up).y + powerUp.GetComponent<Renderer>().bounds.size.y / 2),
                Quaternion.identity
                );
            }else
            {
                Instantiate(
                    powerUp,
                    //Set the position for the new bonus: for X-axis - random position between the borders of 'Player's' movement; for Y-axis - right above the upper screen border 
                    new Vector2(
                        Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX),
                        mainCamera.ViewportToWorldPoint(Vector2.up).y + powerUp.GetComponent<Renderer>().bounds.size.y / 2),
                    Quaternion.identity
                    );
            }
                
        }
    }

    IEnumerator PlanetsCreation()
    {
        //Create a new list copying the arrey
        for (int i = 0; i < planets.Length; i++)
        {
            planetsList.Add(planets[i]);
        }
        yield return new WaitForSeconds(10);
        while (true)
        {
            ////choose random object from the list, generate and delete it
            int randomIndex = Random.Range(0, planetsList.Count);
            GameObject newPlanet = Instantiate(planetsList[randomIndex]);
            planetsList.RemoveAt(randomIndex);
            //if the list decreased to zero, reinstall it
            if (planetsList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    planetsList.Add(planets[i]);
                }
            }
            newPlanet.GetComponent<DirectMoving>().speed = planetsSpeed;

            yield return new WaitForSeconds(timeBetweenPlanets);
        }
    }

    // Spawns life powerup only once per level at a random time
    IEnumerator SpawnLifePowerUpOnce()
    {
        // Wait for random time between min and max
        float randomTime = Random.Range(lifeSpawnTimeMin, lifeSpawnTimeMax);
        yield return new WaitForSeconds(randomTime);

        // Only spawn if player is still alive and lifePowerUp is assigned
        if (Player.instance != null && lifePowerUp != null)
        {
            Instantiate(
                lifePowerUp,
                new Vector2(
                    Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX),
                    mainCamera.ViewportToWorldPoint(Vector2.up).y + lifePowerUp.GetComponent<Renderer>().bounds.size.y / 2),
                Quaternion.identity
            );
        }
    }
}
