using UnityEngine;
using System.Collections;

[System.Serializable]
public class TweakableParameters
{
    public TweakableParameters(float gravity, float jumpPower, bool useAirFriction, float airFrictionHorizontal,
        float terminalVelocity, float ghostJumpTime, float minimumJumpHeight, float releaseEarlyJumpVelocity,
        float apexGravityMultiplier, float maxVelocityX, bool useGroundFriction, float groundFrictionPercentage,
        float releaseTime, float attackTime, float turnAroundBoostPercent, bool useCurveForHorizontalAttackVelocity,
        bool useCurveForHorizontalReleaseVelocity, bool useAnimation, float animationMaxSpeed)
    {
        Gravity = new Vector3(0, gravity, 0);
        JumpPower = jumpPower;
        UseAirFriction = useAirFriction;
        AirFrictionHorizontal = airFrictionHorizontal;
        TerminalVelocity = terminalVelocity;
        GhostJumpTime = ghostJumpTime;
        MinimumJumpHeight = minimumJumpHeight;
        ReleaseEarlyJumpVelocity = releaseEarlyJumpVelocity;
        ApexGravityMultiplier = apexGravityMultiplier;
        MaxVelocityX = maxVelocityX;
        UseGroundFriction = useGroundFriction;
        GroundFrictionPercentage = groundFrictionPercentage;
        ReleaseTime = releaseTime;
        AttackTime = attackTime;
        TurnAroundBoostPercent = turnAroundBoostPercent;
        UseCurveForHorizontalAttackVelocity = useCurveForHorizontalAttackVelocity;
        UseCurveForHorizontalReleaseVelocity = useCurveForHorizontalReleaseVelocity;
        UseAnimation = useAnimation;
        AnimationMaxSpeed = animationMaxSpeed;
    }

    // air
    public Vector3 Gravity = new Vector3(0, -10f, 0);
    public float JumpPower = 10;
    public bool UseAirFriction = true;
    [Range(0.1f, 1f)] public float AirFrictionHorizontal = 0.5f;
    public float TerminalVelocity = -5f;
    [Range(0f, 2f)] public float GhostJumpTime = 0.4f;
    public float MinimumJumpHeight = 0.5f;
    public float ReleaseEarlyJumpVelocity = 0.5f;
    public float ApexGravityMultiplier = 3;

    // ground
    [Range(1f, 50)] public float MaxVelocityX = 15f;
    public bool UseGroundFriction = true;
    [Range(0f, 100f)] public float GroundFrictionPercentage = 50f;
    public float ReleaseTime = 0.4f;
    public float AttackTime = 0.4f;
    [Range(0f, 200f)] public float TurnAroundBoostPercent = 0f;

    public bool UseCurveForHorizontalAttackVelocity = true;
    public bool UseCurveForHorizontalReleaseVelocity = true;
    //public AnimationCurve[] HorizontalVelocityCurvesAttack;
    //public AnimationCurve[] HorizontalVelocityCurvesRelease;

    // animation
    public bool UseAnimation = true;
    [Range(0.1f, 2f)] public float AnimationMaxSpeed = 1.4f;


    public override string ToString()
    {
        return string.Format("Gravity: {0}\n JumpPower: {1}\n UseAirFriction: {2}\n AirFrictionHorizontal: {3}\n TerminalVelocity: {4}\n GhostJumpTime: {5}\n MinimumJumpHeight: {6}\n ReleaseEarlyJumpVelocity: {7}\n ApexGravityMultiplier: {8}\n MaxVelocityX: {9}\n UseGroundFriction: {10}\n GroundFrictionPercentage: {11}\n ReleaseTime: {12}\n AttackTime: {13}\n TurnAroundBoostPercent: {14}\n UseCurveForHorizontalAttackVelocity: {15}\n UseCurveForHorizontalReleaseVelocity: {16}\n UseAnimation: {17}\n AnimationMaxSpeed: {18}", Gravity, JumpPower, UseAirFriction, AirFrictionHorizontal, TerminalVelocity, GhostJumpTime, MinimumJumpHeight, ReleaseEarlyJumpVelocity, ApexGravityMultiplier, MaxVelocityX, UseGroundFriction, GroundFrictionPercentage, ReleaseTime, AttackTime, TurnAroundBoostPercent, UseCurveForHorizontalAttackVelocity, UseCurveForHorizontalReleaseVelocity, UseAnimation, AnimationMaxSpeed);
    }
}