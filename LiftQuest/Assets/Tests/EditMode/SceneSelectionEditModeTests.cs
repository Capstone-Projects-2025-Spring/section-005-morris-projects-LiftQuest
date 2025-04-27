using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionEditModeTests
{
    [SetUp]
    public void Setup()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/LevelSelection.unity");
    }

    //Tests that the button exists
    [Test]
    public void LevelButtonExistsPasses()
    {
        var gameObject = GameObject.Find("Level 1");

        Assert.That(gameObject, Is.Not.Null);
    }

    [Test]
    public void ButtonIsInteractable()
    {

        GameObject buttonObject = new GameObject();
        Button buttonComponent = buttonObject.AddComponent<Button>();

        buttonComponent.interactable = true;

        Assert.IsTrue(buttonComponent.interactable, "The button should be interactable by default");


    }

    [Test]
    public void ButtonIsNotInteractable()
    {

        GameObject buttonObject = new GameObject();
        Button buttonComponent = buttonObject.AddComponent<Button>();

        buttonComponent.interactable = false;

        Assert.IsFalse(buttonComponent.interactable, "The button should not be interactable before level unlocks");


    }



}
