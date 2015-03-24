using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LerpStuff : MonoBehaviour
{
    Vector3 startScale;

    public float amplitude = 10f;
    public float period = 5f;

    protected void Start()
    {
        startScale = transform.localScale;
    }

    protected void Update() {
        float theta = Time.timeSinceLevelLoad / period;
        float distance = amplitude * Mathf.Sin(theta);
        float abs = Mathf.Abs(distance);
        transform.localScale = new Vector3(0, abs, 0) + startScale;
    }
}