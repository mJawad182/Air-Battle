using UnityEngine;

public class FinalEnemyBullet : MonoBehaviour
{
    [Header("Scale Settings")]
    [Tooltip("Starting scale of the bullet")]
    public Vector3 startScale = new Vector3(0.7f, 0.7f, 0.7f);
    
    [Tooltip("Target scale of the bullet")]
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);
    
    [Tooltip("Time in seconds to reach target scale")]
    public float scaleDuration = 2f;
    
    private float elapsedTime = 0f;
    
    private void Start()
    {
        // Set initial scale
        transform.localScale = startScale;
    }
    
    private void Update()
    {
        if (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // Smoothly interpolate from start to target scale
            float t = Mathf.Clamp01(elapsedTime / scaleDuration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
        }
    }
}
