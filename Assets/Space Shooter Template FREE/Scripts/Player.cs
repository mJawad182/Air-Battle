using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject destructionFX;
    public static Player instance;
    
    [Header("Respawn Settings")]
    public int blinkCount = 5;
    public float blinkInterval = 0.15f;
    public Vector2 respawnPosition = new Vector2(0f, -6.2f);
    
    private GameObject engineChild;
    private GameObject rocketsChild;
    private bool rocketsWasActive = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        // Find the Engine and Rockets children by tag
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Engine"))
            {
                engineChild = child.gameObject;
            }
            else if (child.CompareTag("Rockets"))
            {
                rocketsChild = child.gameObject;
            }
        }
    }

    public void GetDamage(int damage)
    {
        GameUIManager.Instance.LoseLife();
        if(GameUIManager.Instance.GetCurrentLives() <= 0){
            Destruction();
        }else{
            Instantiate(destructionFX, transform.position, Quaternion.identity);
            AudioManager.instance.PlaySound(4);
            SetPlayerVisible(false);
            StartCoroutine(PlayerRespawnWait(1f));
        }
    }

    void SetPlayerVisible(bool visible)
    {
        // Disable renderer and collider instead of the whole GameObject
        // so coroutines can still run
        var renderer = GetComponent<SpriteRenderer>();
        var collider = GetComponent<Collider2D>();
        
        if (renderer != null) renderer.enabled = visible;
        if (collider != null) collider.enabled = visible;
        
        // Also set Engine child active/inactive
        if (engineChild != null) engineChild.SetActive(visible);
        
        // Handle Rockets child - save state when hiding, restore when showing
        if (rocketsChild != null)
        {
            if (!visible)
            {
                // Store if rockets was active before hiding
                rocketsWasActive = rocketsChild.activeSelf;
                rocketsChild.SetActive(false);
            }
            else
            {
                // Restore rockets state if it was active before
                if (rocketsWasActive)
                {
                    rocketsChild.SetActive(true);
                }
            }
        }
    }
    
    void SetRendererVisible(bool visible)
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = visible;
        
        // Also set Engine child active/inactive for blinking
        if (engineChild != null) engineChild.SetActive(visible);
        
        // Also blink Rockets if it was active
        if (rocketsChild != null && rocketsWasActive)
        {
            rocketsChild.SetActive(visible);
        }
    }
    
    void SetColliderEnabled(bool enabled)
    {
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = enabled;
    }

    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySound(4);

        GameUIManager.Instance.StartLoseSequence(1f);
        Destroy(gameObject);
    }


    IEnumerator LosePanelWait(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameUIManager.Instance.ShowLosePanel();
    }

    IEnumerator PlayerRespawnWait(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        // Reset position on respawn
        transform.position = respawnPosition;
        
        // Blink effect - player is invincible during blinking
        for (int i = 0; i < blinkCount; i++)
        {
            SetRendererVisible(true);
            yield return new WaitForSecondsRealtime(blinkInterval);
            SetRendererVisible(false);
            yield return new WaitForSecondsRealtime(blinkInterval);
        }
        
        // Fully visible and vulnerable again
        SetPlayerVisible(true);
    }
}
