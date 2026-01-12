using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;        // UI Text to display current score
    public TextMeshProUGUI highScoreText;    // UI Text to display high score

    private int currentScore = 0;
    private int highScore = 0;

    private const string PlayerPrefScoreKey = "Score";
    private const string PlayerPrefHighScoreKey = "HighScore";

    private void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ResetScore();
        LoadScores();
        UpdateUI();
    }

    // Add points to the score
    public void AddScore(int points)
    {
        currentScore += points;

        // Check and update high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(PlayerPrefHighScoreKey, highScore);
        }

        // Save current score
        PlayerPrefs.SetInt(PlayerPrefScoreKey, currentScore);
        PlayerPrefs.Save();

        UpdateUI();
    }

    // Reset current score
    public void ResetScore()
    {
        currentScore = 0;
        PlayerPrefs.SetInt(PlayerPrefScoreKey, currentScore);
        PlayerPrefs.Save();
        UpdateUI();
    }

    // Load scores from PlayerPrefs
    private void LoadScores()
    {
        currentScore = PlayerPrefs.GetInt(PlayerPrefScoreKey, 0);
        highScore = PlayerPrefs.GetInt(PlayerPrefHighScoreKey, 0);
    }

    // Update UI elements
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "" + currentScore;

        if (highScoreText != null)
            highScoreText.text = "" + highScore;
    }

    // Optional: Get current score
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Optional: Get high score
    public int GetHighScore()
    {
        return highScore;
    }
}
