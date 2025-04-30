using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelSelectionPlayModeTests
{
    [UnityTest]
        public IEnumerator VerifySceneChange()
        {
            //set up scene to test
            SceneManager.LoadScene("LevelSelection");
            yield return new WaitForSeconds(2f);

            string sceneName = SceneManager.GetActiveScene().name;
            Assert.That(sceneName, Is.EqualTo("LevelSelection"));
            yield return new WaitForSeconds(2f);

            Button level1Button = GameObject.Find("Level 1").GetComponent<Button>();
            level1Button.onClick.Invoke();
            yield return new WaitForSeconds(2f);

            //get new scene name
            string sceneName2 = SceneManager.GetActiveScene().name;
            yield return new WaitForSeconds(2f);

            Assert.That(sceneName2, Is.EqualTo("Level 1"));
            yield return new WaitForSeconds(2f);
        }

        [UnityTest]
        public IEnumerator VerifySceneMusic()
        {
            //load scene
            SceneManager.LoadScene("MainMenu");
            yield return new WaitForSeconds(2f);

            AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
            string songName = bgm.clip.name;
            yield return new WaitForSeconds(2f);

            Assert.That(songName, Is.EqualTo("mainMenu"));
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("LevelSelection");
            yield return new WaitForSeconds(2f);

            songName = bgm.clip.name;
            yield return new WaitForSeconds(2f);
            
            Assert.That(songName, Is.EqualTo("stageSelection"));
            yield return new WaitForSeconds(2f);
        }
}
