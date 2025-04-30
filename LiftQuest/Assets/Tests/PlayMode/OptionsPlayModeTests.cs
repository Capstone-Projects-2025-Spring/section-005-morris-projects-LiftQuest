using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OptionsPlayModeTests
{
    [UnityTest]
        public IEnumerator modifyVolumeSlidersPasses()
        {
            //yield return new WaitForSeconds(2f);
            //load main menu scene
            SceneManager.LoadScene("MainMenu");
            yield return new WaitForSeconds(2f);

            //find the options button and invoke
            Button optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
            optionsButton.onClick.Invoke();
            yield return new WaitForSeconds(2f);

            Slider volumeSlider = GameObject.Find("VolumeSlide").GetComponent<Slider>();
            float volume = volumeSlider.value;

            //turn volume down
            volumeSlider.value = 0.001f;
            float value1 = volumeSlider.value;
            yield return new WaitForSeconds(2f);

            //turn the volume up
            volumeSlider.value = 1f;
            float value2 = volumeSlider.value;
            yield return new WaitForSeconds(2f);

            Assert.AreNotEqual(value1,value2);

            yield return null;

        }

}
