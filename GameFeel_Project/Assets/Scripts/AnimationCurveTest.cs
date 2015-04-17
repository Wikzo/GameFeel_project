using UnityEngine;
using System.Collections;

public class AnimationCurveTest : MonoBehaviour
{

    public AnimationCurve PositiveCurve;
    public AnimationCurve NegativeCurveTest_old;
    public AnimationCurve NegativeCurve;
    public AnimationCurve Combined;

//    private bool useOriginal = true;

  //  private float timer = 1;

    // Use this for initialization
    void Start()
    {

        Keyframe[] keyframes = new Keyframe[PositiveCurve.length];

        //for (int i = 0; i < NegativeCurve.length; i++)
          //  NegativeCurveTest_old.RemoveKey(i);

        for (int i = 0; i < PositiveCurve.length; i++)
        {
            NegativeCurveTest_old.AddKey(1 - PositiveCurve[i].time, PositiveCurve[i].value);

            NegativeCurveTest_old.SmoothTangents(i, 0);


            keyframes[i] = new Keyframe(1-PositiveCurve[i].time, PositiveCurve[i].value);
            keyframes[i].value = PositiveCurve[i].value;
            keyframes[i].tangentMode = PositiveCurve[i].tangentMode;
            keyframes[i].inTangent = PositiveCurve[i].inTangent;
            keyframes[i].outTangent = PositiveCurve[i].inTangent;
        }

        NegativeCurve = new AnimationCurve(keyframes);
        Combined = new AnimationCurve(keyframes);

        

        for (int i = 0; i < NegativeCurve.length; i++)
        {
            NegativeCurve.SmoothTangents(i, 0);


            Combined.AddKey(Player.NormalizationMap(NegativeCurveTest_old[i].time, 0, 1, -1, 0), NegativeCurveTest_old[i].value);

        }

        for (int i = 0; i < PositiveCurve.length; i++)
            Combined.AddKey(Player.NormalizationMap(PositiveCurve[i].time, 0, 1, 0, 1), PositiveCurve[i].value);
    }

    // Update is called once per frame
    /*void Update()
    {
        return;
        timer -= Time.deltaTime;

        if (useOriginal)
            Debug.Log(PositiveCurve.Evaluate(timer));
        else
            Debug.Log(NegativeCurve.Evaluate(timer));

        if (timer <= 0)
        {
            useOriginal = !useOriginal;
            timer = 1;
            Debug.Log("change");
        }


    }
     * */
}
