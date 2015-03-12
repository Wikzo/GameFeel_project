using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Parameters
{
    public Parameters(float gravity,float terminalVelocity, float maxVelocityX, float jumpPower, float airFrictionHorizontal, float releaseTime,
        float attackTime, bool useAnimation, bool useGroundFriction, bool useAirFriction,float groundFriction,
        float gravityMultiplier, float releaseEarlyJumpVelocity, float minimumJumpHeight, float turnAroundBoost)
    {
        this.Gravity = new Vector3(0,gravity,0);
        this.TerminalVelocity = terminalVelocity;
        this.MaxVelocityX = maxVelocityX;
        this.JumpPower = jumpPower;
        this.AirFrictionHorizontal = airFrictionHorizontal;
        this.ReleaseTime = releaseTime;
        this.AttackTime = attackTime;
        this.UseAnimation = useAnimation;
        this.UseGroundFriction = useGroundFriction;
        this.UseAirFriction = useAirFriction;
        this.GroundFriction = groundFriction;
        this.GravityMultiplier = gravityMultiplier;
        this.ReleaseEarlyJumpVelocity = releaseEarlyJumpVelocity;
        this.MinimumJumpHeight = minimumJumpHeight;
        this.TurnAroundBoost = turnAroundBoost;
    }

    // tweakable jump parameters
    public Vector3 Gravity;
    public float TerminalVelocity;
    public float MaxVelocityX;
    public float JumpPower;
    public float AirFrictionHorizontal;
    public float ReleaseTime;
    public float AttackTime;
    public bool UseAnimation;
    public bool UseGroundFriction;
    public bool UseAirFriction;
    public float GroundFriction;
    public float GravityMultiplier;
    public float ReleaseEarlyJumpVelocity;
    public float MinimumJumpHeight;
    public float TurnAroundBoost;

}

public class ParameterManager : MonoBehaviour
{
    public List<Parameters> MyParameters;

    public int Index = 0;

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
        MakeParameters(5);
        Debug.Log("making 5 default parameters");

    }

    public void MakeParameters(int size)
    {
        MyParameters = new List<Parameters>(size);


        for (int i = 0; i < size; i++)
        {
            float gravity = Random.Range(-1, -30);
            float terminalVelocity = Random.Range(gravity, -30);
            float maxVelocityX = Random.Range(1, 50);
            float jumpPower = Random.Range(2, 20);
            float airFrictionHorizontal = Random.Range(0.1f, 1f);
            float releaseTime = Random.Range(0.001f, 2f);
            float attackTime = Random.Range(0.001f, 2f);
            float friction = Random.Range(0.05f, 0.8f);
            float apexGravityMultiplier = Random.Range(1f, 5f);
            float releaseEarlyJumpVelocity = Random.Range(0f, 1f);
            float minimumJumpHeight = Random.Range(0f, 5f);
            float turnAroundBoost = Random.Range(0f, 200f);

            MyParameters.Add(new Parameters(gravity, terminalVelocity, maxVelocityX, jumpPower, airFrictionHorizontal,
                releaseTime, attackTime, true, true, true, friction, apexGravityMultiplier, releaseEarlyJumpVelocity, minimumJumpHeight, turnAroundBoost));
        }

        //Debug.Log("making new parameters: " + random);


    }
}
