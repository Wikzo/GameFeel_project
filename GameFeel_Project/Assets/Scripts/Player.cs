﻿using System;
using UnityEngine;
using System.Collections;

// http://mpolney.galineer.com/smb.html
using UnityEngine.UI;


public enum HorizontalMovementState
{
    StandingStill,
    HitWallStop,
    Attacking_Left,
    Attacking_Right,
    Sustain_Left,
    Sustain_Right,
    Release_Left,
    Release_Right
}

public enum KeyInputState
{
    Right,
    Left,
    None
}

public class Player : MonoBehaviour
{
    // tweakable jump parameters
    public Vector3 Gravity = new Vector3(0, -1f,0);
    public float MaxVelocityY = -5f;
    public float MaxVelocityX = 15f;
    public float JumpPower = 10;
    public float ReducedHorizontalAirMovement = 0.5f;
    public float ReleaseTime = 0.4f;
    public float AttackTime = 0.4f;
    public bool UseCurveForHorizontalAttackVelocity = true;
    public bool UseCurveForHorizontalReleaseVelocity = true;

    [Range(0f, 1f)] public float TurnAroundBoost = 0f;
    public AnimationCurve[] HorizontalVelocityCurvesAttack;
    public AnimationCurve[] HorizontalVelocityCurvesRelease;

    public LayerMask PlatformMask;

    // alisasing some standard data
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider2D;

    private float _airDrag = 1;

    // acceleration/deacceleration (attack/release)
    public float _currentAttackTime = 0;
    public float _currentReleaseTime = 0;
    private float _maxDeacceleration, _currentDeacceleration, _targetDeacceleration = 0;
    private float _maxAcceleration, _currentAcceleration, _targetAcceleration = 0;


    // behind-the-scenes state data
    private CollisionState _collisionState;
    private Vector3 _velocity = new Vector3(0, 0, 0);
    private HorizontalMovementState _currentHorizontalMovementState;
    private HorizontalMovementState _previousHorizontalMovementState;

    // collision detection via rays
    private float _verticalDistanceBetweenRays, _horizontalDistanceBetweenRays;
    private float SkinWidth = 0.001f;//0.001f; // change this if character gets stuck in geometry (default = 0.02)
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;
    private Vector3 raycastTopLeft, raycastBottomRight, raycastBottomLeft;
    
