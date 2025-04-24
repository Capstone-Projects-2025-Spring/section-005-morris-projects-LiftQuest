using UnityEngine;

public class ButtonPress : MonoBehaviour
{

    [SerializeField] AudioSource buttonSFX;
    public AudioClip buttonClick;
    private void Awake()
    {
        //
    }
    public void playButtonSound()
    {
        buttonSFX.clip = buttonClick;
        buttonSFX.Play();
    }
}
