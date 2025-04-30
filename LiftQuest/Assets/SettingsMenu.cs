using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject tutorialScreen;
    public GameObject optionsScreen;
    public AudioMixer _am;
    public Slider audioSlider;
    
    public void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            loadVolume();

        }
        else
        {
            SetVolume();

        }

    }

    public void SetVolume(){
        float volume = audioSlider.value;
        _am.SetFloat("Volume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume",volume);
    }

    public void SetFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }

    public void TutorialButton(){
        tutorialScreen.SetActive(true);
    }
    public void TutorialBackButton(){
        tutorialScreen.SetActive(false);
    }

    public void OptionsButton(){
        optionsScreen.SetActive(true);
    }
    public void OptionsBackButton(){
        optionsScreen.SetActive(false);
    }

    private void loadVolume()
    {
        audioSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetVolume();

    }


}
