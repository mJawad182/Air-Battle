using UnityEngine;

public class LifeCollectable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIManager.Instance.AddLife();
            Debug.Log("Life added!");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameUIManager.Instance.AddLife();
            Debug.Log("Life added!");
            Destroy(gameObject);
        }
    }
}