    void Awake()
    {
        _transform = transform;
        _localScale = transform.localScale;
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _collisionState = new CollisionState();

        // horizontal collision rays
        float colliderWidth = _boxCollider2D.size.x*Mathf.Abs(_localScale.x) - (2*SkinWidth);
        _horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

        // horizontal collision rays
        float colliderHeight = _boxCollider2D.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);

    }

    public float sum = 0;

    void Update()
    {
        CheckInput();

        sum = _currentAttackTime - (1-_currentReleaseTime);

    }

    private void LateUpdate()
    {
        ApplyGravity();
        Move(_velocity * Time.deltaTime);

    }

    public float Friction = 0.05f;
    void CheckInput()
    {
        InputJump();
        //InputLeftRight();
        //InputLeftRightOld();
        //InputHorizontalDirections();

        MovementStates();
        _velocity.x += -_velocity.x*Friction;
        

        if (_velocity.x > MaxVelocityX)
            _velocity.x = MaxVelocityX;
        else if (_velocity.x < -MaxVelocityX)
            _velocity.x = -MaxVelocityX;

        //transform.position += new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime;

    }

    void InputJump()
    {
        if (_collisionState.IsGrounded && Input.GetKeyDown(KeyCode.Space))
            _velocity.y = JumpPower;
    }

    private float moveSpeedX = 10;

    public static bool NearlyEqual(float f1, float f2)
    {
        // Equal if they are within 0.00001 of each other
        return Math.Abs(f1 - f2) < 0.05;
    }

    private bool useTurnMultiplier = false;

    void MovementStates()
    {
        if (NearlyEqual(_velocity.x, 0f))
        {
            _currentHorizontalMovementState = HorizontalMovementState.StandingStill;
            _currentReleaseTime = 0;

            _currentAttackTime = NormalizationMap(0, -AttackTime, AttackTime, -1, 1);
        }


        if (!_collisionState.IsGrounded)
            _airDrag = ReducedHorizontalAirMovement;

        // http://www.calculatorsoup.com/calculators/physics/velocity_a_t.php
        // Given _maxDeacceleration, _currentDeacceleration and ReleaseTime calculate _targetDeacceleration
        // Given velocity, initial velocity and time calculate the acceleration.
        // _targetDeacceleration = (_maxDeacceleration - _currentDeacceleration)/ReleaseTime



        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) // right key DOWN
        {
            _currentReleaseTime = 0;

            switch (_currentHorizontalMovementState)
            {
                // begin attack
                case HorizontalMovementState.StandingStill:
                case HorizontalMovementState.Attacking_Right:
                case HorizontalMovementState.Release_Right:
                case HorizontalMovementState.Attacking_Left:
                case HorizontalMovementState.Sustain_Left:
                case HorizontalMovementState.Release_Left:
                {

                    _currentHorizontalMovementState = HorizontalMovementState.Attacking_Right;


                    if (ChangedDirection())
                        _currentAttackTime += 2 * NormalizationMap(TurnAroundBoost, 0, 1, 0, AttackTime);


                    if (_previousHorizontalMovementState == HorizontalMovementState.StandingStill
                        || _previousHorizontalMovementState == HorizontalMovementState.Release_Right)
                    {
                        _currentAcceleration = _velocity.x;
                        _maxAcceleration = MaxVelocityX;
                        _targetAcceleration = (_maxAcceleration - _currentAcceleration) / AttackTime;

                        

                    }
                    /*if (ChangedDirection())
                    {
                        _currentAcceleration = _velocity.x;
                        _maxAcceleration = MaxVelocityX;
                        _targetAcceleration = (_maxAcceleration - _currentAcceleration) / AttackTime;
                    }*/

                    if (UseCurveForHorizontalAttackVelocity)
                    {
                        _currentAttackTime += Time.deltaTime;

                        float timeScaled = NormalizationMap(_currentAttackTime, -AttackTime, AttackTime, 0, 1);
                        float valueOriginal =
                            HorizontalVelocityCurvesAttack[0].Evaluate(timeScaled);

                        float valueScaled = NormalizationMap(valueOriginal, 0, 1, -MaxVelocityX, MaxVelocityX);

                        //if (ChangedDirection())
                          //  _currentAttackTime += 0.5f;

                        _velocity.x = valueScaled;
                    }
                    else
                        _velocity.x += _targetAcceleration*Time.deltaTime*_airDrag;

                    // begin sustain
                    if (_velocity.x >= MaxVelocityX)
                    {
                        _velocity.x = MaxVelocityX;
                        _currentAttackTime = AttackTime;
                        _currentHorizontalMovementState = HorizontalMovementState.Sustain_Right;
                    }
                    break;
                }

                case HorizontalMovementState.Sustain_Right:
                {
                    _velocity.x = MaxVelocityX;
                    break;
                }
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) // left key DOWN
        {
            _currentReleaseTime = 0;

            switch (_currentHorizontalMovementState)
            {
                // begin attack
                case HorizontalMovementState.StandingStill:
                case HorizontalMovementState.Attacking_Left:
                case HorizontalMovementState.Release_Left:
                case HorizontalMovementState.Attacking_Right:
                case HorizontalMovementState.Sustain_Right:
                case HorizontalMovementState.Release_Right:
                    {

                        _currentHorizontalMovementState = HorizontalMovementState.Attacking_Left;


                        if (ChangedDirection())
                            _currentAttackTime -= 2*NormalizationMap(TurnAroundBoost, 0, 1, 0, AttackTime);



                        if (_previousHorizontalMovementState == HorizontalMovementState.StandingStill
                            || _previousHorizontalMovementState == HorizontalMovementState.Release_Left)
                        {
                            _currentAcceleration = _velocity.x;
                            _maxAcceleration = MaxVelocityX;
                            _targetAcceleration = (_maxAcceleration - _currentAcceleration) / AttackTime;

                        }

                        /*if (ChangedDirection())
                        {
                            _currentAcceleration = _velocity.x;
                            _maxAcceleration = MaxVelocityX;
                            _targetAcceleration = (_maxAcceleration - _currentAcceleration) / AttackTime;
                        }*/

                        if (UseCurveForHorizontalAttackVelocity)
                        {

                            _currentAttackTime -= Time.deltaTime;

                            float timeScaled = NormalizationMap(_currentAttackTime, -AttackTime, AttackTime, 0, 1);
                            float valueOriginal =
                                HorizontalVelocityCurvesAttack[0].Evaluate(timeScaled);

                            float valueScaled = NormalizationMap(valueOriginal, 0, 1, -MaxVelocityX, MaxVelocityX);

                            //if (ChangedDirection())
                            //  _currentAttackTime -= 0.5f;

                            _velocity.x = valueScaled;
                        }
                        else
                            _velocity.x -= _targetAcceleration * Time.deltaTime * _airDrag;


                        // begin sustain
                        if (_velocity.x <= -MaxVelocityX)
                        {
                            _velocity.x = -MaxVelocityX;
                            _currentAttackTime = -AttackTime;
                            _currentHorizontalMovementState = HorizontalMovementState.Sustain_Left;
                        }
                        break;
                    }

                case HorizontalMovementState.Sustain_Left:
                    {
                        _velocity.x = -MaxVelocityX;
                        break;
                    }
            }
        }

        if (!Input.GetKey(KeyCode.RightArrow)) // right key UP
        {
            switch (_currentHorizontalMovementState)
            {
                // begin release right
                case HorizontalMovementState.Attacking_Right:
                case HorizontalMovementState.Sustain_Right:
                {
                    /*_currentAttackTime = 0;

                    _maxDeacceleration = 0;
                    _currentDeacceleration = _velocity.x;
                    _targetDeacceleration = (_maxDeacceleration - _currentDeacceleration)/ReleaseTime;*/

                    _currentHorizontalMovementState = HorizontalMovementState.Release_Right;

                    if (UseCurveForHorizontalReleaseVelocity)
                    {

                        _currentReleaseTime = ReleaseTime;
                        _targetDeacceleration = _velocity.x;
                    }
                    break;
                }

                case HorizontalMovementState.Release_Right:
                {
                    if (UseCurveForHorizontalReleaseVelocity)
                    {
                        if (_currentAttackTime >= 0)
                            _currentAttackTime -= Time.deltaTime;
                        else
                            _currentAttackTime = 0;


                        _currentReleaseTime -= Time.deltaTime;
                        float currentReleaseTimeNormalized = _currentReleaseTime / ReleaseTime;
                        _velocity.x = HorizontalVelocityCurvesRelease[0].Evaluate(currentReleaseTimeNormalized) * _targetDeacceleration;
                    }
                    else
                        _velocity.x += _targetDeacceleration * Time.deltaTime;

                    //if (NearlyEqual(_velocity.x, 0f))
                    if (_velocity.x <= 0)
                    {
                        _velocity.x = 0;
                        _currentAttackTime = NormalizationMap(0, -AttackTime, AttackTime, 0, 1);


                        _currentHorizontalMovementState = HorizontalMovementState.StandingStill;
                    }


                    break;
                }
            }
        }
        else if (!Input.GetKey(KeyCode.LeftArrow)) // left key UP
        {
            switch (_currentHorizontalMovementState)
            {
                // begin release right
                case HorizontalMovementState.Attacking_Left:
                case HorizontalMovementState.Sustain_Left:
                {
                    /*_currentAttackTime = 0;

                        _maxDeacceleration = 0;
                        _currentDeacceleration = _velocity.x;
                        _targetDeacceleration = (_currentDeacceleration - _maxDeacceleration) / ReleaseTime;
                        _targetDeacceleration *= -1; // invert*/

                        _currentHorizontalMovementState = HorizontalMovementState.Release_Left;

                        if (UseCurveForHorizontalReleaseVelocity)
                        {
                            _currentReleaseTime = ReleaseTime;
                            _targetDeacceleration = _velocity.x;
                        }

                        break;
                    }

                case HorizontalMovementState.Release_Left:
                    {

                        if (UseCurveForHorizontalReleaseVelocity)
                        {
                            if (_currentAttackTime <= 0)
                                _currentAttackTime += Time.deltaTime;
                            else
                                _currentAttackTime = 0;

                            _currentReleaseTime -= Time.deltaTime;
                            float currentReleaseTimeNormalized = _currentReleaseTime / ReleaseTime;
                            _velocity.x = HorizontalVelocityCurvesRelease[0].Evaluate(currentReleaseTimeNormalized) * _targetDeacceleration;
                        }
                        else
                            _velocity.x += _targetDeacceleration * Time.deltaTime;

                        //if (NearlyEqual(_velocity.x, 0f))
                        if (_velocity.x >= 0)
                        {
                            _velocity.x = 0;
                            _currentAttackTime = NormalizationMap(0, -AttackTime, AttackTime, 0, 1);
                            Debug.Log("left release equ");
                            _currentHorizontalMovementState = HorizontalMovementState.StandingStill;
                        }

                        break;
                    }
            }
        }

        _previousHorizontalMovementState = _currentHorizontalMovementState;

    }

    float NormalizationMap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (value - oldMin)*(newMax - newMin)/(oldMax - oldMin) + newMin;
    }

    bool ChangedDirection()
    {
        // only do boost when changing frome one direction to another
        if (_currentHorizontalMovementState == HorizontalMovementState.StandingStill ||
            _previousHorizontalMovementState == HorizontalMovementState.StandingStill)
            return false;

        if (_previousHorizontalMovementState == HorizontalMovementState.Attacking_Left
            || _previousHorizontalMovementState == HorizontalMovementState.Sustain_Left
            || _previousHorizontalMovementState == HorizontalMovementState.Release_Left)
            signLast = -1;
        else
            signLast = 1;

        if (_currentHorizontalMovementState == HorizontalMovementState.Attacking_Left
            || _currentHorizontalMovementState == HorizontalMovementState.Sustain_Left
            || _currentHorizontalMovementState == HorizontalMovementState.Release_Left)
            signCurrent = -1;
        else
            signCurrent = 1;

        if (signLast != signCurrent)
            return true;
        else
            return false;
    }

    private int signLast, signCurrent;

    void OnGUI()
    {
        GUI.Label(new Rect((int)Screen.width/2, (int)Screen.height/2-30, 200,200), "Velocity:\n" + _velocity);

        GUI.Label(new Rect((int)Screen.width / 2, (int)Screen.height / 2, 200, 200), "State: " + _currentHorizontalMovementState);
    }

    void Move(Vector2 deltaMovement)
    {
        var wasGrouned = _collisionState.IsCollidingBelow;
        _collisionState.Reset();

        // calculate collision checks
        CalculateRayOrigins();
        if (Mathf.Abs(_velocity.x) > 0.01f)
            MoveHorizontally(ref deltaMovement);
        MoveVertically(ref deltaMovement);


        //CorrectHorizontalPlacement(ref deltaMovement, true); // when hitting moving platforms
        //CorrectHorizontalPlacement(ref deltaMovement, false); // left



        

        //Debug.Log("vel: " + _velocity + "\ndelta: " + deltaMovement);


        //transform.position += new Vector3(deltaMovement.x, deltaMovement.y, 0);

        transform.Translate(deltaMovement, Space.World); // apply the movement


        if (Time.deltaTime > 0) // update velocity
            _velocity = deltaMovement / Time.deltaTime;


        // clamp velocities
        /*if (_velocity.y < -MaxVelocityY)
            _velocity.y = -MaxVelocityY;
        else if (_velocity.y > MaxVelocityY)
            _velocity.y = MaxVelocityY;

        if (_velocity.x < -MaxVelocityX)
            _velocity.x = -MaxVelocityX;
        else if (_velocity.x > MaxVelocityX)
            _velocity.x = MaxVelocityX;*/

        _velocity.x = Mathf.Min(_velocity.x, MaxVelocityX);
        _velocity.y = Mathf.Min(_velocity.y, MaxVelocityY);

        
    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? raycastBottomRight : raycastBottomLeft;

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit)
                continue;

            // we hit something

            deltaMovement.x = raycastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            // colliding right/left?
            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                //State.IsCollidingRight = true;

                //Debug.Log("Hit right");

                _currentHorizontalMovementState = HorizontalMovementState.StandingStill;


            }
            else
            {
                deltaMovement.x += SkinWidth;
                //State.IsCollidingLeft = true;

                //Debug.Log("Hit left");

                _currentHorizontalMovementState = HorizontalMovementState.StandingStill;


            }

            // stuck inside geometry?
            if (rayDistance < SkinWidth + 0.0001f) // overshoot?
                break;
        }
    }



    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? raycastTopLeft : raycastBottomLeft;

        rayOrigin.x += deltaMovement.x; // already calculated from MoveHorizontally

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < TotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit)
                continue;

            if (!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - raycastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    //StandingOn = raycastHit.collider.gameObject;
                }
            }

            deltaMovement.y = raycastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            // colliding above/below?
            if (isGoingUp)
            {
                deltaMovement.y -= SkinWidth;
                //State.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += SkinWidth;
               // State.IsCollidingBelow = true;
                _collisionState.IsGrounded = true; // FIX: IsGrounded put in manually
            }

            // stuck inside geometry?
            if (rayDistance < SkinWidth + 0.0001f)
                break;
        }
    }

    void ApplyGravity()
    {
        _velocity += Gravity * Time.deltaTime;
    }

    void CalculateRayOrigins()
    {
        var sizeOfBoxCollider = new Vector2(_boxCollider2D.size.x * Mathf.Abs(_localScale.x), _boxCollider2D.size.y * Mathf.Abs(_localScale.y)) / 2;
        var centerOfBoxCollider = new Vector2(_boxCollider2D.center.x * _localScale.x, _boxCollider2D.center.y * _localScale.y);

        raycastTopLeft = transform.position + new Vector3(centerOfBoxCollider.x - sizeOfBoxCollider.x + SkinWidth, centerOfBoxCollider.y + sizeOfBoxCollider.y - SkinWidth);
        raycastBottomRight = transform.position + new Vector3(centerOfBoxCollider.x + sizeOfBoxCollider.x - SkinWidth, centerOfBoxCollider.y - sizeOfBoxCollider.y + SkinWidth);
        raycastBottomLeft = transform.position + new Vector3(centerOfBoxCollider.x - sizeOfBoxCollider.x + SkinWidth, centerOfBoxCollider.y - sizeOfBoxCollider.y + SkinWidth);
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight) // for when platforms move into player
    {
        var halfWidth = (_boxCollider2D.size.x * _localScale.x) / 2f;
        var rayOrigin = isRight ? raycastBottomRight : raycastBottomLeft;

        if (isRight)
            rayOrigin.x -= (halfWidth - SkinWidth);
        else
            rayOrigin.x += (halfWidth + SkinWidth);

        var rayDirection = isRight ? Vector2.right : -Vector2.right;
        var offset = 0f;

        for (var i = 1; i < TotalHorizontalRays - 1; i++)
        {
            // deltaMovement is used, so rays won't be de-synced by 1 frame
            var rayVector = new Vector2(deltaMovement.x + rayOrigin.x, deltaMovement.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));

            Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.blue : Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
            if (!raycastHit)
                continue;

            // calculate displacement to move the player away from platform (inverse direction)
            // offset = (hitPoint-centerPoint) - halfWidthPoint
            offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));



        }

        deltaMovement.x += offset; // push player away from moving platform

        if (offset != 0)
        {
            _currentHorizontalMovementState = HorizontalMovementState.StandingStill;
            Debug.Log("Offset: " + offset);

        }



    }
}
