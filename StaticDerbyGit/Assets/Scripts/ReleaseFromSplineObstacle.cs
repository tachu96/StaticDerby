using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseFromSplineObstacle : MonoBehaviour
{
    public GameObject ActivatedObjectToCheck;

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
                if (ActivatedObjectToCheck == null)
                {
                    // Release from spline
                    component.ReleaseFromSpline();
                }
                else {
                    if (!ActivatedObjectToCheck.activeInHierarchy) {
                        // Game object is not active in the scene, so we kick the player
                        component.ReleaseFromSpline();
                    }
                    //we dont do anything if this is not the case
                }

            }
        }
    }
}