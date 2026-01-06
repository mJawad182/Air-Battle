using UnityEngine;

public class Bonus : MonoBehaviour {

    //when colliding with another object, if another objct is 'Player', sending command to the 'Player'
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Player") 
        {
            // When Final Enemy is present AND missiles are active, limit weapon power to 2
            int maxPower = PlayerShooting.instance.ShouldLimitWeaponPower() ? 2 : PlayerShooting.instance.maxweaponPower;
            
            if (PlayerShooting.instance.weaponPower < maxPower)
            {
                PlayerShooting.instance.weaponPower++;
            }
            Destroy(gameObject);
        }
    }
}
