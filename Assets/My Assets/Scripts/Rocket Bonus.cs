using UnityEngine;

public class RocketBonus : MonoBehaviour
{
    private GameObject rockets;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Activate the rocket system
            rockets = RocketController.instance.rockets;
            rockets.SetActive(true);

            // Increase weapon power if not max
            if (PlayerShooting.instance.weaponPower < PlayerShooting.instance.maxweaponPower)
            {
                PlayerShooting.instance.weaponPower++;
            }

            // Increase rocket fire rate
            if (PlayerRocketSpawner.instance != null)
            {
                PlayerRocketSpawner.instance.IncreaseSpawnSpeed(0.2f); // decreases fire delay, i.e., increases rate
            }

            // Destroy the collectable
            Destroy(gameObject);
        }
    }
}
