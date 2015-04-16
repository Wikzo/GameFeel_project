using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionTransition : MonoBehaviour
{
    public GameObject BackgroundPlane;
    public List<GameObject> ObjectsToMove;
    public Button KeepPlayingButton;
    public Text KeepPlayingButtonText;

    public Vector3 MoveToOffset;
    public float TransitionTime = 1f;
    public float TransitionTimeIncrement = 0.1f;
    public iTween.EaseType EasingTypeIn;
    public iTween.EaseType EasingTypeOut;

    private List<Vector3> _objectsDefaultPos;
    private float _backgroundPlaneStartPosY;
    private int _sequenceNumber = 0;
    
    private void Start()
    {
        _objectsDefaultPos = new List<Vector3>(ObjectsToMove.Count);
        for (int index = 0; index < ObjectsToMove.Count; index++)
            _objectsDefaultPos.Add(ObjectsToMove[index].transform.position);


        _backgroundPlaneStartPosY = BackgroundPlane.transform.position.y;

        NextTransition(0);

        _hideQuestions = false;
        _showHideButton = true;
        KeepPlayingButtonText.text = "Resume Playing";



    }

    int numberOfElements = 1;
    int numberOfElementsSummed = 0;

    private bool _hideQuestions;
    private bool _showHideButton;

    public void CompletedHideTransition()
    {
        _showHideButton = true;

        if (_hideQuestions)
        {
            KeepPlayingButtonText.text = "Resume Questions";
            Demographics.Instance.MyGameState = GameState.Playing;

        }
        else
        {
            KeepPlayingButtonText.text = "Resume Playing";
            Demographics.Instance.MyGameState = GameState.MidQuestionnaire;
            //_keepPlayingButtonOutline.StopColorAnimation();
        }

        KeepPlayingButton.interactable = true;

    }

    public void ToggleHideQuestions()
    {
        _hideQuestions = !_hideQuestions;

        float endPos = 0;

        // 617 = height

        if (_hideQuestions)
        {
            endPos = _backgroundPlaneStartPosY + 620f;
            KeepPlayingButtonText.text = "Resume Questions";
            Demographics.Instance.MyGameState = GameState.Playing;

        }
        else
        {
            endPos = _backgroundPlaneStartPosY;
            KeepPlayingButtonText.text = "Resume Playing";
            Demographics.Instance.MyGameState = GameState.MidQuestionnaire;

        }

        Hashtable t = new Hashtable();
        t.Add("y", endPos);
        t.Add("time", TransitionTime);
        t.Add("delay", 0.1f);
        t.Add("oncomplete", "CompletedHideTransition");
        t.Add("oncompletetarget", this.gameObject);

        iTween.MoveTo(BackgroundPlane, t);
        KeepPlayingButton.interactable = false;


    }

    public void NextTransition(int sequenceNumber)
    {
        KeepPlayingButton.interactable = false;

        _sequenceNumber = sequenceNumber;

        int offset = 0;

        switch (sequenceNumber)
        {
            case 1: // 0-2
                numberOfElements = 3;
                break;
            case 2: // 3-9
                numberOfElements = 7;
                break;
            case 3: // 10-15
                numberOfElements = 6;
                break;

            default:
                numberOfElements = 0;
                break;

        }

        for (int index = 0; index < ObjectsToMove.Count; index++)
        {
            if (sequenceNumber > 3)
                ObjectsToMove[index].transform.position = _objectsDefaultPos[index];


            //if (offset > numberOfElements)
            //    offset = numberOfElements;

            Vector3 startPos = ObjectsToMove[index].transform.position;
            Hashtable ht = new Hashtable();

            if (sequenceNumber == 0)
            {
                if (index < 2)
                {
                    offset++;
                    ht.Add("easetype", EasingTypeOut);
                }

            }

            if (sequenceNumber == 1)
            {
                if (index < 2)
                    offset++;
                ht.Add("easetype", EasingTypeIn);
            }
            else if (sequenceNumber == 2)
            {
                if (index > 2 && index < 10)
                    offset++;

                ht.Add("easetype", EasingTypeIn);
            }
            else if (sequenceNumber == 3)
            {
                if (index > 10)
                {
                    offset++;

                    ht.Add("easetype", EasingTypeIn);
                }
                //else
                    //ht.Add("easetype", EasingTypeOut);

            }

            ht.Add("x", startPos.x - MoveToOffset.x);
            ht.Add("time", TransitionTime + offset * TransitionTimeIncrement);
            //ht.Add("looptype", iTween.LoopType.pingPong);
            ht.Add("delay", 0.1f);
            ht.Add("oncomplete", "TransitionComplete");

            iTween.MoveTo(ObjectsToMove[index], ht);
        }
    }

    public void TransitionComplete()
    {
        /*Debug.Log("turninig off: " + numberOfElements);
        for (int i = 0; i < numberOfElements; i++)
        {
            ObjectsToMove[i].SetActive(false);
        }*/

        KeepPlayingButton.interactable = true;


        if (_sequenceNumber >= 3)
            StartCoroutine(WaitAndThenGoToNewRound());
    }

    IEnumerator WaitAndThenGoToNewRound()
    {
        yield return new WaitForSeconds(TransitionTime + 6*TransitionTimeIncrement);

        StateManager.Instance.ContinueToNextRound();

    }

}
