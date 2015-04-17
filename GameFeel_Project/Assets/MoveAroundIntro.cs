using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveAroundIntro : MonoBehaviour
{
    public GameObject NextExampleButtonObject;
    public Text NextExampleButtonText;
    public Text ExampleText;
    public Text ExamplesCompletedText;

    public float MoveLeftTimer;
    public float MoveRightTimer;
    public int JumpTimes;

    private Player _myPlayer;
    private int index;
    private bool _hasUpdated;

    // Use this for initialization
    private void Start()
    {
        NextExampleButtonObject.SetActive(false);

        _myPlayer = GetComponent<Player>();

        _myPlayer.MyTweakableParameters.AttackTime = TweakableParameters.AttackAndReleaseTimes[0].x;
        _myPlayer.MyTweakableParameters.ReleaseTime = TweakableParameters.AttackAndReleaseTimes[0].x;

        ExampleText.text = "Example 1";
        index = 0;
        _hasUpdated = false;

        ExamplesCompletedText.text = "";

        Demographics.Instance.MyGameState = GameState.Playing;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            MoveLeftTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
            MoveRightTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            JumpTimes++;

        if (MoveLeftTimer > 2f && MoveRightTimer > 2f && JumpTimes >= 2 && !_hasUpdated)
        {
            _hasUpdated = true;

            NextExampleButtonObject.SetActive(true);

            if (index > 0)
            {
                ExamplesCompletedText.text = "Examples completed. Click button above to try the actual game.";
                NextExampleButtonText.text = "Click To Start Game";
            }
            else
            {
                ExamplesCompletedText.text = "Click button above to try next example.";                
            }

            index++;
        }
    }

    public void ShowNextExample()
    {
        _hasUpdated = false;

        if (index == 1)
        {
            _myPlayer.MyTweakableParameters.AttackTime = TweakableParameters.AttackAndReleaseTimes[1].y;
            _myPlayer.MyTweakableParameters.ReleaseTime = TweakableParameters.AttackAndReleaseTimes[1].y;

            ExampleText.text = "Example 2";
            ExamplesCompletedText.text = "";


            MoveRightTimer = 0;
            MoveLeftTimer = 0;
            JumpTimes = 0;

            NextExampleButtonObject.SetActive(false);

        }
        else
        {
            Demographics.Instance.PostLatinSquare();
            Demographics.Instance.MyGameState = GameState.Playing;

            Application.LoadLevel("1");
        }



    }
}
