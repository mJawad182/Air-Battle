using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script moves the attached object along the Y-axis with the defined speed
/// </summary>
public class DirectMoving : MonoBehaviour
{
    [Tooltip("Moving speed on Y axis in local space")]
    public float speed;
    private bool appPaused = false;
    private void Update()
    {
        if (appPaused)
            return;

        // Stop movement if app not focused or game is paused
        if (appPaused || GameUIManager.isGamePaused)
            return;
        float delta;

        if (gameObject.CompareTag("Background"))
        {
            delta = Mathf.Min(Time.unscaledDeltaTime, 0.05f);
            transform.Translate(Vector3.up * speed * delta, Space.Self);
        }
        else
            transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnApplicationPause(bool pause)
    {
        appPaused = pause;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // Explicitly restore state on resume
        appPaused = !hasFocus;
    }

}
