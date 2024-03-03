using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
[RequireComponent(typeof(SplineExtrude))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class SplineRideable : MonoBehaviour
{
    [field: SerializeField] public SplineAnimate SplineAnimate { get; private set; }
    [field: SerializeField] public SplineContainer SplineContainer { get; private set; }

    public void RideSpline(GameObject rider)
    {

    }

}
