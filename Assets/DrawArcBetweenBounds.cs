using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class DrawArcBetweenBounds : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    int NumSegments = 10;

    int curveCount;
    public Transform[] controlPoints;

    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        controlPoints = new Transform[transform.childCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (controlPoints.Length != transform.childCount)
        {
            controlPoints = new Transform[transform.childCount];
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            controlPoints[i] = transform.GetChild(i).transform;
        }

        curveCount = transform.childCount / 3;

        if (lr == null)
        {
            lr = GetComponent<LineRenderer>();
        }

        DrawBezierCurve();

    }

    //Draw a Bezier curve between two points
    private void DrawBezierCurve()
    {
        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= NumSegments; i++)
            {
                float t = i / (float)NumSegments;
                int nodeIndex = j * 3;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].gameObject.GetComponent<Renderer>().bounds.center, controlPoints[nodeIndex + 1].gameObject.GetComponent<Renderer>().bounds.center, controlPoints[nodeIndex + 2].gameObject.GetComponent<Renderer>().bounds.center, controlPoints[nodeIndex + 3].gameObject.GetComponent<Renderer>().bounds.center);

                lr.positionCount = (j * NumSegments) + i + 1;
                lr.SetPosition((j * NumSegments) + (i - 1) + 1, pixel);
                lr.SetPosition(0, transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.center);
            }

        }
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
