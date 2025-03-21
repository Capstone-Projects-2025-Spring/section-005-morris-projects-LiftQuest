using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        //there is no game

    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Player has Quit the game");
    }
}
