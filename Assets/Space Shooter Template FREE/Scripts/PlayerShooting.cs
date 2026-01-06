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
    private SpriteRenderer playerRenderer;
    
    // Store weapon power before Final Enemy battle
    private int savedWeaponPower = -1;
    
    // Sound indices
    private const int NORMAL_SHOOT_SOUND = 0;
    private const int FINAL_ENEMY_SHOOT_SOUND = 7;
    private int currentShootSound = NORMAL_SHOOT_SOUND;



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
        playerRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        levelTimer += Time.deltaTime;

        // Stop shooting after 120 seconds of THIS level
        if (levelTimer >= 147f)
            return;

        // Don't shoot if player is not visible (respawning/blinking)
        if (playerRenderer != null && !playerRenderer.enabled)
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
    
    // Check if Final Enemy is currently in the scene
    public bool IsFinalEnemyPresent()
    {
        return GameObject.Find("Final Enemy(Clone)") != null || GameObject.Find("Low Final Enemy(Clone)") != null;
    }
    
    // Check if missiles/rockets are currently active
    public bool AreMissilesActive()
    {
        return true;
    }
    
    // Check if weapon power should be limited (Final Enemy present AND missiles active)
    public bool ShouldLimitWeaponPower()
    {
        return IsFinalEnemyPresent();
    }
    
    void UpdateCurrentProjectile()
    {
        // Check if Final Enemy clone exists in scene
        if (IsFinalEnemyPresent() && finalEnemyProjectile != null)
        {
            currentProjectile = finalEnemyProjectile;
            currentShootSound = FINAL_ENEMY_SHOOT_SOUND;
            
            // Save weapon power before limiting (only once)
            if (AreMissilesActive() && savedWeaponPower == -1)
            {
                savedWeaponPower = weaponPower;
            }
            
            // Limit weapon power to 2 when Final Enemy is present and missiles are active
            if (AreMissilesActive() && weaponPower > 2)
            {
                weaponPower = 2;
            }
        }
        else
        {
            currentProjectile = projectileObject;
            currentShootSound = NORMAL_SHOOT_SOUND;
        }
    }
    
    // Revert to normal state when Final Enemy is removed
    public void RevertToNormalState()
    {
        // Revert to original projectile
        currentProjectile = projectileObject;
        
        // Revert to original shooting sound
        currentShootSound = NORMAL_SHOOT_SOUND;
        
        // Restore saved weapon power if it was saved
        if (savedWeaponPower != -1)
        {
            weaponPower = savedWeaponPower;
            savedWeaponPower = -1;
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
        // When Final Enemy is present AND missiles are active, limit weapon power to 2
        int effectivePower = ShouldLimitWeaponPower() ? Mathf.Min(weaponPower, 2) : weaponPower;
        
        switch (effectivePower) // according to weapon power 'pooling' the defined anount of projectiles, on the defined position, in the defined rotation
        {
            case 1:
                CreateLazerShot(currentProjectile, guns.centralGun.transform.position, Vector3.zero);
                guns.centralGunVFX.Play();
                AudioManager.instance.PlaySound(currentShootSound);
                break;
            case 2:
                CreateLazerShot(currentProjectile, guns.rightGun.transform.position, Vector3.zero);
                guns.leftGunVFX.Play();
                AudioManager.instance.PlaySound(currentShootSound);
                CreateLazerShot(currentProjectile, guns.leftGun.transform.position, Vector3.zero);
                guns.rightGunVFX.Play();
                AudioManager.instance.PlaySound(currentShootSound);
                break;
            case 3:
                CreateLazerShot(currentProjectile, guns.centralGun.transform.position, Vector3.zero);
                CreateLazerShot(currentProjectile, guns.rightGun.transform.position, new Vector3(0, 0, -0.5f));
                guns.leftGunVFX.Play();
                AudioManager.instance.PlaySound(currentShootSound);
                CreateLazerShot(currentProjectile, guns.leftGun.transform.position, new Vector3(0, 0, 0.5f));
                guns.rightGunVFX.Play();
                AudioManager.instance.PlaySound(currentShootSound);
                break;
            case 4:
                CreateLazerShot(currentProjectile, guns.centralGun.transform.position, Vector3.zero);
                CreateLazerShot(currentProjectile, guns.rightGun.transform.position, new Vector3(0, 0, -0.5f));
                guns.leftGunVFX.Play();
                AudioManager.instance.PlaySound(currentShootSound);
                CreateLazerShot(currentProjectile, guns.leftGun.transform.position, new Vector3(0, 0, 0.5f));
                guns.rightGunVFX.Play();
                AudioManager.instance.PlaySound(currentShootSound);
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
