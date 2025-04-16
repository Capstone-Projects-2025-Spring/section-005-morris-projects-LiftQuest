//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    
    public void GoToLogIn()
    {
        SceneManager.LoadScene("LogIn");
    }

    public void GoToRegister()
    {
        SceneManager.LoadScene("Register");
    }

    public void GoToProfileTest()
    {
        SceneManager.LoadScene("ProfileTest");
    }


}
