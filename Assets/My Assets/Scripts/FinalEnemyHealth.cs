using UnityEngine;

public class FinalEnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Effects")]
    public GameObject destructionVFX;
    public GameObject hitEffect;

    private bool isActivated = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Called when Final Enemy reaches its target position and battle starts
    public void ActivateBattle()
    {
        if (isActivated) return;
        isActivated = true;

        // Play missile activation sound (index 7)
        AudioManager.instance.PlaySound(7);

        // Activate missiles/rockets
        
    }

    public void GetDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destruction();
        }
        else if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
        }
    }

    void Destruction()
    {
        // Play destruction sound
        AudioManager.instance.PlaySound(5);

        // Spawn destruction VFX
        if (destructionVFX != null)
        {
            Instantiate(destructionVFX, transform.position, Quaternion.identity);
        }

        // Revert everything before destroying
        RevertEverything();

        Destroy(gameObject);
    }

    // Called when Final Enemy is destroyed or exits the screen
    public void RevertEverything()
    {
        // Revert PlayerShooting to normal state (bullet type, sound, weapon power)
        if (PlayerShooting.instance != null)
        {
            PlayerShooting.instance.RevertToNormalState();
        }
    }

    private void OnDestroy()
    {
        // Safety check - revert if destroyed by any means
        if (isActivated)
        {
            RevertEverything();
        }
    }
}

