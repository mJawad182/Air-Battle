using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance;

    private const string MUSIC_KEY = "MusicOn";
    private const string SFX_KEY = "SfxOn";

    [Header("UI")]
    public Button musicButton;
    public Button sfxButton;
    public Image musicButtonImage;
    public Image sfxButtonImage;

    [Header("Sprites")]
    public Sprite greenSprite;
    public Sprite redSprite;

    private bool musicOn;
    private bool sfxOn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicOn = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        sfxOn = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;

        ApplyMusicState();
        ApplySfxState();
        UpdateUI();
    }

    // ---------------- MUSIC ----------------
    public void ToggleMusic()
    {
        musicOn = !musicOn;
        PlayerPrefs.SetInt(MUSIC_KEY, musicOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplyMusicState();
        UpdateUI();
    }

    private void ApplyMusicState()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetMusic(musicOn);
    }

    // ---------------- SFX ----------------
    public void ToggleSfx()
    {
        sfxOn = !sfxOn;
        PlayerPrefs.SetInt(SFX_KEY, sfxOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySfxState();
        UpdateUI();
    }

    public void ApplySfxState()
    {
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allSources)
        {
            if (source.CompareTag("SFX"))
                source.mute = !sfxOn;
        }
    }

    private void UpdateUI()
    {
        musicButtonImage.sprite = musicOn ? greenSprite : redSprite;
        sfxButtonImage.sprite = sfxOn ? greenSprite : redSprite;
    }

    public bool IsSfxOn() => sfxOn;
}
