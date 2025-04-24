using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardScreen;

    public void Next(){
        leaderboardScreen.SetActive(true);
    }

    public void LevelSelect(){
        SceneManager.LoadScene("LevelSelection");
    }

    public void Quit(){
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry(){
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
