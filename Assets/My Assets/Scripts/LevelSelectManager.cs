using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Level Buttons (1 to 10)")]
    public Button[] levelButtons;

    [Header("Button Images (same order as buttons)")]
    public Image[] buttonImages;

    [Header("Sprites")]
    public Sprite lockedSprite;
    public Sprite[] levelActiveSprites; // Level 1�10 sprites

    private const int MAX_LEVEL = 4;

    private void Start()
    {
        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < MAX_LEVEL; i++)
        {
            int levelIndex = i + 1;

            if (levelIndex <= unlockedLevel)
            {
                levelButtons[i].interactable = true;
                buttonImages[i].sprite = levelActiveSprites[i];
            }
            else
            {
                levelButtons[i].interactable = false;
                buttonImages[i].sprite = lockedSprite;
            }
        }
    }

    /// <summary>
    /// Assigned to button OnClick (pass level index 1�10)
    /// </summary>
    public void OnLevelButtonClicked(int levelIndex)
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

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
}
