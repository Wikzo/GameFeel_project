using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class TweakableParameters
{
    static public Vector2 GravityRange = new Vector2(-5f,-30.1f);
    static public Vector2 TerminalVelocityRange = new Vector2(-5,-60.1f);
    static public Vector2 JumpPowerRange = new Vector2(2f,20.1f);
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
        new Vector2(0.001f, 0.05f), // x2 y4
        new Vector2(0.051f, 0.1f), // x3 y2
        new Vector2(0.11f, 0.24f), // x1 y3
        new Vector2(0.241f, 0.8f),  // x4 y5
        new Vector2(0.81f, 2.1f),  // x5 y1
    };



    static public Vector2 TurnAroundBoostPercentRange = new Vector2(0f, 200.1f);
    static public Vector2 AnimationMaxSpeedRange = new Vector2(50f, 150f);


    public TweakableParameters(float? gravity, float? jumpPower, bool? useAirFriction, float? airFrictionHorizontal,
        float? terminalVelocity, float? ghostJumpTime, float? minimumJumpHeight, float? releaseEarlyJumpVelocity,
        float? apexGravityMultiplier, float? maxVelocityX, bool? useGroundFriction, float? groundFrictionPercentage,
        float? releaseTime, float? attackTime, float? turnAroundBoostPercent, bool? useCurveForHorizontalAttackVelocity,
        bool? useCurveForHorizontalReleaseVelocity, bool? useAnimation, float? animationMaxSpeed, int? isDuplicate)
    {
        if (gravity.HasValue)
            Gravity = new Vector3(0, gravity.Value, 0);
        if (jumpPower.HasValue)
            JumpPower = jumpPower.Value;
    
        if (useAirFriction.HasValue)
            UseAirFriction = useAirFriction.Value;

        if (airFrictionHorizontal.HasValue)
            AirFrictionHorizontalPercentage = airFrictionHorizontal.Value;

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

    // air
    public Vector3 Gravity = new Vector3(0, -10f, 0);
    public float JumpPower = 15;
    public bool UseAirFriction = true;
    public float AirFrictionHorizontalPercentage = 90f;
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


    public override string ToString()
    {
            return string.Format("Gravity: {0}\n JumpPower: {1}\n UseAirFriction: {2}\n AirFrictionHorizontalPercentage: {3}\n TerminalVelocity: {4}\n GhostJumpTime: {5}\n MinimumJumpHeight: {6}\n ReleaseEarlyJumpVelocity: {7}\n ApexGravityMultiplier: {8}\n MaxVelocityX: {9}\n UseGroundFriction: {10}\n GroundFrictionPercentage: {11}\n ReleaseTime: {12}\n AttackTime: {13}\n TurnAroundBoostPercent: {14}\n UseCurveForHorizontalAttackVelocity: {15}\n UseCurveForHorizontalReleaseVelocity: {16}\n UseAnimation: {17}\n AnimationMaxSpeed: {18}", Gravity.y, JumpPower, Convert.ToInt32(UseAirFriction), AirFrictionHorizontalPercentage, TerminalVelocity, GhostJumpTime, MinimumJumpHeight, ReleaseEarlyJumpVelocity, ApexGravityMultiplier, MaxVelocityX, Convert.ToInt32(UseGroundFriction), GroundFrictionPercentage, ReleaseTime, AttackTime, TurnAroundBoostPercent, Convert.ToInt32(UseCurveForHorizontalAttackVelocity), Convert.ToInt32(UseCurveForHorizontalReleaseVelocity), Convert.ToInt32(UseAnimation), AnimationMaxSpeed);
    }

    public string[] ToStringList()
    {
        // splits into individual entries

        string[] lines = ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        return lines;
    }

    // TODO: remember to remove ground friction (and more)
    public string ToStringDatabaseFormat(bool useSeperators)
    {
        if (useSeperators)
            return string.Format("&Gravity={0}&JumpPower={1}&UseAirFriction={2}&AirFrictionHorizontalPercentage={3}&TerminalVelocity={4}&GhostJumpTime={5}&MinimumJumpHeight={6}&ReleaseEarlyJumpVelocity={7}&ApexGravityMultiplier={8}&MaxVelocityX={9}&UseGroundFriction={10}&GroundFrictionPercentage={11}&ReleaseTime={12}&AttackTime={13}&TurnAroundBoostPercent={14}&UseCurveForHorizontalAttackVelocity={15}&UseCurveForHorizontalReleaseVelocity={16}&UseAnimation={17}&AnimationMaxSpeed={18}", Gravity.y, JumpPower, Convert.ToInt32(UseAirFriction), AirFrictionHorizontalPercentage, TerminalVelocity, GhostJumpTime, MinimumJumpHeight, ReleaseEarlyJumpVelocity, ApexGravityMultiplier, MaxVelocityX, Convert.ToInt32(UseGroundFriction), GroundFrictionPercentage, ReleaseTime, AttackTime, TurnAroundBoostPercent, Convert.ToInt32(UseCurveForHorizontalAttackVelocity), Convert.ToInt32(UseCurveForHorizontalReleaseVelocity), Convert.ToInt32(UseAnimation), AnimationMaxSpeed);
        else
            return string.Format(Gravity.y + JumpPower+Convert.ToInt32(UseAirFriction).ToString()+AirFrictionHorizontalPercentage+TerminalVelocity+GhostJumpTime+MinimumJumpHeight+ReleaseEarlyJumpVelocity+ApexGravityMultiplier+MaxVelocityX+Convert.ToInt32(UseGroundFriction).ToString()+GroundFrictionPercentage+ReleaseTime+AttackTime+TurnAroundBoostPercent+Convert.ToInt32(UseCurveForHorizontalAttackVelocity).ToString()+Convert.ToInt32(UseCurveForHorizontalReleaseVelocity).ToString()+Convert.ToInt32(UseAnimation).ToString()+AnimationMaxSpeed);
    }


}