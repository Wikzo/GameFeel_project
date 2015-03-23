using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParameterManager : MonoBehaviour
{
    public List<TweakableParameters> MyParameters;
    
    public int Index = 0;

    public bool UseRandomGravity = false;
    public bool UseRandomJumpPower = false;
    public bool UseRandomUseAirFriction = true;
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

    private bool parametersCreated;
    public void MakeParameters()
    {
        parametersCreated = true;
        MakeParameters(5, true);
        Debug.Log("making 5 default parameters");

    }

    public void MakeParameters(int size, bool makeDuplicates)
    {
        MyParameters = new List<TweakableParameters>(size);
        List<TweakableParameters> MyParametersDuplicates = new List<TweakableParameters>(size);

        


        for (int i = 0; i < size; i++)
        {
            // Random.Range: min [inclusive], max [exclusive]
            // ? identifier makes it nullable

            float tempGravity = Random.Range(TweakableParameters.GravityRange.x, TweakableParameters.GravityRange.y); // needs to be calculated internally for the terminal velocity to work!
            float? terminalVelocity = UseRandomTerminalVelocity ? Random.Range(tempGravity, TweakableParameters.GravityRange.y) : (float?)null;
            float? gravity = UseRandomGravity ?  tempGravity : (float?)null;
            float? jumpPower = UseRandomJumpPower ? Random.Range(TweakableParameters.JumpPowerRange.x, TweakableParameters.JumpPowerRange.y) : (float?)null;
            bool? useAirFriction = UseRandomUseAirFriction ? true : (bool?)null;
            float? airFrictionHorizontal = Random.Range(TweakableParameters.AirFrictionHorizontalPercentageRange.x, TweakableParameters.AirFrictionHorizontalPercentageRange.y);
            float? ghostJumpTime = UseRandomGhostJumpTime ? Random.Range(TweakableParameters.GhostJumpTimeRange.x, TweakableParameters.GhostJumpTimeRange.y) : (float?)null;
            float? minimumJumpHeight = UseRandomMinimumJumpHeight ? Random.Range(TweakableParameters.MinimumJumpHeightRange.x, TweakableParameters.MinimumJumpHeightRange.y) : (float?) null;
            float? releaseEarlyJumpVelocity = UseRandomReleaseEarlyJumpVelocity ? Random.Range(TweakableParameters.ReleaseEarlyJumpVelocityRange.x, TweakableParameters.ReleaseEarlyJumpVelocityRange.y) : (float?)null;
            float? apexGravityMultiplier = UseRandomApexGravityMultiplier ? Random.Range(TweakableParameters.ApexGravityMultiplierRange.x, TweakableParameters.ApexGravityMultiplierRange.y) : (float?)null;
            float? maxVelocityX = UseRandomMaxVelocity ? Random.Range(TweakableParameters.MaxVelocityXRange.x, TweakableParameters.MaxVelocityXRange.y) : (float?)null;
            bool? useGroundFriction = UseRandomGroundFriction ? true : (bool?)null;
            float? groundFriction = UseRandomGroundFriction ? Random.Range(TweakableParameters.GroundFrictionPercentageRange.x, TweakableParameters.GroundFrictionPercentageRange.y) : (float?)null;
            float? releaseTime = UseRandomReleaseTime ? Random.Range(TweakableParameters.ReleaseTimeRange.x, TweakableParameters.ReleaseTimeRange.y) : (float?)null;
            float? attackTime = UseRandomAttackTime ? Random.Range(TweakableParameters.AttackTimeRange.x, TweakableParameters.AttackTimeRange.y) : (float?)null;
            float? turnAroundBoostPercent = UseRandomTurnAroundBoost ? Random.Range(TweakableParameters.TurnAroundBoostPercentRange.x, TweakableParameters.TurnAroundBoostPercentRange.y) : (float?)null;
            bool? useCurveForHorizontalAttackVelocity = true;
            bool? useCurveForHorizontalReleaseVelocity = true;
            bool? useAnimation = true;
            float? animationMaxSpeed = UseRandomAnimationMaxSpeed ? Random.Range(TweakableParameters.AnimationMaxSpeedRange.x, TweakableParameters.AnimationMaxSpeedRange.y) : (float?)null;

            MyParameters.Add(new TweakableParameters(gravity, jumpPower, useAirFriction, airFrictionHorizontal, terminalVelocity, ghostJumpTime,
                minimumJumpHeight, releaseEarlyJumpVelocity, apexGravityMultiplier, maxVelocityX, useGroundFriction,
                groundFriction, releaseTime, attackTime, turnAroundBoostPercent, useCurveForHorizontalAttackVelocity, useCurveForHorizontalReleaseVelocity,
                useAnimation, null, null));

            if (makeDuplicates)
            {
                MyParametersDuplicates.Add(new TweakableParameters(gravity, jumpPower, useAirFriction, airFrictionHorizontal, terminalVelocity, ghostJumpTime,
                minimumJumpHeight, releaseEarlyJumpVelocity, apexGravityMultiplier, maxVelocityX, useGroundFriction,
                groundFriction, releaseTime, attackTime, turnAroundBoostPercent, useCurveForHorizontalAttackVelocity, useCurveForHorizontalReleaseVelocity,
                useAnimation, null, 1));
            }
            
        }

        if (makeDuplicates)
            MyParameters.AddRange(MyParametersDuplicates);
    }
}
