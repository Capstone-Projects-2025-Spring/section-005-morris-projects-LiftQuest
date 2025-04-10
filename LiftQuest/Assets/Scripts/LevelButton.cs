using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Sprite lockSprite;

    private int level = 0;

    private Button button;

    public Image image;
    
    void OnEnable()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }
    
    public void Setup(int level, bool isUnlock)
    {
        this.level = level;

        if(isUnlock)
        {
            image.sprite = null;
            button.enabled = true;
 
        }else
        {
            image.sprite = lockSprite;
            button.enabled = false;

        }

    }
    
    public void OnClick()
    {
        Debug.Log(button.name + "Button Clicked.");

    }
}
