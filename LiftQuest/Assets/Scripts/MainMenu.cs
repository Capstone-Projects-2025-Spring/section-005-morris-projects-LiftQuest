using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        //temporary Play to get to Level Selection menu
        //To be removed later
        SceneManager.LoadSceneAsync(1);


    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Player has Quit the game");
    }
}
