using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Level Buttons (1 to 10)")]
    public Button[] levelButtons;

    [Header("Button Images (same order as buttons)")]
    public Image[] buttonImages;

    [Header("Lock Screens (Optional - if not assigned, will get from 2nd child of button)")]
    public GameObject[] lockScreens; // Lock screen GameObjects (same order as buttons)

    [Header("Level Unlock Settings")]
    [Tooltip("If enabled, all levels will be unlocked. If disabled, uses normal progression based on PlayerPrefs.")]
    public bool unlockAllLevels = false;

    [Header("Sprites")]
    public Sprite lockedSprite;
    public Sprite[] levelActiveSprites; // Level 1�10 sprites

    private const int MAX_LEVEL = 4;

    private void Start()
    {
        UpdateLevelButtons();
    }
    
    /// <summary>
    /// Gets the lock screen GameObject for a button index
    /// First tries to use provided lockScreens array, otherwise gets from 2nd child of button
    /// </summary>
    private GameObject GetLockScreen(int buttonIndex)
    {
        // If lock screens array is provided and has enough elements, use it
        if (lockScreens != null && buttonIndex < lockScreens.Length && lockScreens[buttonIndex] != null)
        {
            return lockScreens[buttonIndex];
        }
        
        // Otherwise, get from 2nd child of button (index 1)
        if (buttonIndex < levelButtons.Length && levelButtons[buttonIndex] != null)
        {
            if (levelButtons[buttonIndex].transform.childCount > 1)
            {
                return levelButtons[buttonIndex].transform.GetChild(1).gameObject;
            }
        }
        
        return null;
    }

    private void UpdateLevelButtons()
    {
        int unlockedLevel;
        
        // If unlockAllLevels is enabled, unlock all levels
        if (unlockAllLevels)
        {
            unlockedLevel = MAX_LEVEL; // Unlock all levels
        }
        else
        {
            unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Normal progression
        }

        for (int i = 0; i < MAX_LEVEL; i++)
        {
            int levelIndex = i + 1;

            if (levelIndex <= unlockedLevel)
            {
                // Unlocked: Make button interactable and hide lock screen
                levelButtons[i].interactable = true;
                
                // Hide lock screen - use provided reference or get from 2nd child
                GameObject lockScreen = GetLockScreen(i);
                if (lockScreen != null)
                {
                    lockScreen.SetActive(false);
                }
            }
            else
            {
                // Locked: Make button non-interactable and show lock screen
                levelButtons[i].interactable = false;
                
                // Show lock screen - use provided reference or get from 2nd child
                GameObject lockScreen = GetLockScreen(i);
                if (lockScreen != null)
                {
                    lockScreen.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Assigned to button OnClick (pass level index 1�10)
    /// </summary>
    public void OnLevelButtonClicked(int levelIndex)
    {
        int unlockedLevel;
        
        // If unlockAllLevels is enabled, allow all levels
        if (unlockAllLevels)
        {
            unlockedLevel = MAX_LEVEL;
        }
        else
        {
            unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        }

        if (levelIndex <= unlockedLevel)
        {
            PlayerPrefs.SetInt("SelectedLevel", levelIndex);
            PlayerPrefs.Save();

            //LevelProgressManager.Instance.LoadLevel(levelIndex);
        }
    }
    public void SELECTLEVELBTN()
    {
        LevelProgressManager.Instance.LoadLevel(PlayerPrefs.GetInt("SelectedLevel"));

    }

    public void Back(){ 
         AudioManager.instance.PlayClickSound();
        StartCoroutine(BackDelay());
    }
    IEnumerator BackDelay()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Main Menu");
    }
}
