using UnityEngine;
using System.Collections;

public class SetResolution : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    // Use this for initialization
    private void Awake()
    {
        PlayerPrefs.SetInt("Screenmanager Resolution Width", 960);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", 600);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);

        Screen.SetResolution(960, 600, false);

        //DontDestroyOnLoad(gameObject);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Screenmanager Resolution Width", 960);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", 600);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }*/

#endif
}