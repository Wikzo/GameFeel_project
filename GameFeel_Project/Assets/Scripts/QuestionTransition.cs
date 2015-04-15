using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestionTransition : MonoBehaviour
{
    public List<GameObject> ObjectsToMove;
    public Vector3 MoveToOffset;
    public float TransitionTime = 1f;
    public float TransitionTimeIncrement = 0.1f;
    public iTween.EaseType EasingType;

    private List<Vector3> _objectsDefaultPos;
    private int _sequenceNumber = 0;
    
    private void Start()
    {
        _objectsDefaultPos = new List<Vector3>(ObjectsToMove.Count);
        for (int index = 0; index < ObjectsToMove.Count; index++)
            _objectsDefaultPos.Add(ObjectsToMove[index].transform.position);

        NextTransition(0);

    }

    int numberOfElements = 1;


    public void NextTransition(int sequenceNumber)
    {
        _sequenceNumber = sequenceNumber;

        int offset = 0;

        switch (sequenceNumber)
        {
            case 1:
                numberOfElements = 3;
                break;
            case 2:
                numberOfElements = 7;
                break;
            case 3: //
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

            offset = index;

            if (offset > numberOfElements)
                offset = numberOfElements;

            Vector3 startPos = ObjectsToMove[index].transform.position;

            Hashtable ht = new Hashtable();
            ht.Add("easetype", EasingType);
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

        if (_sequenceNumber >= 3)
            StartCoroutine(WaitAndThenGoToNewRound());
    }

    IEnumerator WaitAndThenGoToNewRound()
    {
        yield return new WaitForSeconds(TransitionTime + 6*TransitionTimeIncrement);

        StateManager.Instance.ContinueToNextRound();

    }

}
