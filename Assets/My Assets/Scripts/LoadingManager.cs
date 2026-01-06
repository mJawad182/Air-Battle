using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingPanel; // Your loading panel
    public Slider progressBar;      // Slider to show loading progress
    public float minLoadTime = 0.5f; // Minimum display time

    public static int targetBuildIndex = 1; // Scene to load (MainMenu default)

    private void Start()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        if (progressBar != null)
            progressBar.value = 0f;

        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetBuildIndex);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            // Update progress bar (0 - 1)
            if (progressBar != null)
                progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);

            // When loaded and minimum display time passed
            if (operation.progress >= 0.9f && timer >= minLoadTime)
            {
                if (progressBar != null)
                    progressBar.value = 1f;

                loadingPanel.SetActive(false);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
