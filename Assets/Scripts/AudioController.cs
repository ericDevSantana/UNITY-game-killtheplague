using UnityEngine;
using UnityEngine.UI;

public enum SoundEffects
{
    SPLASH,
    BUTTON
}
public class AudioController : MonoBehaviour
{
    public AudioClip soundTrack;
    public AudioClip splashSound;
    public AudioClip buttonSound;

    public AudioSource soundTrackSource;
    public static AudioSource effectSoundSource;

    public Slider musicSlider;
    public Slider effectSlider;

    public GameController gameController;
    public SaveController saveManager;

    public bool isMainMenu = true;

    private static AudioController instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        soundTrackSource.clip = soundTrack;

        saveManager = FindObjectOfType(typeof(SaveController)) as SaveController;
        gameController = FindObjectOfType(typeof(GameController)) as GameController;

        musicSlider.onValueChanged.AddListener(delegate { MusicVolumeChange(); });
        effectSlider.onValueChanged.AddListener(delegate { EffectVolumeChange(); });
        
        soundTrackSource.volume = saveManager.LoadMusicVolume();
        instance.GetComponent<AudioSource>().volume = saveManager.LoadEffectsVolume();

        musicSlider.value = saveManager.LoadMusicVolume();
        effectSlider.value = saveManager.LoadEffectsVolume();

    }

    void Update()
    {
        //ONLY PLAYS MUSIC WHEN THE GAME'S STATE IS IN MAIN MENU
        if (isMainMenu)
        {
            soundTrackSource.Play();
            isMainMenu = !isMainMenu;
        }

        if(gameController.GetCurrentGameState() != GameStates.MAINMENU &&
            gameController.GetCurrentGameState() != GameStates.SETTINGS && !isMainMenu)
        {
            isMainMenu = !isMainMenu;
        }
    }


    public static void PlaySound(SoundEffects effectSound)
    {
        switch (effectSound)
        {
            case SoundEffects.SPLASH:
                instance.GetComponent<AudioSource>().PlayOneShot(instance.splashSound);
            break;

            case SoundEffects.BUTTON:
                instance.GetComponent<AudioSource>().PlayOneShot(instance.buttonSound);
            break;
        }
    }

    public void MusicVolumeChange()
    {
        soundTrackSource.volume = musicSlider.value;
        saveManager.SaveMusicVolume(musicSlider.value);
    }

    public void EffectVolumeChange()
    {
        instance.GetComponent<AudioSource>().volume = effectSlider.value;
        saveManager.SaveEffectsVolume(effectSlider.value);
    }
}

