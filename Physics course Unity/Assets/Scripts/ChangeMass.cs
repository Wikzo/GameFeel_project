using UnityEngine;
using System.Collections;

public class ChangeMass : MonoBehaviour {

    private float massCube = 1;
    private float massSphere = 1;

    public Rigidbody Cube;
    public Rigidbody Sphere;

    void OnGUI()
    {
        GUI.Label(new Rect(10, Screen.height / 2-40, 100, 20), "Cube mass:");
        massCube = GUI.HorizontalSlider(new Rect(10, Screen.height / 2, 100, 20), massCube, 1, 3000);
        Cube.rigidbody.mass = massCube;

        GUI.Label(new Rect(10, Screen.height / 2 + 30, 100, 20), "Sphere mass:");
        massSphere = GUI.HorizontalSlider(new Rect(10, Screen.height / 2 + 70, 100, 20), massSphere, 1, 900);
        Sphere.rigidbody.mass = massSphere;

        if (GUI.Button(new Rect(10, Screen.height / 2+100, 100, 20), "Restart"))
            Application.LoadLevel("Buoyancy");
    }
}
