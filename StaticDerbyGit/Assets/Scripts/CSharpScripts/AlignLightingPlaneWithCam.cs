using UnityEngine;

public class FaceCameraHorizontally : MonoBehaviour
{
    [SerializeField] private Transform PlayerCam;

    void Update()
    {
        if (PlayerCam != null)
        {
            // Get the camera's forward direction without the vertical component
            Vector3 cameraForward = PlayerCam.forward;
            cameraForward.y = 0f;

            transform.forward = cameraForward;
        }
        else
        {
            Debug.LogError("Assign PlayerCam Variable to the "+gameObject.name+ " on the air attack!!");
        }
    }
}