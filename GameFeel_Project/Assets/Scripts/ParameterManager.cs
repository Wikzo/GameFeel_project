using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParameterManager : MonoBehaviour
{
    public List<TweakableParameters> MyParameters;

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
        MakeParameters(50);
        Debug.Log("making 5 default parameters");

    }

    public void MakeParameters(int size)
    {
        MyParameters = new List<TweakableParameters>(size);


        for (int i = 0; i < size; i++)
        {
            // Random.Range: min [inclusive], max [exclusive]

            float gravity = Random.Range(-5, -30.1f);
            float jumpPower = Random.Range(2, 20.1f);
            bool useAirFriction = true;
            float airFrictionHorizontal = Random.Range(0.1f, 1.1f);
            float terminalVelocity = Random.Range(gravity, -30.1f);
            float ghostJumpTime = Random.Range(0f, 2.1f);
            float minimumJumpHeight = Random.Range(0.1f, 5.1f);
            float releaseEarlyJumpVelocity = Random.Range(0f, 1.1f);
            float apexGravityMultiplier = Random.Range(1f, 5.1f);
            float maxVelocityX = Random.Range(1, 20.1f);
            bool useGroundFriction = false;
            float groundFriction = Random.Range(0f, 99.1f);
            float releaseTime = Random.Range(0.001f, 2.1f);
            float attackTime = Random.Range(0.001f, 2.1f);
            float turnAroundBoostPercent = Random.Range(0f, 200.1f);
            bool useCurveForHorizontalAttackVelocity = true;
            bool useCurveForHorizontalReleaseVelocity = true;
            bool useAnimation = true;
            float animationMaxSpeed = 1.4f;

            MyParameters.Add(new TweakableParameters(gravity, jumpPower, useAirFriction, airFrictionHorizontal, terminalVelocity, ghostJumpTime,
                minimumJumpHeight, releaseEarlyJumpVelocity, apexGravityMultiplier, maxVelocityX, useGroundFriction,
                groundFriction, releaseTime, attackTime, turnAroundBoostPercent, useCurveForHorizontalAttackVelocity, useCurveForHorizontalReleaseVelocity,
                useAnimation, animationMaxSpeed));
        }
    }
}
