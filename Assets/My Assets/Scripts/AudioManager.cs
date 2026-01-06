using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Audio Clips (Optional)")]
    public AudioClip[] clips; // assign clips in inspector

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        

        // Ensure there is an AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }


        ApplySfxState();



    }
    public void ApplySfxState()
    {
        // Only apply to SFX objects
        //if (!audioSource.gameObject.CompareTag("SFX"))
        //    return;

        int sfxState = PlayerPrefs.GetInt("SfxOn"); // default = ON

        // 0 = ON → mute = false
        // 1 = OFF → mute = true
        audioSource.mute = (sfxState != 1);
    }


    /// <summary>
    /// Play a specific audio clip
    /// </summary>
    /// <param name="clip">AudioClip to play</param>
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Play a clip from the pre-assigned array by index
    /// </summary>
    /// <param name="index">Index of the clip in the clips array</param>
    public void PlaySound(int index)
    {
        if (clips != null && index >= 0 && index < clips.Length)
        {
            PlaySound(clips[index]);
        }
    }
}
