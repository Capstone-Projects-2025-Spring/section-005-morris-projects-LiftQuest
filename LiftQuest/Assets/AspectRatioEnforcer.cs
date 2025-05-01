using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour
{
    public float targetAspect = 4f / 3f; // Set your target aspect ratio (4:3 in this case)
    public Camera mainCamera;

    void Start()
    {
        // Check if the camera is set
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Set the resolution based on the target aspect ratio
        UpdateAspectRatio();
    }

    void UpdateAspectRatio()
    {
        // Get the screen's current aspect ratio
        float currentAspect = (float)Screen.width / (float)Screen.height;

        // Compare current and target aspect ratios
        float scaleHeight = currentAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // If the screen is too narrow, adjust the camera's viewport to add black bars
            mainCamera.rect = new Rect(0f, (1f - scaleHeight) / 2f, 1f, scaleHeight);
        }
        else
        {
            // If the screen is too wide, adjust the camera's viewport to add black bars horizontally
            float scaleWidth = 1.0f / scaleHeight;
            mainCamera.rect = new Rect((1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
        }
    }
}
