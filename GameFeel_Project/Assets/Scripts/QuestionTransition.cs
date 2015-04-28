using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionTransition : MonoBehaviour
{
    public GameObject BackgroundPlane;
    public RectTransform BackgroundPlaneRect;
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

    int numberOfElements = 1;


    private bool hideButtonLerp;
    public float hideButtoCurrentLerpTime;
    private float hideButtonLerpTime = 0.3f;//
    private Vector2 hideButtonStartPos, hideButtonEndPos;
    private bool hideButtonGoingUp;


    private void Awake()
    {
        _objectsDefaultPos = new List<Vector3>(ObjectsToMove.Count);
        for (int index = 0; index < ObjectsToMove.Count; index++)
            _objectsDefaultPos.Add(ObjectsToMove[index].transform.position);


        hideButtonLerp = false;
        nextQuestionLerp = false;
        KeepPlayingButton.interactable = false;
        KeepPlayingButtonText.text = "Resume Playing";
    }

    void Start()
    {
        ShowNextQuestionsLerp();
    }

    public void StartTransition()
    {
        ShowNextQuestionsLerp();
    }

    protected void Update()
    {
        // UP-DOWN LERP ----------------------
        // lerping the canvas moving up/down (using anchoredPosition)

        if (hideButtonLerp)
        {
            hideButtoCurrentLerpTime += Time.deltaTime;

            if (hideButtoCurrentLerpTime > hideButtonLerpTime)
            {
                hideButtoCurrentLerpTime = hideButtonLerpTime;
                hideButtonLerp = false;
                KeepPlayingButton.interactable = true;
            }

            float perc = hideButtoCurrentLerpTime/hideButtonLerpTime;
            BackgroundPlaneRect.anchoredPosition = Vector2.Lerp(hideButtonStartPos, hideButtonEndPos, perc);
        }
        
        // LEFT-RIGHT lerp -------------
        else if (nextQuestionLerp)
        {
            nextQuestionCurrentLerpTime += Time.deltaTime;

            if (nextQuestionCurrentLerpTime > nextQuestionLerpTime)
            {
                nextQuestionCurrentLerpTime = nextQuestionLerpTime;
                nextQuestionLerp = false;
                KeepPlayingButton.interactable = true;

                if (questionIndex >= 4)
                    StartCoroutine(GoToNextRound());
            }

            float perc_ = nextQuestionCurrentLerpTime / nextQuestionLerpTime;
            BackgroundPlaneRect.anchoredPosition = Vector2.Lerp(nextQuestionStartPos, nextQuestionEndPos, perc_);
        }
        // --------------------


    }

    IEnumerator GoToNextRound()
    {
        yield return new WaitForSeconds(0.2f);

        StateManager.Instance.ContinueToNextRound();
    }

    public int questionIndex = 0;
    private bool nextQuestionLerp;
    private float nextQuestionCurrentLerpTime;
    private float nextQuestionLerpTime = 0.5f;
    private Vector2 nextQuestionStartPos, nextQuestionEndPos;

    public void ShowNextQuestionsLerp()
    {
        // width = 960 * 4, height = 600

        if (nextQuestionLerp)
            return;

        questionIndex++;
        float endPos = 960 * questionIndex;

        nextQuestionLerp = true;
        nextQuestionCurrentLerpTime = 0f;

        nextQuestionStartPos = BackgroundPlaneRect.anchoredPosition;
        nextQuestionEndPos = new Vector2(-endPos, 0);

        KeepPlayingButton.interactable = false;

    }

    public void ToggleHideQuestions()
    {
        // using anchoredPosition instead of setting Transform directly
        // seems to work more reliable (in web) with the parent/children transforms

        if (hideButtonLerp)
            return;

        hideButtonLerp = true;
        hideButtoCurrentLerpTime = 0f;
        hideButtonGoingUp = !hideButtonGoingUp;

        if (hideButtonGoingUp)
        {
            hideButtonEndPos = new Vector2(BackgroundPlaneRect.anchoredPosition.x, 600);
            KeepPlayingButtonText.text = "Resume Questions";
            Demographics.Instance.MyGameState = GameState.Playing;
        }
        else
        {
            hideButtonEndPos = new Vector2(BackgroundPlaneRect.anchoredPosition.x, 0);
            KeepPlayingButtonText.text = "Resume Playing";
            Demographics.Instance.MyGameState = GameState.MidQuestionnaire;
        }

        hideButtonStartPos = BackgroundPlaneRect.anchoredPosition;

        KeepPlayingButton.interactable = false;
    }
}
