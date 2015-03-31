using UnityEngine;
using System.Collections;

public class Demographics : MonoBehaviour
{
    public string name = "";
    public string gender = "";
    public string age = "";
    public string country = "";
    public string id = "";
    public int experienceGames = 0;
    public int experiencePlatformers = 0;

    static readonly string[] scale = new string[] { "1\n(none)", "2", "3", "4", "5","6","7\n(a lot)" };

    private bool showDemographics = true;

    private static Demographics _instance;
    public static Demographics Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(Demographics)) as Demographics;

            return _instance;
        }
    }

    void OnApplicationQuit()
    {
        _instance = null; // release on exit
    }

    private void Awake()
    {
        // http://clearcutgames.net/home/?p=437
        // First we check if there are any other instances conflicting
        if (_instance != null && _instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        _instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        id = SystemInfo.deviceUniqueIdentifier;
    }

    void OnGUI()
    {
        if (!showDemographics)
            return;

        GUILayout.BeginArea(new Rect(0,0, Screen.width*0.7f, Screen.height));

        GUILayout.Label("Your name:");
        name = GUILayout.TextField(name);

        GUILayout.Label("Your gender:");
        gender = GUILayout.TextField(gender);

        GUILayout.Label("Your age:");
        age = GUILayout.TextField(age);

        GUILayout.Label("Your country:");
        country = GUILayout.TextField(country);

        GUILayout.Label("Your experience with playing videogames:");
        experienceGames = GUILayout.SelectionGrid(experienceGames, scale, scale.Length);

        GUILayout.Label("Your experience with 2D platformer games:");
        experiencePlatformers = GUILayout.SelectionGrid(experiencePlatformers, scale, scale.Length);

        if (GUILayout.Button("Click To Begin"))
        {
            showDemographics = false;
            Application.LoadLevel("1");
        }


        GUILayout.EndArea();
    }

    public string ToStringDatabaseFormat()
    {
        return
            string.Format(
                "&Gender={0}&Age={1}&Country={2}&GUID={3}&ExperienceGames={4}&ExperiencePlatformers={5}",
                gender,
                age,
                country,
                id,
                experienceGames,
                experiencePlatformers);


    }

}