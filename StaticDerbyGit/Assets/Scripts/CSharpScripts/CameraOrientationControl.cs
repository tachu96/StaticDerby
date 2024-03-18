using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrientationControl : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform player;

    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);

        viewDir.y = 0f;

        orientation.forward = viewDir.normalized;

        if (Mathf.Abs(orientation.localEulerAngles.x) > 0.01f || Mathf.Abs(orientation.localEulerAngles.z) > 0.01f)
        {
            Debug.Log("Orientation is Fucked Up" + orientation.localEulerAngles);
        }
    }
}
