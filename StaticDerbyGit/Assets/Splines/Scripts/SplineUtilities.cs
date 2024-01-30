using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public static class SplineUtilities
{
    public static float GetNearestPoint(SplineContainer container, Vector3 position)
    {
        SplineUtility.GetNearestPoint(container.Spline, position, out var pos, out var t);
        return t;
    }

    public static void ReverseSpline(SplineContainer container)
    {
        var spline = container.Spline;
        container.Spline = new Spline(spline.Reverse());
        container.Spline.SetTangentMode(TangentMode.AutoSmooth);
    }

}
