using System;
using UnityEngine;
using System.Collections;

// http://mpolney.galineer.com/smb.html
using UnityEngine.UI;
using Random = UnityEngine.Random;


public enum HorizontalMovementState
{
    StandingStill,
    HitWallStop,
    AttackingLeft,
    AttackingRight,
    SustainLeft,
    SustainRight,
    ReleaseLeft,
    ReleaseRight
}


public class Player : MonoBehaviour
{
    public bool UseRandomValuesAtStart = false;
    public bool DrawDebugMenu = true;
    public GUIStyle DebugGUIStyle;
    public TweakableParameters MyTweakableParameters;

    public AnimationCurve[] HorizontalVelocityCurvesAttack;
    public AnimationCurve[] HorizontalVelocityCurvesRelease;

    public LayerMask PlatformMask;

    // alisasing some standard data
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider2D;

    // acceleration/deacceleration (attack/release)
    private float _currentAttackTime = 0;
    private float _currentReleaseTime = 0;
    private float _targetDeacceleration = 0;
    private float _maxAcceleration, _currentAcceleration, _targetAcceleration = 0;

    // animation stuff
    private Animator _animator;
    private float _animationPlaybackSpeed = 1f;

    // behind-the-scenes state data
    private CollisionState _collisionState;
    private Vector3 _velocity = new Vector3(0, 0, 0);
    private HorizontalMovementState _currentHorizontalMovementState;
    private HorizontalMovementState _previousHorizontalMovementState;
    private float _currentGhostJumpTime;
    private int _signLast, _signCurrent;
    private bool canReleaseEarly = true;
    private float _gravityMultiplier = 1;
    private bool _jumpHitApex = false;


    // collision detection via rays
    private float _verticalDistanceBetweenRays, _horizontalDistanceBetweenRays;
    private float SkinWidth = 0.001f;//0.001f; // change this if character gets stuck in geometry (default = 0.02)
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;
    private Vector3 _raycastTopLeft, _raycastBottomRight, _raycastBottomLeft;
    
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

