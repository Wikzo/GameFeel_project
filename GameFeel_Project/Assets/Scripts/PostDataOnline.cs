using System;
using UnityEngine;
using System.Collections;

// http://wiki.unity3d.com/index.php?title=Server_Side_Highscores

public class PostDataOnline : MonoBehaviour
{
    private string secretKey = "hSrxsRtAupPMAt74mftNx8J7"; // Edit this value and make sure it's the same as the one stored on the server
    string addScoreURL = "http://tunnelvisiongames.com/gf/addfeeling.php?"; //be sure to add a ? to your url
    string highscoreURL = "http://tunnelvisiongames.com/gf/display.php";



    void Start()
    {
        //StartCoroutine(GetScores());
    }

    private string loadedString = "loadedString";

    /*void OnGUI()
    {
        name = GUI.TextField(new Rect(Screen.width/2, Screen.height/2-50, 200, 20), name);
        scoreText = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2, 200, 20), scoreText);

        score = Convert.ToInt32(scoreText);

        if (GUI.Button(new Rect(Screen.width/2, Screen.height/2+50, 100, 30), "Send data"))
        {
            StartCoroutine(PostScores(name, score));
        }

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 100, 100, 30), "Load data"))
        {
            StartCoroutine(GetScores());

        }

        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2, 500, 300), loadedString);
    }*/

    public void PostData(string name, string demographics, string rating, string parametersWithSeperators, string fps)
    {
        StartCoroutine(PostScores(name, demographics, rating, parametersWithSeperators, fps));
    }

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

    // remember to use StartCoroutine when calling this function!
    IEnumerator PostScores(string name, string demographics, string rating, string parametersWithSeperators, string fps)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = MD5Test.Md5Sum(name + secretKey);
        //Debug.Log(hash);

        //string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        string post_url = addScoreURL + "Name=" + WWW.EscapeURL(name) + demographics + rating + parametersWithSeperators + fps + "&hash=" + hash;

        string urlWithoutSpaces = post_url.Replace(" ", "+");

        //Debug.Log(urlWithoutSpaces);

        //Debug.Log(post_url);
        
        /*+ "&gravity=" + gravity
            + "&jumpPower=" + jumpPower
            + "&useAirFriction=" + useAirFriction.ToString()
            + "&airFrictionHorizontal=" + airFrictionHorizontal
            + "&terminalVelocity=" + terminalVelocity
            + "&ghostJumpTime=" + ghostJumpTime
            + "&minimumJumpHeight=" + minimumJumpHeight
            + "&releaseEarlyJumpVelocity=" + releaseEarlyJumpVelocity
            + "&apexGravityMultiplier=" + apexGravityMultiplier
            + "&useGroundFriction=" + useGroundFriction.ToString()
            + "&groundFrictionPercentage=" + groundFrictionPercentage
            + "&releaseTime=" + releaseTime
            + "&attackTime=" + attackTime
            + "&turnAroundBoostPercent=" + turnAroundBoostPercent
            + "&useCurveForHorizontalAttackVelocity=" + useCurveForHorizontalAttackVelocity.ToString()
            + "&useCurveForHorizontalReleaseVelocity=" + useCurveForHorizontalReleaseVelocity.ToString()
            + "&useAnimation=" + useAnimation.ToString()
            + "&animationMaxSpeed=" + animationMaxSpeed
            + "&hash=" + hash;*/

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(urlWithoutSpaces);
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