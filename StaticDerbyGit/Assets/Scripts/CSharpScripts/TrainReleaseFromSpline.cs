using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TrainReleaseFromSpline : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Get the component with the method you want to call
            SplineRider component = other.GetComponent<SplineRider>();

            // Check if the component exists
            if (component != null)
            {
                // Release from spline
                component.ReleaseFromSpline();

            }
        }
    }

}