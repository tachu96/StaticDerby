using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SplineReverse : MonoBehaviour
{
    [SerializeField] private SplineContainer container;
    [SerializeField] private bool reverse;

    private void OnValidate()
    {
        if (reverse)
        {
            ReverseSpline();
        }
    }

    public void ReverseSpline()
    {
        var spline = container.Spline;
        container.Spline = new Spline(spline.Reverse());
        container.Spline.SetTangentMode(TangentMode.AutoSmooth);
        reverse = false;
    }

}
