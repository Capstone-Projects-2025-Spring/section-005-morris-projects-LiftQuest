using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text statusText;
    public Text repCountText;
    public Text instructionText;
    public Text connectionStatusText;

    private Receiver sensorReceiver;

    void Start()
    {
        sensorReceiver = FindObjectOfType<Receiver>();
        
        if (sensorReceiver == null)
        {
            Debug.LogError("Receiver not found");
        }
        
        if (statusText == null)
        {
            Debug.LogError("Status not assigned!");
        }
        
        if (repCountText == null)
        {
            Debug.LogError("Rep Count Text not assigned!");
        }
        
        if (instructionText == null)
        {
            Debug.LogError("Instruction Text not assigned!");
        }
        else
        {
            instructionText.text = "Follow the steps, then perform the excercise";
        }
        
        if (connectionStatusText == null)
        {
            Debug.LogError("Connection Status Text is not assigned!");
        }
    }

    void Update()
    {
        if (statusText != null && sensorReceiver != null)
        {
            statusText.text = sensorReceiver.GetCalibrationStatus();
        }

        if (repCountText != null && sensorReceiver != null)
        {
            repCountText.text = $"Reps: {sensorReceiver.GetRepCount()}";
        }
        
        if (connectionStatusText != null && sensorReceiver != null)
        {
            connectionStatusText.text = sensorReceiver.IsConnected() ? 
                "Connected" : "Disconnected";
            
            connectionStatusText.color = sensorReceiver.IsConnected() ? 
                Color.green : Color.red;
        }
    }
}

