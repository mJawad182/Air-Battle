using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public GameObject rockets;
    public ParticleSystem[] vfxArray;
    private float levelTimer;




    private void Awake()
    {
        ApplySfxSettings();
        
        animator.enabled = true;
        Time.timeScale = 0f;

        // Start the initial game delay
        StartCoroutine(StartGameDelay(8f));

        // Start checking for 120 seconds gameplay
        StartCoroutine(CheckGameTime());
    }
    private void Start()
    {
        levelTimer = 0f;
    }

    private void ApplySfxSettings()
    {
        if (AudioSettingsManager.Instance != null)
        {
            AudioSettingsManager.Instance.ApplySfxState();
        }
    }
    IEnumerator StartGameDelay(float delay)
    {
        DisableAllVFX();
        AudioManager.instance.PlaySound(2); 
        // Use unscaled time so it works while timeScale = 0
        yield return new WaitForSecondsRealtime(delay);
        if(animator != null)
        {
            animator.enabled = false;
        }
        
        Time.timeScale = 1f;
        EnableAllVFX();
    }

    IEnumerator CheckGameTime()
    {
        while (levelTimer < 147f)
        {
            levelTimer += Time.deltaTime;
            yield return null;
        }

        AudioManager.instance.PlaySound(3);

        yield return new WaitForSecondsRealtime(3f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.transform.position;
        pos.x = 0f;
        player.transform.position = pos;
        yield return new WaitForSecondsRealtime(2f);

        foreach (var obj in GameObject.FindGameObjectsWithTag("Bonus"))
            Destroy(obj);

        animator.enabled = true;
        animator.Play("ReverseJetFlip", 0, 0f);
        rockets.SetActive(false);

        // Let animation + background play
        yield return new WaitForSeconds(4f); // use SCALED time

        // NOW freeze the game
        Time.timeScale = 0f;

        CleanupScene();

        GameUIManager.Instance.ShowWinPanel();
    }

    private void CleanupScene()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Projectile"))
            Destroy(obj);

        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(obj);

        
        

        var background = GameObject.FindGameObjectWithTag("Background");
        if (background != null)
            background.GetComponent<DirectMoving>().speed = 0f;
    }

    // Turn all VFX on
    public void EnableAllVFX()
    {
        foreach (ParticleSystem ps in vfxArray)
        {
            ps.gameObject.SetActive(true);
            ps.Play();
        }
    }

    // Turn all VFX off
    public void DisableAllVFX()
    {
        foreach (ParticleSystem ps in vfxArray)
        {
            ps.Stop();
            ps.gameObject.SetActive(false);
        }
    }
}
