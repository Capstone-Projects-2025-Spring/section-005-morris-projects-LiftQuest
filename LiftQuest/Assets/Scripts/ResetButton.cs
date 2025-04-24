using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private Button button;
    private Receiver receiver;

    void Start()
    {
        button = GetComponent<Button>();
        receiver = FindObjectOfType<Receiver>();

        if (button != null && receiver != null)
        {
            button.onClick.AddListener(ResetCalibration);
        }
        else
        {
            if (button == null)
                Debug.LogError("Button not found!");
            if (receiver == null)
                Debug.LogError("Receiver not found!");
        }
    }

    void ResetCalibration()
    {
        if (receiver != null)
        {
            receiver.ResetCalibration();
        }
    }
}