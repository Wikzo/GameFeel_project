using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class TweakableParameters
{
    static public Vector2 GravityRange = new Vector2(-5f,-30.1f);
    static public Vector2 TerminalVelocityRange = new Vector2(-5,-60.1f);
    static public Vector2 JumpPowerRange = new Vector2(2f,30.1f);
    static public Vector2 AirFrictionHorizontalPercentageRange = new Vector2(0, 99.1f);
    static public Vector2 GhostJumpTimeRange = new Vector2(0f,2.1f);
    static public Vector2 MinimumJumpHeightRange = new Vector2(0.1f, 5.1f);
    static public Vector2 ReleaseEarlyJumpVelocityRange = new Vector2(0f, 3.1f);
    static public Vector2 ApexGravityMultiplierRange = new Vector2(1f, 15.1f);
    static public Vector2 MaxVelocityXRange = new Vector2(1, 20.1f);
    static public Vector2 GroundFrictionPercentageRange = new Vector2(0f, 99.1f);

    static public Vector2 ReleaseTimeRange = new Vector2(0.001f, 2.1f);
    static public Vector2 AttackTimeRange = new Vector2(0.001f, 2.1f);

    static public Vector2[] AttackAndReleaseTimes = new[]
    { 
        // milliseconds
        new Vector2(0.001f, 0.1f),
        new Vector2(0.011f, 0.24f),
        new Vector2(0.241f, 1f),
        new Vector2(1.1f, 3.1f),
    };



    static public Vector2 TurnAroundBoostPercentRange = new Vector2(100f, 400.1f);
    static public Vector2 AnimationMaxSpeedRange = new Vector2(50f, 150f);


    public TweakableParameters(float? gravity, float? jumpPower, bool? useAirFriction, bool? keepGroundMomentumAfterJump, float? airFrictionHorizontal,
        float? terminalVelocity, float? ghostJumpTime, float? minimumJumpHeight, float? releaseEarlyJumpVelocity,
        float? apexGravityMultiplier, float? maxVelocityX, bool? useGroundFriction, float? groundFrictionPercentage,
        float? releaseTime, float? attackTime, float? turnAroundBoostPercent, bool? useCurveForHorizontalAttackVelocity,
        bool? useCurveForHorizontalReleaseVelocity, bool? useAnimation, float? animationMaxSpeed, int? isDuplicate)
    {

        MyRating = new Rating();

        if (gravity.HasValue)
            Gravity = new Vector3(0, gravity.Value, 0);
        if (jumpPower.HasValue)
            JumpPower = jumpPower.Value;
    
        if (useAirFriction.HasValue)
            UseAirFriction = useAirFriction.Value;

        if (airFrictionHorizontal.HasValue)
            AirFrictionHorizontalPercentage = airFrictionHorizontal.Value;

        if (keepGroundMomentumAfterJump.HasValue)
            KeepGroundMomentumAfterJump = keepGroundMomentumAfterJump.Value;

        if (terminalVelocity.HasValue)
            TerminalVelocity = terminalVelocity.Value;

        if (ghostJumpTime.HasValue)
            GhostJumpTime = ghostJumpTime.Value;

        if (minimumJumpHeight.HasValue)      
            MinimumJumpHeight = minimumJumpHeight.Value;

        if (releaseEarlyJumpVelocity.HasValue)      
            ReleaseEarlyJumpVelocity = releaseEarlyJumpVelocity.Value;

        if (apexGravityMultiplier.HasValue)      
            ApexGravityMultiplier = apexGravityMultiplier.Value;

        if (maxVelocityX.HasValue)      
            MaxVelocityX = maxVelocityX.Value;

        if (useGroundFriction.HasValue)      
            UseGroundFriction = useGroundFriction.Value;

        if (groundFrictionPercentage.HasValue)      
            GroundFrictionPercentage = groundFrictionPercentage.Value;

        if (releaseTime.HasValue)      
            ReleaseTime = releaseTime.Value;

        if (attackTime.HasValue)      
            AttackTime = attackTime.Value;

        if (turnAroundBoostPercent.HasValue)      
            TurnAroundBoostPercent = turnAroundBoostPercent.Value;

        if (useCurveForHorizontalAttackVelocity.HasValue)      
            UseCurveForHorizontalAttackVelocity = useCurveForHorizontalAttackVelocity.Value;

        if (useCurveForHorizontalReleaseVelocity.HasValue)      
            UseCurveForHorizontalReleaseVelocity = useCurveForHorizontalReleaseVelocity.Value;

        if (useAnimation.HasValue)      
            UseAnimation = useAnimation.Value;

        if (animationMaxSpeed.HasValue)      
            AnimationMaxSpeed = animationMaxSpeed.Value;

        if (isDuplicate.HasValue)
            IsDuplicate = 1;
        else
            IsDuplicate = 0;
    }


    // ratings
    public Rating MyRating;

    // air
    public Vector3 Gravity = new Vector3(0, -10f, 0);
    public float JumpPower = 15;
    public bool UseAirFriction = true;
    public float AirFrictionHorizontalPercentage = 90f;
    public bool KeepGroundMomentumAfterJump = true;
    public float TerminalVelocity = -20f;
    public float GhostJumpTime = 0.4f;
    public float MinimumJumpHeight = 0.5f;
    public float ReleaseEarlyJumpVelocity = 0.5f;
    public float ApexGravityMultiplier = 3;

    // ground
    public float MaxVelocityX = 15f;
    public bool UseGroundFriction = true;
    public float GroundFrictionPercentage = 50f;
    public float ReleaseTime = 0.4f;
    public float AttackTime = 0.4f;
    public float TurnAroundBoostPercent = 0f;

    public bool UseCurveForHorizontalAttackVelocity = true;
    public bool UseCurveForHorizontalReleaseVelocity = true;
    //public AnimationCurve[] HorizontalVelocityCurvesAttack;
    //public AnimationCurve[] HorizontalVelocityCurvesRelease;

    // animation
    public bool UseAnimation = true;
    public float AnimationMaxSpeed = 100f;

    // extra
    public int IsDuplicate;

    // stage
    public int Level = 0;
    public int Deaths = 0;
    public float TimeSpentOnLevel = 0;


    /*public override string ToString()
    {
            return string.Format("Gravity: {0}\n JumpPower: {1}\n UseAirFriction: {2}\n AirFrictionHorizontalPercentage: {3}\n TerminalVelocity: {4}\n GhostJumpTime: {5}\n MinimumJumpHeight: {6}\n ReleaseEarlyJumpVelocity: {7}\n ApexGravityMultiplier: {8}\n MaxVelocityX: {9}\n UseGroundFriction: {10}\n GroundFrictionPercentage: {11}\n ReleaseTime: {12}\n AttackTime: {13}\n TurnAroundBoostPercent: {14}\n UseCurveForHorizontalAttackVelocity: {15}\n UseCurveForHorizontalReleaseVelocity: {16}\n UseAnimation: {17}\n AnimationMaxSpeed: {18}", Gravity.y, JumpPower, Convert.ToInt32(UseAirFriction), AirFrictionHorizontalPercentage, TerminalVelocity, GhostJumpTime, MinimumJumpHeight, ReleaseEarlyJumpVelocity, ApexGravityMultiplier, MaxVelocityX, Convert.ToInt32(UseGroundFriction), GroundFrictionPercentage, ReleaseTime, AttackTime, TurnAroundBoostPercent, Convert.ToInt32(UseCurveForHorizontalAttackVelocity), Convert.ToInt32(UseCurveForHorizontalReleaseVelocity), Convert.ToInt32(UseAnimation), AnimationMaxSpeed);
    }*/

    public string[] ToStringList()
    {
        // splits into individual entries

        string[] lines = ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        return lines;
    }

    public string ToStringDatabaseFormat()
    {
        this.MyRating.Description = "fesen";
            return string.Format("&Gravity={0}&JumpPower={1}&AirFrictionHorizontalPercentage={2}&TerminalVelocity={3}&GhostJumpTime={4}&MinimumJumpHeight={5}&ReleaseEarlyJumpVelocity={6}&ApexGravityMultiplier={7}&MaxVelocityX={8}&ReleaseTime={9}&AttackTime={10}&AnimationMaxSpeed={11}&Level={12}&Deaths={13}&TimeSpentOnLevel={14}",
                Gravity.y,
                JumpPower,
                AirFrictionHorizontalPercentage,
                TerminalVelocity,
                GhostJumpTime,
                MinimumJumpHeight,
                ReleaseEarlyJumpVelocity,
                ApexGravityMultiplier,
                MaxVelocityX,
                ReleaseTime,
                AttackTime,
                AnimationMaxSpeed,
                ParameterManager.Instance.Index,
                StateManager.Instance.DeathsOnThisLevel,
                StateManager.Instance.TimeSpentOnLevel);
    }


}