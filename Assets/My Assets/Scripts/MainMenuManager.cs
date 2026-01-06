using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject loadingMenu;
    public GameObject mainMenu;
    public GameObject Settings;
    public Slider loadingSlider;
    private void Awake()
    {
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f; // CRITICAL
        StartCoroutine(Loading());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame() 
    {
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
