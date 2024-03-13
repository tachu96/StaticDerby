using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ReleaseFromSplineObstacle : MonoBehaviour
{
    public GameObject ActivatedObjectToCheck;

    private bool dead=false;

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
                        if (!dead) {
                            // Game object is not active in the scene, so we kick the player
                            component.ReleaseFromSpline();
                        }

                    }
                    //we dont do anything if this is not the case
                }

            }
        }
    }

    private void Update()
    {
        if (dead) { return; }

        if (ActivatedObjectToCheck != null)
        {
            //if the object is active, we kill the robot
            if(ActivatedObjectToCheck.activeInHierarchy)
            {
                //safeguard just in case lmao
                if (!dead) { 
                    //lets kill the robot
                    dead=true;

                    SplineAnimate splineAnimate = GetComponent<SplineAnimate>();

                    // If there is a SplineAnimate component, remove it
                    if (splineAnimate != null)
                    {
                        Destroy(splineAnimate);
                    }

                    // Check if the GameObject already has a Rigidbody component
                    Rigidbody rb = GetComponent<Rigidbody>();

                    // If there is no Rigidbody component, add one
                    if (rb == null)
                    {
                        rb = gameObject.AddComponent<Rigidbody>();
                    }
                }
            }
        }
    }
}