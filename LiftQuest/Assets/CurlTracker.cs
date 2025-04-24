using UnityEngine;

public class CurlTracker : MonoBehaviour
{
    private bool reachedTop = false;
    private int repCount = 0;

     void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("TopZone"))
        {
            reachedTop = true;
            Debug.Log("Top reached");
        }
        else if (other.gameObject.CompareTag("BottomZone") && reachedTop)
        {
            repCount++;
            reachedTop = false;
            Debug.Log("Rep Complete! Total Reps: " + repCount);
        }
    }
}
