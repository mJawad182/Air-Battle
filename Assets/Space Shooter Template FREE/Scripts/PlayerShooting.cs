using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//guns objects in 'Player's' hierarchy
[System.Serializable]
public class Guns
{
    public GameObject rightGun, leftGun, centralGun;
    [HideInInspector] public ParticleSystem leftGunVFX, rightGunVFX, centralGunVFX; 
}
    

public class PlayerShooting : MonoBehaviour {
    public bool isRocketActive = false;
    [Tooltip("shooting frequency. the higher the more frequent")]
    public float fireRate;

    [Tooltip("projectile prefab")]
    public GameObject projectileObject;
    
    [Tooltip("projectile prefab to use when Final Enemy is present")]
    public GameObject finalEnemyProjectile;

    //time for a new shot
    //[HideInInspector] public float nextFire;


    [Tooltip("current weapon power")]
    [Range(1, 4)]       //change it if you wish
    public int weaponPower = 1; 

    public Guns guns;
    bool shootingIsActive = true; 
    [HideInInspector] public int maxweaponPower = 4; 
    public static PlayerShooting instance;
    private float levelTimer;
    private float fireTimer;
    private GameObject currentProjectile;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        StartCoroutine(FirstShot());
        guns.leftGunVFX = guns.leftGun.GetComponent<ParticleSystem>();
        guns.rightGunVFX = guns.rightGun.GetComponent<ParticleSystem>();
        guns.centralGunVFX = guns.centralGun.GetComponent<ParticleSystem>();

        levelTimer = 0f;
        fireTimer = 0f;
        currentProjectile = projectileObject; // Default projectile
    }


    private void Update()
    {
        levelTimer += Time.deltaTime;

        // Stop shooting after 120 seconds of THIS level
        if (levelTimer >= 122f)
            return;

        // Check if Final Enemy is in scene and switch projectile accordingly
        UpdateCurrentProjectile();

        fireTimer += Time.deltaTime;

        if (shootingIsActive && fireTimer >= 1f / fireRate)
        {
            MakeAShot();
            fireTimer = 0f;
        }
    }
    
    void UpdateCurrentProjectile()
    {
        // Check if Final Enemy clone exists in scene
        GameObject finalEnemy = GameObject.Find("Final Enemy(Clone)");
        
        if (finalEnemy != null && finalEnemyProjectile != null)
        {
            currentProjectile = finalEnemyProjectile;
        }
        else
        {
            currentProjectile = projectileObject;
        }
    }



    //IEnumerator FirstShotDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    MakeAShot();
    //    nextFire = Time.time + 1 / fireRate; // schedule next shot
    //}

    //method for a shot
    void MakeAShot() 
    {
        switch (weaponPower) // according to weapon power 'pooling' the defined anount of projectiles, on the defined position, in the defined rotation
        {
            case 1:
                CreateLazerShot(currentProjectile, guns.centralGun.transform.position, Vector3.zero);
                guns.centralGunVFX.Play();
                AudioManager.instance.PlaySound(0);
                break;
            case 2:
                CreateLazerShot(currentProjectile, guns.rightGun.transform.position, Vector3.zero);
                guns.leftGunVFX.Play();
                AudioManager.instance.PlaySound(0);
                CreateLazerShot(currentProjectile, guns.leftGun.transform.position, Vector3.zero);
                guns.rightGunVFX.Play();
                AudioManager.instance.PlaySound(0);
                break;
            case 3:
                CreateLazerShot(currentProjectile, guns.centralGun.transform.position, Vector3.zero);
                CreateLazerShot(currentProjectile, guns.rightGun.transform.position, new Vector3(0, 0, -0.5f));
                guns.leftGunVFX.Play();
                AudioManager.instance.PlaySound(0);
                CreateLazerShot(currentProjectile, guns.leftGun.transform.position, new Vector3(0, 0, 0.5f));
                guns.rightGunVFX.Play();
                AudioManager.instance.PlaySound(0);
                break;
            case 4:
                CreateLazerShot(currentProjectile, guns.centralGun.transform.position, Vector3.zero);
                CreateLazerShot(currentProjectile, guns.rightGun.transform.position, new Vector3(0, 0, -0.5f));
                guns.leftGunVFX.Play();
                AudioManager.instance.PlaySound(0);
                CreateLazerShot(currentProjectile, guns.leftGun.transform.position, new Vector3(0, 0, 0.5f));
                guns.rightGunVFX.Play();
                AudioManager.instance.PlaySound(0);
                CreateLazerShot(currentProjectile, guns.leftGun.transform.position, new Vector3(0, 0, 1.5f));
                CreateLazerShot(currentProjectile, guns.rightGun.transform.position, new Vector3(0, 0, -1.5f));
                break;
        }
    }

    void CreateLazerShot(GameObject lazer, Vector3 pos, Vector3 rot) //translating 'pooled' lazer shot to the defined position in the defined rotation
    {
        Instantiate(lazer, pos, Quaternion.Euler(rot));
    }
    IEnumerator FirstShot()
    {
        shootingIsActive = false;
        yield return new WaitForSeconds(0.3f);
        shootingIsActive = true;
    }
}
