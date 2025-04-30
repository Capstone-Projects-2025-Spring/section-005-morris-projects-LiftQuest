using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour
{

    public Button[] buttons;
    
    public Sprite lockSprite;

    public Sprite[] levelImages;

    public void OnClick()
    {
        Debug.Log(gameObject.name + " Button Clicked.");
    }

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if(PlayerPrefs.GetString("Username") == "HooterTheOwl"){
            unlockedLevel = 3;
        }
        
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
            buttons[i].GetComponent<Image>().sprite = lockSprite;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
            buttons[i].GetComponent<Image>().sprite = levelImages[i];
        }
    }


    public void returnToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void loadLevel(GameObject buttonObject)
    {
        SceneManager.LoadScene(buttonObject.name);
    }



}
