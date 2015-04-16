using UnityEngine;
using System.Collections;

public class IntroButton : MonoBehaviour
{
    public void LoadLevel(string level)
    {
        if (level == "1")
        {
            Demographics.Instance.PostLatinSquare();
            Demographics.Instance.MyGameState = GameState.Playing;
        }
        Application.LoadLevel(level);


    }

}