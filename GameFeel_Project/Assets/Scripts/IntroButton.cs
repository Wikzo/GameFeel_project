using UnityEngine;
using System.Collections;

public class IntroButton : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - (Screen.height * 0.15f), 300, 30), "Click To Continue"))
            Application.LoadLevel("Demographics");
    }

}