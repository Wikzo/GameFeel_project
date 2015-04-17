using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ParameterManager : MonoBehaviour
{
    public List<TweakableParameters> MyParameters;
    public List<GameObject> MyQuestionnaireUI;
    public List<QuestionnaireData2> MyQuestionnaireData;
    public Player Player;
    public int NumberOfParameters = 4;
    public int LatinSequence = 1;

    public int Level = 0;

    public string LatinSquareSequenceDatabaseFormat()
    {
        // seq1: AA, BB, BA, AB
        // seq2: BB, AB, AA, BA
        // seq3: AB, BA, BB, AA
        // seq4: BA, AA, AB, BB

        string letters = "";

        int myLevel = Level + 1;

        switch (LatinSequence)
        {
            case 1:
                if (myLevel == 1)
                    letters = "AA";
                else if (myLevel == 2)
                    letters = "BB";
                else if (myLevel == 3)
                    letters = "BA";
                else if (myLevel == 4)
                    letters = "AB";

                break;
            case 2:
                if (myLevel == 1)
                    letters = "BB";
                else if (myLevel == 2)
                    letters = "AB";
                else if (myLevel == 3)
                    letters = "AA";
                else if (myLevel == 4)
                    letters = "BA";
                break;
            case 3:
                if (myLevel == 1)
                    letters = "AB";
                else if (myLevel == 2)
                    letters = "BA";
                else if (myLevel == 3)
                    letters = "BB";
                else if (myLevel == 4)
                    letters = "AA";
                break;
            case 4:
                if (myLevel == 1)
                    letters = "BA";
                else if (myLevel == 2)
                    letters = "AA";
                else if (myLevel == 3)
                    letters = "AB";
                else if (myLevel == 4)
                    letters = "BB";
                break;



        }
        return string.Format("&LatinSequence={0}-{1}", LatinSequence.ToString(), letters);
    }
    

    public bool UseRandomGravity = false;
    public bool UseRandomJumpPower = false;
    public bool UseRandomUseAirFriction = true;
    public bool UseRandomKeepGroundMomentumAfterJump = true;
    public bool UseRandomAirFrictionHorizontal = true;
    public bool UseRandomTerminalVelocity = false;
    public bool UseRandomGhostJumpTime = true;
    public bool UseRandomMinimumJumpHeight = true;
    public bool UseRandomReleaseEarlyJumpVelocity = true;
    public bool UseRandomApexGravityMultiplier = true;
    public bool UseRandomMaxVelocity = false;
    public bool UseRandomGroundFriction = false;
    public bool UseRandomReleaseTime = true;
    public bool UseRandomAttackTime = true;
    public bool UseRandomTurnAroundBoost = true;
    public bool UseRandomAnimationMaxSpeed = false;

    public char[] _releaseTimes;
    public char[] _attackTimes;
    private bool parametersCreated;


    private static ParameterManager _instance;
    public static ParameterManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(ParameterManager)) as ParameterManager;

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

        if (!parametersCreated)
            MakeParameters();
    }

    public void MakeParameters()
    {
        parametersCreated = true;
        MakeParameters(NumberOfParameters, false);
        //Debug.Log("making 5 default parameters");

    }

    void Start()
    {
        LatinSequence = Demographics.Instance.LatinSquareSequence;

        if (NumberOfParameters != MyQuestionnaireUI.Count)
        {
            Debug.Log("Error - need to have " + NumberOfParameters + " MyQuestionnaireUI canvases!");
            return;
        }

        MyQuestionnaireData = new List<QuestionnaireData2>(MyQuestionnaireUI.Count);
        
        for (int i = 0; i < MyQuestionnaireUI.Count; i++)
        {
            GameObject g = MyQuestionnaireUI[i].gameObject;

            MyQuestionnaireData.Add(g.GetComponent<QuestionnaireData2>());
        }
    }

    void MakeRandomReleaseAttackTimesWithinSegments()
    {
        // Balanced Latin Square (incomplete)
        // https://explorable.com/counterbalanced-measures-design
        // http://rintintin.colorado.edu/~chathach/balancedlatinsquares.html

        LatinSequence = Demographics.Instance.LatinSquareSequence;

        if (LatinSequence > NumberOfParameters)
            Debug.Log("Error! Latin square sequence is bigger than NumberOfParameters!");

        _releaseTimes = new char[NumberOfParameters];
        _attackTimes = new char[NumberOfParameters];

        // a = small
        // b = large

        // [attack] [release]

        // seq1: AA, BB, BA, AB
        // seq2: BB, AB, AA, BA
        // seq3: AB, BA, BB, AA
        // seq4: BA, AA, AB, BB

        //Debug.Log("Using Latin square sequence: " + LatinSequence);

        switch (LatinSequence)
        {
            case 1:
                _attackTimes[0] = 'a';
                _releaseTimes[0] = 'a';

                _attackTimes[1] = 'b';
                _releaseTimes[1] = 'b';

                _attackTimes[2] = 'b';
                _releaseTimes[2] = 'a';

                _attackTimes[3] = 'a';
                _releaseTimes[3] = 'b';

                break;

            case 2:
                _attackTimes[0] = 'b';
                _releaseTimes[0] = 'b';

                _attackTimes[1] = 'a';
                _releaseTimes[1] = 'b';

                _attackTimes[2] = 'a';
                _releaseTimes[2] = 'a';

                _attackTimes[3] = 'b';
                _releaseTimes[3] = 'a';

                break;

            case 3:
                _attackTimes[0] = 'a';
                _releaseTimes[0] = 'b';

                _attackTimes[1] = 'b';
                _releaseTimes[1] = 'a';

                _attackTimes[2] = 'b';
                _releaseTimes[2] = 'b';

                _attackTimes[3] = 'a';
                _releaseTimes[3] = 'a';

                break;

            case 4:
                _attackTimes[0] = 'b';
                _releaseTimes[0] = 'a';

                _attackTimes[1] = 'a';
                _releaseTimes[1] = 'a';

                _attackTimes[2] = 'a';
                _releaseTimes[2] = 'b';

                _attackTimes[3] = 'b';
                _releaseTimes[3] = 'b';

                break;
        }


        return;


        /*for (int i = 0; i < segments; i++)
            _releaseTimes[i] = i;

        ShuffleArray<int>(_releaseTimes);

        _attackTimes = new int[segments];
        for (int i = 0; i < segments; i++)
            _attackTimes[i] = i;

        ShuffleArray<int>(_attackTimes);

        string s = "AR: [";

        for (int i = 0; i < _attackTimes.Length; i++)
        {
            s += _attackTimes[i].ToString() + _releaseTimes[i].ToString();
            s += ", ";
        }

        s += "]";

        Debug.Log(s);*/
    }

    public void MakeParameters(int size, bool makeDuplicates)
    {
        MyParameters = new List<TweakableParameters>(size);
        List<TweakableParameters> MyParametersDuplicates = new List<TweakableParameters>(size);
        MakeRandomReleaseAttackTimesWithinSegments();

        for (int i = 0; i < size; i++)
        {
            // Random.Range: min [inclusive], max [exclusive]
            // ? identifier makes it nullable

            float tempGravity = Random.Range(TweakableParameters.GravityRange.x, TweakableParameters.GravityRange.y); // needs to be calculated internally for the terminal velocity to work!
            float? terminalVelocity = UseRandomTerminalVelocity ? Random.Range(tempGravity, TweakableParameters.GravityRange.y) : (float?)null;
            float? gravity = UseRandomGravity ?  tempGravity : (float?)null;
            float? jumpPower = UseRandomJumpPower ? Random.Range(TweakableParameters.JumpPowerRange.x, TweakableParameters.JumpPowerRange.y) : (float?)null;
            bool? useAirFriction = UseRandomUseAirFriction ? true : (bool?)null;
            bool? keepGroundMomentumAfterJump = UseRandomKeepGroundMomentumAfterJump ? true : (bool?)null;
            float? airFrictionHorizontal = UseRandomAirFrictionHorizontal ? Random.Range(TweakableParameters.AirFrictionHorizontalPercentageRange.x, TweakableParameters.AirFrictionHorizontalPercentageRange.y)  : (float?)null;
            float? ghostJumpTime = UseRandomGhostJumpTime ? Random.Range(TweakableParameters.GhostJumpTimeRange.x, TweakableParameters.GhostJumpTimeRange.y) : (float?)null;
            float? minimumJumpHeight = UseRandomMinimumJumpHeight ? Random.Range(TweakableParameters.MinimumJumpHeightRange.x, TweakableParameters.MinimumJumpHeightRange.y) : (float?) null;
            float? releaseEarlyJumpVelocity = UseRandomReleaseEarlyJumpVelocity ? Random.Range(TweakableParameters.ReleaseEarlyJumpVelocityRange.x, TweakableParameters.ReleaseEarlyJumpVelocityRange.y) : (float?)null;
            float? apexGravityMultiplier = UseRandomApexGravityMultiplier ? Random.Range(TweakableParameters.ApexGravityMultiplierRange.x, TweakableParameters.ApexGravityMultiplierRange.y) : (float?)null;
            float? maxVelocityX = UseRandomMaxVelocity ? Random.Range(TweakableParameters.MaxVelocityXRange.x, TweakableParameters.MaxVelocityXRange.y) : (float?)null;
            bool? useGroundFriction = UseRandomGroundFriction ? true : (bool?)null;
            float? groundFriction = UseRandomGroundFriction ? Random.Range(TweakableParameters.GroundFrictionPercentageRange.x, TweakableParameters.GroundFrictionPercentageRange.y) : (float?)null;
            
            // original --- completely random
            //float? releaseTime = UseRandomReleaseTime ? Random.Range(TweakableParameters.ReleaseTimeRange.x, TweakableParameters.ReleaseTimeRange.y) : (float?)null;
            //float? attackTime = UseRandomAttackTime ? Random.Range(TweakableParameters.AttackTimeRange.x, TweakableParameters.AttackTimeRange.y) : (float?)null;

            int releaseTimeIndex = -1;
            int attackTimeIndex = -1;

            if (_releaseTimes[i] == 'a')
                releaseTimeIndex = 0;
            else if (_releaseTimes[i] == 'b')
                releaseTimeIndex = 1;

            if (_attackTimes[i] == 'a')
                attackTimeIndex = 0;
            else if (_attackTimes[i] == 'b')
                attackTimeIndex = 1;


            float? releaseTime = UseRandomReleaseTime ? Random.Range(TweakableParameters.AttackAndReleaseTimes[releaseTimeIndex].x, TweakableParameters.AttackAndReleaseTimes[releaseTimeIndex].y) : (float?)null;
            float? attackTime = UseRandomAttackTime ? Random.Range(TweakableParameters.AttackAndReleaseTimes[attackTimeIndex].x, TweakableParameters.AttackAndReleaseTimes[attackTimeIndex].y) : (float?)null;

            /*string s = string.Format("Sequence {0}: {1}{2} ({3}; {4})",
                i,
                _releaseTimes[i],
                _attackTimes[i],
                releaseTime,
                attackTime);*/


            //Debug.Log(s);


            float? turnAroundBoostPercent = UseRandomTurnAroundBoost ? Random.Range(TweakableParameters.TurnAroundBoostPercentRange.x, TweakableParameters.TurnAroundBoostPercentRange.y) : (float?)null;
            bool? useAnimation = true;
            float? animationMaxSpeed = UseRandomAnimationMaxSpeed ? Random.Range(TweakableParameters.AnimationMaxSpeedRange.x, TweakableParameters.AnimationMaxSpeedRange.y) : (float?)null;

            MyParameters.Add(new TweakableParameters(gravity, jumpPower, useAirFriction, keepGroundMomentumAfterJump, airFrictionHorizontal, terminalVelocity, ghostJumpTime,
                minimumJumpHeight, releaseEarlyJumpVelocity, apexGravityMultiplier, maxVelocityX, useGroundFriction,
                groundFriction, releaseTime, attackTime, turnAroundBoostPercent,
                useAnimation, animationMaxSpeed, null));

            if (makeDuplicates)
            {
                MyParametersDuplicates.Add(new TweakableParameters(gravity, jumpPower, useAirFriction, keepGroundMomentumAfterJump, airFrictionHorizontal, terminalVelocity, ghostJumpTime,
                minimumJumpHeight, releaseEarlyJumpVelocity, apexGravityMultiplier, maxVelocityX, useGroundFriction,
                groundFriction, releaseTime, attackTime, turnAroundBoostPercent,
                useAnimation, animationMaxSpeed, 1));
            }
            
        }

        if (makeDuplicates)
            MyParameters.AddRange(MyParametersDuplicates);
    }


    public static void ShuffleArray<T>(T[] arr)
    {
        // http://answers.unity3d.com/questions/16531/randomizing-arrays.html
        // Fisher–Yates shuffle

        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    public static void Shuffle<T>(IList<T> list)
    {
        // http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp

        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