        _animator = this.GetComponent<Animator>();
    }

    void Start()
    {
        if (UseRandomValuesAtStart)
            ChangeParameters();
    }

    void ChangeParameters()
    {

        //Debug.Log(ParameterManager.Instance.MyParameters[myIndex].TerminalVelocity);

        MyTweakableParameters = ParameterManager.Instance.MyParameters[ParameterManager.Instance.Index];

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Creating 50 new params");
            ParameterManager.Instance.MakeParameters(50);
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (ParameterManager.Instance.Index + 1 < ParameterManager.Instance.MyParameters.Count)
                ParameterManager.Instance.Index++;
            else
                ParameterManager.Instance.Index = 0;

            ChangeParameters();

        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (ParameterManager.Instance.Index - 1 >= 0)
                ParameterManager.Instance.Index--;
            else
                ParameterManager.Instance.Index = ParameterManager.Instance.MyParameters.Count;

            ChangeParameters();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Application.loadedLevelName == "PlayerPrefab")
                Application.LoadLevel("Duplicate");
            else if (Application.loadedLevelName == "Duplicate")
                Application.LoadLevel("Duplicate2");
        }


        CheckInput();
    }

    private void LateUpdate()
    {
        ApplyGravity();
        Move(_velocity * Time.deltaTime);

        if (MyTweakableParameters.UseAnimation)
            PlayAnimation();

    }

    void CheckInput()
    {
        InputJump();
        //InputLeftRight();
        //InputLeftRightOld();
        //InputHorizontalDirections();

        MovementStates();
        if (MyTweakableParameters.UseGroundFriction)
        {
            if (_collisionState.IsGrounded)
                _velocity.x += -_velocity.x * MyTweakableParameters.GroundFrictionPercentage/100;
        }

        if (!_collisionState.IsGrounded && MyTweakableParameters.UseAirFriction)
            _velocity.x *= MyTweakableParameters.AirFrictionHorizontal;

        if (_velocity.x > MyTweakableParameters.MaxVelocityX)
            _velocity.x = MyTweakableParameters.MaxVelocityX;
        else if (_velocity.x < -MyTweakableParameters.MaxVelocityX)
            _velocity.x = -MyTweakableParameters.MaxVelocityX;

        //transform.position += new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime;

    }

    

    void InputJump()
    {
        if (_collisionState.IsGrounded)
        {
            canReleaseEarly = true;
            _gravityMultiplier = 1;
            _jumpHitApex = false;

        }

        if ((_collisionState.IsGrounded || _currentGhostJumpTime > 0) && Input.GetKeyDown(KeyCode.Space))
        {
            _velocity.y = MyTweakableParameters.JumpPower;
            _currentGhostJumpTime = -1;
        }

        else if (!_collisionState.IsGrounded)
        {
            // release early?
            if (!Input.GetKey(KeyCode.Space))
            {
                if ((_velocity.y < MyTweakableParameters.JumpPower - MyTweakableParameters.MinimumJumpHeight) && canReleaseEarly && !_jumpHitApex)
                {
                    //Debug.Log(_velocity.y);
                    _velocity.y = MyTweakableParameters.ReleaseEarlyJumpVelocity;

                    canReleaseEarly = false;
                }
            }
            // apply gravity multiplier when hitting jump apex
            if (_velocity.y <= 0)
            {
                _jumpHitApex = true;
                _gravityMultiplier = MyTweakableParameters.ApexGravityMultiplier;
            }
        }

    }

    

    void PlayAnimation()
    {
        if (!_collisionState.IsGrounded)
            _animator.SetBool("IsJumping", true);
        else
            _animator.SetBool("IsJumping", false);

        _animator.SetFloat("Velocity", _velocity.x);



        _animationPlaybackSpeed = NormalizationMap(Mathf.Abs(_velocity.x), 0, MyTweakableParameters.MaxVelocityX, 0f, MyTweakableParameters.AnimationMaxSpeed);
        _animator.speed = _animationPlaybackSpeed;
    }

    private float moveSpeedX = 10;

    public static bool NearlyEqual(float f1, float f2)
    {
        // Equal if they are within 0.00001 of each other
        return Math.Abs(f1 - f2) < 0.05;
    }

    private bool useTurnMultiplier = false;
    private int _currentDirection = 0;
    private int _previousDirection = 0;
    private bool _changedDirection = false;
    private bool _isFacingRight;

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
        _isFacingRight = transform.localScale.x > 0;
    }

    void MovementStates()
    {

        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && NearlyEqual(_velocity.x, 0f))
        {
            _currentHorizontalMovementState = HorizontalMovementState.StandingStill;
            _currentReleaseTime = 0;

            _currentAttackTime = 0;//NormalizationMap(0, -AttackTime, AttackTime, -1, 1);

            _velocity.x = 0;

            //Debug.Log("Reset speed");
        }


        if (NearlyEqual(_velocity.y, 0f))
            _velocity.y = 0;



        // http://www.calculatorsoup.com/calculators/physics/velocity_a_t.php
        // Given _maxDeacceleration, _currentDeacceleration and ReleaseTime calculate _targetDeacceleration
        // Given velocity, initial velocity and time calculate the acceleration.
        // _targetDeacceleration = (_maxDeacceleration - _currentDeacceleration)/ReleaseTime



        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) // right key DOWN
        {
            _currentDirection = 1;

            if (!_isFacingRight)
                Flip();
            
            _currentReleaseTime = 0;

            switch (_currentHorizontalMovementState)
            {
                // begin attack
                case HorizontalMovementState.StandingStill:
                case HorizontalMovementState.AttackingRight:
                case HorizontalMovementState.ReleaseRight:
                case HorizontalMovementState.AttackingLeft:
                case HorizontalMovementState.SustainLeft:
                case HorizontalMovementState.ReleaseLeft:
                {

                    _currentHorizontalMovementState = HorizontalMovementState.AttackingRight;


                    //if (ChangedDirection())
                      //  _currentAttackTime += 2 * NormalizationMap(TurnAroundBoostPercent, 0, 1, 0, AttackTime);


                    if (_previousHorizontalMovementState == HorizontalMovementState.StandingStill
                        || _previousHorizontalMovementState == HorizontalMovementState.ReleaseRight)
                    {
                        _currentAcceleration = _velocity.x;
                        _maxAcceleration = MyTweakableParameters.MaxVelocityX;
                        _targetAcceleration = (_maxAcceleration - _currentAcceleration) / MyTweakableParameters.AttackTime;

                        

                    }

                    if (MyTweakableParameters.UseCurveForHorizontalAttackVelocity)
                    {
                        _currentAttackTime += Time.deltaTime;


                        if (ChangedDirection())
                            _currentAttackTime += MyTweakableParameters.AttackTime * MyTweakableParameters.TurnAroundBoostPercent / 100;

                        float valueScaled = 0;
                        if (_currentAttackTime < 0) // left
                        {
                            float timeScaled = NormalizationMap(_currentAttackTime, 0, -MyTweakableParameters.AttackTime, 0, 1);
                            float valueOriginal =
                                HorizontalVelocityCurvesAttack[0].Evaluate(timeScaled);

                            valueScaled = valueOriginal * -MyTweakableParameters.MaxVelocityX;
                        }

                        else if (_currentAttackTime > 0) // right 
                        {
                            float timeScaled = NormalizationMap(_currentAttackTime, 0, MyTweakableParameters.AttackTime, 0, 1);
                            float valueOriginal =
                                HorizontalVelocityCurvesAttack[0].Evaluate(timeScaled);

                            valueScaled = valueOriginal * MyTweakableParameters.MaxVelocityX;
                                // = NormalizationMap(valueOriginal, 0, 1, -MaxVelocityX, MaxVelocityX);
                        }
                        //Debug.Log("right time: " + _currentAttackTime + "; value: " + valueScaled);

                        _velocity.x = valueScaled;
                    }
                    else
                        _velocity.x += _targetAcceleration * Time.deltaTime;

                    // begin sustain
                    if (_velocity.x >= MyTweakableParameters.MaxVelocityX)
                    {
                        _velocity.x = MyTweakableParameters.MaxVelocityX;
                        _currentAttackTime = MyTweakableParameters.AttackTime;
                        _currentHorizontalMovementState = HorizontalMovementState.SustainRight;
                    }
                    break;
                }

                case HorizontalMovementState.SustainRight:
                {
                    _velocity.x = MyTweakableParameters.MaxVelocityX;
                    break;
                }
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) // left key DOWN
        {
            _currentDirection = -1;
            if (_isFacingRight)
                Flip();

            _currentReleaseTime = 0;

            switch (_currentHorizontalMovementState)
            {
                // begin attack
                case HorizontalMovementState.StandingStill:
                case HorizontalMovementState.AttackingLeft:
                case HorizontalMovementState.ReleaseLeft:
                case HorizontalMovementState.AttackingRight:
                case HorizontalMovementState.SustainRight:
                case HorizontalMovementState.ReleaseRight:
                    {

                        _currentHorizontalMovementState = HorizontalMovementState.AttackingLeft;


                        //if (ChangedDirection())
                          //  _currentAttackTime -= 2*NormalizationMap(TurnAroundBoostPercent, 0, 1, 0, AttackTime);



                        if (_previousHorizontalMovementState == HorizontalMovementState.StandingStill
                            || _previousHorizontalMovementState == HorizontalMovementState.ReleaseLeft)
                        {
                            _currentAcceleration = _velocity.x;
                            _maxAcceleration = MyTweakableParameters.MaxVelocityX;
                            _targetAcceleration = (_maxAcceleration - _currentAcceleration) / MyTweakableParameters.AttackTime;

                        }


                        if (MyTweakableParameters.UseCurveForHorizontalAttackVelocity)
                        {

                            _currentAttackTime -= Time.deltaTime;

                            if (ChangedDirection())
                                _currentAttackTime -= MyTweakableParameters.AttackTime * MyTweakableParameters.TurnAroundBoostPercent / 100;

                            float valueScaled = 0;

                            if (_currentAttackTime > 0) // right
                            {
                                float timeScaled = NormalizationMap(_currentAttackTime, 0, MyTweakableParameters.AttackTime, 0, 1);
                                float valueOriginal =
                                    HorizontalVelocityCurvesAttack[0].Evaluate(timeScaled);

                                valueScaled = valueOriginal * MyTweakableParameters.MaxVelocityX;
                            }

                            else if (_currentAttackTime < 0) // left
                            {

                                float timeScaled = NormalizationMap(_currentAttackTime, 0, -MyTweakableParameters.AttackTime, 0, 1);
                                float valueOriginal =
                                    HorizontalVelocityCurvesAttack[0].Evaluate(timeScaled);

                                valueScaled = valueOriginal * -MyTweakableParameters.MaxVelocityX;
                                    // = NormalizationMap(valueOriginal, 0, 1, -MaxVelocityX, MaxVelocityX);
                            }
                            //Debug.Log("left time: " + _currentAttackTime + "; value: " + valueScaled);


                            _velocity.x = valueScaled;
                        }
                        else
                            _velocity.x -= _targetAcceleration * Time.deltaTime;


                        // begin sustain
                        if (_velocity.x <= -MyTweakableParameters.MaxVelocityX)
                        {
                            _velocity.x = -MyTweakableParameters.MaxVelocityX;
                            _currentAttackTime = -MyTweakableParameters.AttackTime;
                            _currentHorizontalMovementState = HorizontalMovementState.SustainLeft;
                        }
                        break;
                    }

                case HorizontalMovementState.SustainLeft:
                    {
                        _velocity.x = -MyTweakableParameters.MaxVelocityX;
                        break;
                    }
            }
        }

        if (!Input.GetKey(KeyCode.RightArrow) && _currentDirection == 1) // right key UP
        {
            switch (_currentHorizontalMovementState)
            {
                // begin release right
                case HorizontalMovementState.AttackingRight:
                case HorizontalMovementState.SustainRight:
                {
                    /*_currentAttackTime = 0;

                    _maxDeacceleration = 0;
                    _currentDeacceleration = _velocity.x;
                    _targetDeacceleration = (_maxDeacceleration - _currentDeacceleration)/ReleaseTime;*/

                    _currentHorizontalMovementState = HorizontalMovementState.ReleaseRight;

                    if (MyTweakableParameters.UseCurveForHorizontalReleaseVelocity)
                    {
                        _currentReleaseTime = MyTweakableParameters.ReleaseTime;
                        _targetDeacceleration = _velocity.x;
                    }
                    break;
                }

                case HorizontalMovementState.ReleaseRight:
                {
                    if (MyTweakableParameters.UseCurveForHorizontalReleaseVelocity)
                    {
                        if (_currentAttackTime >= 0)
                            _currentAttackTime -= Time.deltaTime;
                        else
                            _currentAttackTime = 0;


                        _currentReleaseTime -= Time.deltaTime;
                        float currentReleaseTimeNormalized = _currentReleaseTime / MyTweakableParameters.ReleaseTime;
                        _velocity.x = HorizontalVelocityCurvesRelease[0].Evaluate(currentReleaseTimeNormalized) * _targetDeacceleration;
                    }
                    else
                        _velocity.x += _targetDeacceleration * Time.deltaTime;

                    //if (NearlyEqual(_velocity.x, 0f))
                    if (_velocity.x <= 0)
                    {
                        _velocity.x = 0;
                        _currentAttackTime = NormalizationMap(0, -MyTweakableParameters.AttackTime, MyTweakableParameters.AttackTime, 0, 1);


                        _currentHorizontalMovementState = HorizontalMovementState.StandingStill;

                        _currentDirection = 0;

                    }


                    break;
                }
            }
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && _currentDirection == -1) // left key UP
        {
            switch (_currentHorizontalMovementState)
            {
                // begin release left
                case HorizontalMovementState.AttackingLeft:
                case HorizontalMovementState.SustainLeft:
                {
                    /*_currentAttackTime = 0;

                    _maxDeacceleration = 0;
                    _currentDeacceleration = _velocity.x;
                    _targetDeacceleration = (_maxDeacceleration - _currentDeacceleration)/ReleaseTime;*/

                    _currentHorizontalMovementState = HorizontalMovementState.ReleaseLeft;

                    if (MyTweakableParameters.UseCurveForHorizontalReleaseVelocity)
                    {
                        _currentReleaseTime = MyTweakableParameters.ReleaseTime;
                        _targetDeacceleration = _velocity.x;
                    }
                    break;
                }

                case HorizontalMovementState.ReleaseLeft:
                {
                    if (MyTweakableParameters.UseCurveForHorizontalReleaseVelocity)
                    {
                        if (_currentAttackTime <= 0)
                            _currentAttackTime += Time.deltaTime;
                        else
                            _currentAttackTime = 0;


                        _currentReleaseTime -= Time.deltaTime;
                        float currentReleaseTimeNormalized = _currentReleaseTime / MyTweakableParameters.ReleaseTime;
                        _velocity.x = HorizontalVelocityCurvesRelease[0].Evaluate(currentReleaseTimeNormalized)*
                                      _targetDeacceleration;
                    }
                    else
                        _velocity.x -= _targetDeacceleration * Time.deltaTime;

                    //if (NearlyEqual(_velocity.x, 0f))
                    if (_velocity.x >= 0)
                    {
                        _velocity.x = 0;
                        _currentAttackTime = NormalizationMap(0, -MyTweakableParameters.AttackTime, MyTweakableParameters.AttackTime, 0, 1);


                        _currentHorizontalMovementState = HorizontalMovementState.StandingStill;

                        _currentDirection = 0;
                    }


                    break;
                }
            }
        }

        _previousHorizontalMovementState = _currentHorizontalMovementState;
        _previousDirection = _currentDirection;

    }




    public static float NormalizationMap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (value - oldMin)*(newMax - newMin)/(oldMax - oldMin) + newMin;
    }

    private bool ChangedDirection()
    {

        // only do boost when changing frome one direction to another
        if (_currentHorizontalMovementState == HorizontalMovementState.StandingStill ||
            _previousHorizontalMovementState == HorizontalMovementState.StandingStill)
            return false;

        if (_previousHorizontalMovementState == HorizontalMovementState.AttackingLeft
            || _previousHorizontalMovementState == HorizontalMovementState.SustainLeft
            || _previousHorizontalMovementState == HorizontalMovementState.ReleaseLeft)
            _signLast = -1;
        else
            _signLast = 1;

        if (_currentHorizontalMovementState == HorizontalMovementState.AttackingLeft
            || _currentHorizontalMovementState == HorizontalMovementState.SustainLeft
            || _currentHorizontalMovementState == HorizontalMovementState.ReleaseLeft)
            _signCurrent = -1;
        else
            _signCurrent = 1;

        if (_signLast != _signCurrent && _collisionState.IsGrounded)
        {
            //Debug.Log("turnaround boost");
            _animator.SetTrigger("IsTurning");

            return true;
        }
        else
            return false;
    }


    void OnGUI()
    {
        if (!DrawDebugMenu)
            return;

        // show parameters
        GUILayout.BeginArea(new Rect(Screen.width - (Screen.width * 0.3f), 0, Screen.width * 0.3f, Screen.height * 0.8f));
        {
            GUILayout.BeginVertical(); // also can put width in here
            {
                GUILayout.Label(MyTweakableParameters.ToString(), DebugGUIStyle);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();



        GUI.Label(new Rect(10, 0, 180, 20), "Velocity: " + _velocity, DebugGUIStyle);
        GUI.Label(new Rect(10, 20, 180, 20), "State: " + _currentHorizontalMovementState, DebugGUIStyle);
        GUI.Label(new Rect(10, 40, 180, 20), "Index: " + ParameterManager.Instance.Index + " / " + ParameterManager.Instance.MyParameters.Count, DebugGUIStyle);
        GUI.Label(new Rect(10, 60, 180, 20), "Level: " + Application.loadedLevelName, DebugGUIStyle);
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

        if (!_collisionState.IsGrounded) // ghost jump timer when leaving ground
        {
            if (_currentGhostJumpTime > -1)
                _currentGhostJumpTime -= Time.deltaTime;
        }
        else if (_collisionState.IsGrounded) // on ground
        {
            _currentGhostJumpTime = MyTweakableParameters.GhostJumpTime;
        }

        //CorrectHorizontalPlacement(ref deltaMovement, true); // when hitting moving platforms
        //CorrectHorizontalPlacement(ref deltaMovement, false); // left



        

        //Debug.Log("vel: " + _velocity + "\ndelta: " + deltaMovement);


        //transform.position += new Vector3(deltaMovement.x, deltaMovement.y, 0);

        transform.Translate(deltaMovement, Space.World); // apply the movement


        if (Time.deltaTime > 0) // update velocity
            _velocity = deltaMovement / Time.deltaTime;


        // clamp velocities
        /*if (_velocity.y < -TerminalVelocity)
            _velocity.y = -TerminalVelocity;
        else if (_velocity.y > TerminalVelocity)
            _velocity.y = TerminalVelocity;

        if (_velocity.x < -MaxVelocityX)
            _velocity.x = -MaxVelocityX;
        else if (_velocity.x > MaxVelocityX)
            _velocity.x = MaxVelocityX;*/

        _velocity.x = Mathf.Min(_velocity.x, MyTweakableParameters.MaxVelocityX);

        if (_velocity.y < MyTweakableParameters.TerminalVelocity)
            _velocity.y = MyTweakableParameters.TerminalVelocity;


    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

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
        var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;

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
        _velocity += (MyTweakableParameters.Gravity * _gravityMultiplier) * Time.deltaTime;
    }

    void CalculateRayOrigins()
    {
        var sizeOfBoxCollider = new Vector2(_boxCollider2D.size.x * Mathf.Abs(_localScale.x), _boxCollider2D.size.y * Mathf.Abs(_localScale.y)) / 2;
        var centerOfBoxCollider = new Vector2(_boxCollider2D.center.x * _localScale.x, _boxCollider2D.center.y * _localScale.y);

        _raycastTopLeft = transform.position + new Vector3(centerOfBoxCollider.x - sizeOfBoxCollider.x + SkinWidth, centerOfBoxCollider.y + sizeOfBoxCollider.y - SkinWidth);
        _raycastBottomRight = transform.position + new Vector3(centerOfBoxCollider.x + sizeOfBoxCollider.x - SkinWidth, centerOfBoxCollider.y - sizeOfBoxCollider.y + SkinWidth);
        _raycastBottomLeft = transform.position + new Vector3(centerOfBoxCollider.x - sizeOfBoxCollider.x + SkinWidth, centerOfBoxCollider.y - sizeOfBoxCollider.y + SkinWidth);
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight) // for when platforms move into player
    {
        var halfWidth = (_boxCollider2D.size.x * _localScale.x) / 2f;
        var rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;

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
