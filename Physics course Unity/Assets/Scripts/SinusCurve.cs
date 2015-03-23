using System.Collections.Generic;
using UnityEngine;
using System.Collections;

// https://www.youtube.com/watch?v=yAHl_kpqr-k&index=21&list=WL

public class SinusCurve : MonoBehaviour
{
    public float Precision = 0.1f;
    public float StartPosX = 0f;
    public float StartPosY = 0f;
    private List<GameObject> Spheres;

    public float AddedSpeed = 1;

    public bool UseColor = true;

    public Gradient Gradient;
    public Material White;

    // Use this for initialization
    private void Start()
    {
        PlaceSpheres();
    }

    void OnGUI()
    {
        //GUI.backgroundColor = Color.red;
        AddedSpeed = GUI.HorizontalSlider(new Rect(10,10,100,100), AddedSpeed, 0f, 2f);
        UseColor = GUI.Toggle(new Rect(120, 10, 100, 30), UseColor, "Use Color");
        GUI.Label(new Rect(0,0,20,20), AddedSpeed.ToString());
    }

    // Update is called once per frame
    private void Update()
    {

    }

    void PlaceSpheres()
    {
        Spheres = new List<GameObject>();

        for (float angle = StartPosX; angle < Mathf.PI * 8; angle += Precision)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            sphere.transform.position = new Vector3(angle, Mathf.Sin(angle) + StartPosY,0);
            sphere.transform.renderer.material = White;
            sphere.transform.renderer.material.color = Gradient.Evaluate(angle % 1f);

            var bounce = sphere.AddComponent<BouncingSinus>();
            bounce.BounceSpeed = angle;

        }
    }
}
