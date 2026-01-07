using UnityEngine;

public class BackgroundPauser : MonoBehaviour
{
    [Header("References (Auto-found if not assigned)")]
    public DirectMoving[] backgroundMovers;
    
    private bool isPaused = false;
    private bool wasEnemyActive = false;
    
    // Store original speeds to restore later
    private float[] originalBackgroundSpeeds;

    private void Start()
    {
        // Auto-find background movers if not assigned
        if (backgroundMovers == null || backgroundMovers.Length == 0)
        { 
            backgroundMovers = FindObjectsByType<DirectMoving>(FindObjectsSortMode.None);
        }
        
        // Store original background speeds
        originalBackgroundSpeeds = new float[backgroundMovers.Length];
        for (int i = 0; i < backgroundMovers.Length; i++)
        {
            if (backgroundMovers[i] != null)
            {
                originalBackgroundSpeeds[i] = backgroundMovers[i].speed;
            }
        }
    }

    private void Update()
    {
        bool isFinalEnemyActive = IsFinalEnemyPresent();
        bool isLowFinalEnemyActive = IsLowFinalEnemyPresent();
        bool isAnyBossActive = isFinalEnemyActive || isLowFinalEnemyActive;
        
        // Boss just appeared - pause background
        if (isAnyBossActive && !wasEnemyActive)
        {
            PauseBackground();
            wasEnemyActive = true;
        }
        // Boss just disappeared - resume background
        else if (!isAnyBossActive && wasEnemyActive)
        {
            ResumeBackground();
            wasEnemyActive = false;
        }
    }
    
    private bool IsFinalEnemyPresent()
    {
        return GameObject.Find("Final Enemy(Clone)") != null;
    }
    
    private bool IsLowFinalEnemyPresent()
    {
        // Check for either LowFinalEnemyMove or LowEnemySpawnerMove2
        return FindFirstObjectByType<LowFinalEnemyMove>() != null || 
               FindFirstObjectByType<LowEnemySpawnerMove2>() != null;
    }
    
    private void PauseBackground()
    {
        if (isPaused) return;
        isPaused = true;
        
        // Pause background movement only
        for (int i = 0; i < backgroundMovers.Length; i++)
        {
            if (backgroundMovers[i] != null && backgroundMovers[i].CompareTag("Background"))
            {
                backgroundMovers[i].speed = 0f;
            }
        }
    }
    
    private void ResumeBackground()
    {
        if (!isPaused) return;
        isPaused = false;
        
        // Resume background movement
        for (int i = 0; i < backgroundMovers.Length; i++)
        {
            if (backgroundMovers[i] != null && backgroundMovers[i].CompareTag("Background"))
            {
                backgroundMovers[i].speed = originalBackgroundSpeeds[i];
            }
        }
    }
}
