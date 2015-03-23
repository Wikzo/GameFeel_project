using UnityEngine;
using System.Collections;

// https://www.youtube.com/watch?v=-ayh6oEtjbA&index=20&list=WL

public class BouncingSinus : MonoBehaviour
{

    public float BounceSpeed = 2f;
    private Vector3 StartPos;
    private float angle = 0f;

    private SinusCurve sin;
    private Color StartColor;

    // Use this for initialization
    private void Start()
    {
        StartPos = transform.position;
        StartColor = transform.renderer.material.color;

        SinusCurve s = (SinusCurve)FindObjectOfType(typeof (SinusCurve));
        
        if (!s)
            Debug.LogWarning("Error, could not find sinus object");
        else
            sin = s;
    }

    private float added = 1;
    private void Update()
    {
        if (sin != null)
        {
            if (sin.UseColor)
                transform.renderer.material.color = StartColor;
            else
                transform.renderer.material.color = Color.white;

            added = sin.AddedSpeed;
        }

        Vector3 newPos = StartPos + new Vector3(0, Mathf.Sin(angle), 0);
        transform.position = newPos;


        angle += BounceSpeed * added * Time.deltaTime;
    }
}
