using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public GameObject tutorialScreen;
    public GameObject optionsScreen;
    public AudioMixer _am;
    public void SetVolume(float volume){
        _am.SetFloat("Volume", volume);
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


}
