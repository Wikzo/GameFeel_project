using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RotateAroundFixedAngle1 : MonoBehaviour
{
    public Vector3 center;
    public GameObject centerObject;

    public Vector3 axis;
    public float angularVelocity;
    private Vector3 r0;
    private float t0;

    void Start()
    {
        r0 = transform.position;
        t0 = Time.fixedTime;

        if (centerObject != null)
            center = centerObject.transform.position;
    }

    void FixedUpdate()
    {
        float theta = angularVelocity*
                      (Time.fixedTime - t0);

        Vector3 w = axis.normalized*
                    angularVelocity;

        Vector3 u = Vector3.Dot(r0 - center, w)/
                    w.sqrMagnitude*w;

        Vector3 y = Vector3.Cross(w, r0 - center);

        y = y.normalized*(r0 - (center + u)).magnitude;

        transform.position = center + u +
                             Mathf.Cos(theta)*(r0 - (center + u)) +
                             Mathf.Sin(theta)*y;
    }
}

