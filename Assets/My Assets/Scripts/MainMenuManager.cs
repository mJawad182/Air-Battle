using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public GameObject loadingMenu;
    public GameObject mainMenu;
    public GameObject Settings;
    public Slider loadingSlider;
    
    [Header("High Score")]
    public TextMeshProUGUI highScoreText;
    private void Awake()
    {
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f; // CRITICAL
        UpdateHighScore();
        StartCoroutine(Loading());
    }
    
    /// <summary>
    /// Updates the high score text from PlayerPrefs
    /// </summary>
    void UpdateHighScore()
    {
        if (highScoreText != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = highScore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame() 
    {
        AudioManager.instance.PlayClickSound();
        StartCoroutine(PlayGameDelay());
    }
    IEnumerator PlayGameDelay()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("LevelSelection");
    }
    public void LoadSettings()
    {
        Settings.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void saveSettings()
    {
        Settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void ExitGame()
    {
        AudioManager.instance.PlayClickSound();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    IEnumerator Loading()
    {
        loadingMenu.SetActive(true);
        mainMenu.SetActive(false);

        loadingSlider.value = 0f;

        while (loadingSlider.value < 1f)
        {
            loadingSlider.value += Time.unscaledDeltaTime * 0.5f;
            yield return null;
        }

        mainMenu.SetActive(true);
        loadingMenu.SetActive(false);
    }

}
