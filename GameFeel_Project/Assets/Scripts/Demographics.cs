using UnityEngine;
using System.Collections;

public enum GameState
{
    IntroDemographics,
    MidQuestionnaire,
    Playing,
    ShowPostQuestionnaire
}

public class Demographics : MonoBehaviour
{
    public GameState MyGameState;

    public string YourName = "";
    public string Gender = "";
    public string Age = "";
    public string Country = "";
    public string Id = "";
    public string ExperienceGames = "";
    public string ExperiencePlatformers = "";


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
        Id = SystemInfo.deviceUniqueIdentifier;

        MyGameState = GameState.IntroDemographics;
    }

    public string ToStringDatabaseFormat()
    {
        return
            string.Format(
                "&Gender={0}&Age={1}&Country={2}&GUID={3}&ExperienceGames={4}&ExperiencePlatformers={5}",
                Gender,
                Age,
                Country,
                Id,
                ExperienceGames,
                ExperiencePlatformers);
    }

}