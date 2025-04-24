using UnityEngine;

public class BicepCurlDetector : MonoBehaviour
{
    private enum RepState { Rest, Curling }
    private RepState currentState = RepState.Rest;
    private int repCount = 0;
    
    [Header("Detection Settings")]
    public float debounceTime = 0.5f;
    
    private float lastRepTime = 0f;


    public bool UpdatePositions(bool isInRestPosition, bool isInCurlPosition)
    {
        bool repCompleted = false;
        
        if (Time.time - lastRepTime < debounceTime)
        {
            return false;
        }

        switch (currentState)
        {
            case RepState.Rest:
                if (isInCurlPosition && !isInRestPosition)
                {
                    currentState = RepState.Curling;
                    repCount++;
                    repCompleted = true;
                    lastRepTime = Time.time;
                    Debug.Log("Rep completed: " + repCount);
                }
                break;

            case RepState.Curling:
                if (isInRestPosition && !isInCurlPosition)
                {
                    currentState = RepState.Rest;
                    lastRepTime = Time.time;
                    Debug.Log("Back to rest position");
                }
                break;
        }

        return repCompleted;
    }

    public int GetRepCount()
    {
        return repCount;
    }
    
    public void ResetRepCount()
    {
        repCount = 0;
        currentState = RepState.Rest;
    }
}

