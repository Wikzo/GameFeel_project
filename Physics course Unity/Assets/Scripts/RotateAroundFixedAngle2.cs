using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RotateAroundFixedAngle2 : MonoBehaviour
{
    public Vector3 center;
    public GameObject centerObject;

    public Vector3 axis;
    public float angularVelocity;

    void Start()
    {
        if (centerObject != null)
            center = centerObject.transform.position;
    }

    void FixedUpdate()
    {
        // this ALSO rotates the object around itself

        var deltaTheta = angularVelocity * Time.fixedDeltaTime;
        transform.RotateAround(center, axis, (float)(deltaTheta * 180.0 / Mathf.PI));
    }
}

