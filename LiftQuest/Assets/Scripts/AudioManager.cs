using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("-----------Audio Source-----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource buttonSFX;
    [SerializeField] AudioSource SFXSource;

    [Header("-----------Audio Clips-----------")]
    public AudioClip mainMenuMusic;
    public AudioClip levelSelectMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;
    public AudioClip level4Music;
    public AudioClip level5Music;
    public AudioClip buttonClick;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //different music in different scenes
        switch (scene.name)
        {
            case "LevelSelection":
                musicSource.Stop();
                musicSource.clip = levelSelectMusic;
                musicSource.Play();
                break;
            case "MainMenu":
                musicSource.Stop();
                musicSource.clip = mainMenuMusic;
                musicSource.Play();
                break;
            case "Level 1":
                musicSource.Stop();
                musicSource.clip = level1Music;
                musicSource.Play();
                break;
            case "Level 2":
                musicSource.Stop();
                musicSource.clip = level2Music;
                musicSource.Play();
                break;
            case "Level 3":
                musicSource.Stop();
                musicSource.clip = level3Music;
                musicSource.Play();
                break;
            case "Level 4":
                musicSource.Stop();
                musicSource.clip = level4Music;
                musicSource.Play();
                break;
            case "Level 5":
                musicSource.Stop();
                musicSource.clip = level5Music;
                musicSource.Play();
                break;
            default:
                Debug.Log("Login/SignUp will continue main menu scene music");
                break;
        }

        //only switch it scene change
        //if (source.clip != musicSource.clip)
        //{
            //musicSource.enabled = false;
            //musicSource.clip = source.clip;
            //musicSource.enabled = true;
        //}

        
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the event to prevent memory leaks
    }

    //private void Start()
    //{
        //musicSource.clip = background;
        //musicSource.Play();

    //}

    public void playButtonSound()
    {
        buttonSFX.clip = buttonClick;
        buttonSFX.Play();
    }

    public void playSFX(AudioClip clip)
    {
        buttonSFX.PlayOneShot(clip);

    }



}
