using System;
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

    public string Gender = "";
    public string Age = "";
    public string Country = "";
    public string Id = "";
    public string ExperienceGames = "";
    public string ExperiencePlatformers = "";
    public int LatinSquareSequence = 1;

    string getLatinSequenceURL = "http://tunnelvisiongames.com/g/displaylatin.php";



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

        StartCoroutine(GetLatestLatinSequence());

    }

    public string ToStringDatabaseFormat()
    {
        return
            string.Format(
                "&Gender={0}&Age={1}&Country={2}&ExperienceGames={3}&ExperiencePlatformers={4}",
                Gender,
                Age,
                Country,
                ExperienceGames,
                ExperiencePlatformers);
    }

    IEnumerator GetLatestLatinSequence()
    {
        WWW hs_get = new WWW(getLatinSequenceURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error getting the latin square sequence: " + hs_get.error);
        }
        else
        {
            //Debug.Log(hs_get.text);

            int latin = 1;
            if (Int32.TryParse(hs_get.text, out latin))
            {
                LatinSquareSequence = latin;

                LatinSquareSequence++;

                if (LatinSquareSequence > 4)
                    LatinSquareSequence = 1;

                //print("Succesfully loaded latin square sequence (+1): " + LatinSquareSequence);

            }
            else
                print("Unsuccesfully loaded latin square sequence!");
        }
    }

}