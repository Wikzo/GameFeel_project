using System;
using UnityEngine;
using System.Collections;

// http://wiki.unity3d.com/index.php?title=Server_Side_Highscores

public class HSController : MonoBehaviour
{
    private string secretKey = "hejsa123123"; // Edit this value and make sure it's the same as the one stored on the server
    string addScoreURL = "http://tunnelvisiongames.com/unityserver/addscore.php?"; //be sure to add a ? to your url
    string highscoreURL = "http://tunnelvisiongames.com/unityserver/display.php";



    void Start()
    {
        //StartCoroutine(GetScores());
    }
    string name = "Name";
    string scoreText = "5";
    int score = 0;

    private string loadedString = "loadedString";

    void OnGUI()
    {
        name = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2 - 50, 200, 20), name);
        scoreText = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2, 200, 20), scoreText);

        score = Convert.ToInt32(scoreText);

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 50, 100, 30), "Send data"))
        {
            StartCoroutine(PostScores(name, score));
        }

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 100, 100, 30), "Load data"))
        {
            StartCoroutine(GetScores());

        }

        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2, 500, 300), loadedString);
    }

    // remember to use StartCoroutine when calling this function!
    IEnumerator PostScores(string name, int score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = MD5Test.Md5Sum(name + score + secretKey);
        Debug.Log(hash);

        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
        Debug.Log(post_url);

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        Debug.Log(hs_post.uploadProgress);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
    {
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            Debug.Log(hs_get.text);
            loadedString = hs_get.text;
        }


    }

}