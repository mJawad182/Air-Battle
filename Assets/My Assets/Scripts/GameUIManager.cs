using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    [HideInInspector]
    public static bool isGamePaused = false; // Track pause
    
    private bool isLoadingScene = false; // Prevent multiple simultaneous scene loads
    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI Coins;

    [Header("Lives System")]
    public Image[] lifeSprites;          // Array of 3 life UI Images
    public Sprite activeLifeSprite;      // Normal life sprite
    public Sprite inactiveLifeSprite;    // Blacked out life sprite
    
    private int currentLives = 3;
    private const int MAX_LIVES = 3;
    
    public bool HasLivesRemaining => currentLives > 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HideAllPanels();
        isGamePaused = false;
        isLoadingScene = false;
        InitializeLives();
        
        // Ensure time scale is normal at start
        Time.timeScale = 1f;
    }

    // --- PAUSE PANEL ---
    public void ShowPausePanel()
    {
        if(pausePanel != null)
            pausePanel.SetActive(true);

        // Pause game
        Time.timeScale = 0f;
        isGamePaused = true;

        // Pause audio
        if (AudioManager.instance.audioSource != null && AudioManager.instance.audioSource.isPlaying)
            AudioManager.instance.audioSource.Pause();
    }


    public void HidePausePanel()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Resume game
        Time.timeScale = 1f;
        isGamePaused = false;

        // Resume audio
        if (AudioManager.instance.audioSource != null)
            AudioManager.instance.audioSource.UnPause();
    }




    public void ResumeGame()
    {
        Time.timeScale = 1f;
        HidePausePanel();
    }

    // --- WIN PANEL ---
    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
        Coins.text = PlayerPrefs.GetInt("HighScore").ToString();
        LevelProgressManager.Instance.OnLevelWin();
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        if (isLoadingScene) return; // Prevent multiple calls
        
        AudioManager.instance.PlaySound(10);
        StartCoroutine(NextLevelDelay());
    }
    IEnumerator NextLevelDelay()
    {
        isLoadingScene = true;
        
        // Reset time scale BEFORE loading to prevent glitches
        Time.timeScale = 1f;
        isGamePaused = false;
        
        // Use WaitForSecondsRealtime since game might be paused
        yield return new WaitForSecondsRealtime(0.1f);
        
        // Load next level
        LevelProgressManager.Instance.LoadNextLevel();
        
        // Ensure time scale is normal after load
        Time.timeScale = 1f;
    }

    public void HideWinPanel()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // --- LOSE PANEL ---
    public void ShowLosePanel()
    {
        if (losePanel != null)
            losePanel.SetActive(true);

        isGamePaused = true;
        Time.timeScale = 0f;

        GameObject background = GameObject.FindGameObjectWithTag("Background");
        if (background != null)
            background.GetComponent<DirectMoving>().speed = 0f;
    }

    public void HideLosePanel()
    {
        if (losePanel != null)
            losePanel.SetActive(false);

        isGamePaused = false;
        Time.timeScale = 1f;
    }

    // Hide all panels at start
    private void HideAllPanels()
    {
        
       
    }

    // --- LIVES SYSTEM ---
    
    /// <summary>
    /// Initialize lives at start of game
    /// </summary>
    public void InitializeLives()
    {
        currentLives = MAX_LIVES;
        UpdateLifeSprites();
    }

    /// <summary>
    /// Call this when player gets hit. Returns true if player still has lives, false if player is destroyed.
    /// </summary>
    public bool LoseLife()
    {
        if (currentLives <= 0) return false;

        currentLives--;
        UpdateLifeSprites();

        if (currentLives <= 0)
        {
            
            return false;
        }

        return true;
    }

    /// <summary>
    /// Call this when player collects a life pickup. Returns true if life was added.
    /// </summary>
    public bool AddLife()
    {
        if (currentLives >= MAX_LIVES) return false;

        currentLives++;
        UpdateLifeSprites();
        return true;
        
    }

    /// <summary>
    /// Updates the life sprites based on current lives
    /// </summary>
    private void UpdateLifeSprites()
    {
        if (lifeSprites == null || lifeSprites.Length == 0) return;

        for (int i = 0; i < lifeSprites.Length; i++)
        {
            if (lifeSprites[i] != null)
            {
                // If index is less than current lives, show active sprite
                lifeSprites[i].sprite = (i < currentLives) ? activeLifeSprite : inactiveLifeSprite;
            }
        }
    }

    /// <summary>
    /// Called when player loses all lives
    /// </summary>
    private void OnPlayerDeath()
    {
        // You can add explosion effect, sound, etc. here
        StartLoseSequence(1.5f); // Show lose panel after delay
    }

    /// <summary>
    /// Get current lives count
    /// </summary>
    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void ReturnToHome()
    {
        if (isLoadingScene) return; // Prevent multiple calls
        
        AudioManager.instance.PlaySound(10);
        StartCoroutine(ReturnToHomeDelay());
    }
    IEnumerator ReturnToHomeDelay()
    {
        isLoadingScene = true;
        
        // Reset time scale BEFORE loading to prevent glitches
        Time.timeScale = 1f;
        isGamePaused = false;
        
        // Use WaitForSecondsRealtime since game might be paused
        yield return new WaitForSecondsRealtime(0.1f);
        
        // Load scene
        SceneManager.LoadScene("Main Menu");
        
        // Ensure time scale is normal after load
        Time.timeScale = 1f;
    }
    public void Replay()
    {
        if (isLoadingScene) return; // Prevent multiple calls
        
        AudioManager.instance.PlaySound(10);
        StartCoroutine(ReplayDelay());
    }
    IEnumerator ReplayDelay()
    {
        isLoadingScene = true;
        
        // Reset time scale BEFORE loading to prevent glitches
        Time.timeScale = 1f;
        isGamePaused = false;
        
        // Use WaitForSecondsRealtime since game might be paused
        yield return new WaitForSecondsRealtime(0.1f);
        
        // Load scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // Ensure time scale is normal after load
        Time.timeScale = 1f;
    }
    public void StartLoseSequence(float delay)
    {
        StartCoroutine(LosePanelDelay(delay));
    }

    private IEnumerator LosePanelDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowLosePanel();
    }
}
