using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    private const string LEVEL_KEY = "UnlockedLevel";
    private const int MAX_LEVEL = 4;
    private const int FIRST_LEVEL_BUILD_INDEX = 2; // Level1


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize PlayerPrefs if not present
        if (!PlayerPrefs.HasKey(LEVEL_KEY))
        {
            PlayerPrefs.SetInt(LEVEL_KEY, 1);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Call this when the player wins a level.
    /// Unlocks the next level if available.
    /// </summary>
    public void OnLevelWin()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        // Convert build index → level number
        int currentLevelNumber = currentBuildIndex - FIRST_LEVEL_BUILD_INDEX + 1;
        int nextLevelNumber = currentLevelNumber + 1;

        int unlockedLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);

        if (nextLevelNumber <= MAX_LEVEL && nextLevelNumber > unlockedLevel)
        {
            PlayerPrefs.SetInt(LEVEL_KEY, nextLevelNumber);
            PlayerPrefs.Save();
        }
    }


    /// <summary>
    /// Loads the next level if it exists.
    /// </summary>
    public void LoadNextLevel()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextBuildIndex = currentBuildIndex + 1;

        int nextLevelNumber = nextBuildIndex - FIRST_LEVEL_BUILD_INDEX + 1;

        if (nextLevelNumber <= MAX_LEVEL)
        {
            SceneManager.LoadScene(nextBuildIndex);
        }
    }


    /// <summary>
    /// Loads a specific level (used by level select screen).
    /// </summary>
    public void LoadLevel(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > MAX_LEVEL)
            return;

        int buildIndex = FIRST_LEVEL_BUILD_INDEX + (levelNumber - 1);
        SceneManager.LoadScene(buildIndex);
    }




    public int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }
}
