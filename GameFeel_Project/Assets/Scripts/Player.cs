using System;
using System.Linq;
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

    public Transform BallGraphic;

    public TweakableParameters MyTweakableParameters;

    public LayerMask PlatformMask;

    public Transform KillFloor;

    // alisasing some standard data
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider2D;
    private TrailRenderer _trailRenderer;

    // acceleration/deacceleration (attack/release)
    public float _currentAttackTime = 0;
    public float _currentReleaseTime = 0;
    public float _targetDeacceleration = 0;

    // animation stuff
    private Animator _animator;
    private float _animationPlaybackSpeed = 1f;
    public int _currentDirection = 0;
    private bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
    }

    //private FreezeFrames _freezeFrames;

    // behind-the-scenes state data
    public CollisionState _collisionState;
    public Vector3 _velocity = new Vector3(0, 0, 0);
    public HorizontalMovementState _currentHorizontalMovementState;
    private HorizontalMovementState _previousHorizontalMovementState;
    private float _currentGhostJumpTime;
    private int _signLast, _signCurrent;
    private bool canReleaseEarly = true;
    private float _gravityMultiplier = 1;
    private bool _jumpHitApex = false;

    private AudioSource _audioSource;
    public Vector3 StartPosition;
    public Vector3 CheckpointPosition;

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
        _audioSource = GetComponent<AudioSource>();
        _trailRenderer = GetComponent<TrailRenderer>();
        StartPosition = transform.position;
        CheckpointPosition = _transform.position;
    }

    public void ChangeParameters()
    {
        MyTweakableParameters = ParameterManager.Instance.MyParameters[ParameterManager.Instance.Index];

        //Debug.Log("Changed parameters (index: " + ParameterManager.Instance.Index + ")");
    }

    void Update()
    {
        if (_transform.position.y < KillFloor.position.y)
            Die();

        CheckInput();
    }

    private void LateUpdate()
    {
        if (Demographics.Instance.MyGameState != GameState.Playing)
            return;

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
        /*if (MyTweakableParameters.UseGroundFriction)
        {
            if (_collisionState.IsGrounded)
                _velocity.x += -_velocity.x * MyTweakableParameters.GroundFrictionPercentage/100;
        }*/
        
        if (!_collisionState.IsGrounded && MyTweakableParameters.UseAirFriction)
        {
            _velocity.x *= MyTweakableParameters.AirFrictionHorizontalPercentage/100;
            //_velocity.x += -_velocity.x * MyTweakableParameters.AirFrictionHorizontalPercentage / 100;
            //Debug.Log("use air friction");
        }

        if (_velocity.x > MyTweakableParameters.MaxVelocityX)
            _velocity.x = MyTweakableParameters.MaxVelocityX;
        else if (_velocity.x < -MyTweakableParameters.MaxVelocityX)
            _velocity.x = -MyTweakableParameters.MaxVelocityX;

        //transform.position += new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime;

    }

    private bool _playingJumpSound = false;
    public AudioClip JumpSound;
    public AudioClip DieSound;
    private bool _hasJumped;

    void InputJump()
    {
        // grounded
        if (_collisionState.IsGrounded)
        {
            canReleaseEarly = true;
            _gravityMultiplier = 1;
            _jumpHitApex = false;
            _audioSource.pitch = 1;
            _playingJumpSound = false;
            _audioSource.Stop();
            _hasJumped = false;


        }

        // initial jump button
        if ((_collisionState.IsGrounded || _currentGhostJumpTime > 0) && Input.GetKeyDown(KeyCode.Space))
        {
            _velocity.y = MyTweakableParameters.JumpPower;
            _currentGhostJumpTime = -1;
            _hasJumped = true;

            if (!_playingJumpSound)
            {
                _audioSource.pitch = 1;
                audio.PlayOneShot(JumpSound);
                _playingJumpSound = true;
            }


        }

        // continuing holding jump button
        if (!_collisionState.IsGrounded && Input.GetKey(KeyCode.Space) && _playingJumpSound)
        {

            //if (!_audioSource.isPlaying)
              //  _audioSource.Play();


            /*if (_audioSource.time >= JumpSound.length)
            {
                _playingJumpSound = false;
                _audioSource.Stop();
            }*/

            //if (_audioSource.pitch < 1.15f)
            //_audioSource.pitch += Time.deltaTime;
        }

        // release early
        if (!_collisionState.IsGrounded)
        {
            // release early?
            if (!Input.GetKey(KeyCode.Space))
            {
                _playingJumpSound = false;

                if ((_velocity.y < MyTweakableParameters.JumpPower - MyTweakableParameters.MinimumJumpHeight) && canReleaseEarly && !_jumpHitApex)
                {
                    //Debug.Log(_velocity.y);
                    if (_hasJumped) // don't apply if walking outside an edge (ONLY when jumping)
                        _velocity.y = MyTweakableParameters.ReleaseEarlyJumpVelocity;

                    canReleaseEarly = false;
                }
            }
            // apply gravity multiplier when hitting jump apex
            if (_velocity.y <= 0)
            {
                //_audioSource.Stop();
                _playingJumpSound = false;
                _jumpHitApex = true;
                _gravityMultiplier = MyTweakableParameters.ApexGravityMultiplier;
            }


        }

    }


    void PlayAnimation()
    {
        BallGraphic.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * _velocity.x * -MyTweakableParameters.AnimationMaxSpeed);

        return;

        // old animation stuff for Mario sprite ...
        if (!_collisionState.IsGrounded)
            _animator.SetBool("IsJumping", true);
        else
            _animator.SetBool("IsJumping", false);

        _animator.SetFloat("Velocity", _velocity.x);


        _animationPlaybackSpeed = NormalizationMap(Mathf.Abs(_velocity.x), 0, MyTweakableParameters.MaxVelocityX, 0f, MyTweakableParameters.AnimationMaxSpeed);
        _animator.speed = _animationPlaybackSpeed;
    }


    public static bool NearlyEqual(float f1, float f2)
    {
        // Equal if they are within 0.00001 of each other
        return Math.Abs(f1 - f2) < 0.05;
    }

    

    private void Flip()
    {
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
        BallGraphic.localScale = new Vector3(-BallGraphic.localScale.x, BallGraphic.localScale.y, BallGraphic.localScale.y);

        _isFacingRight = BallGraphic.transform.localScale.x > 0;

        //Debug.Log("flip");
        _velocity.x *= 100000;

    }

    private float _initialVelocity;
    private float _finalVelocity;
    private float _acceleration;

    void MovementStates()
    {


        var deltaTime = Time.deltaTime;

        // RIGHT
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) // right key DOWN
        {
            

            // FIND TARGET ACCELERATION
            if (_currentHorizontalMovementState != HorizontalMovementState.AttackingRight
                && _currentHorizontalMovementState != HorizontalMovementState.SustainRight)
            {
                // http://www.calculatorsoup.com/calculators/physics/velocity_a_t.php
                // a = (v - v0)/t

                // v0 = initial velocity
                // v = final velocity
                // a = acceleration
                // t = time

                _initialVelocity = _velocity.x;
                _finalVelocity = MyTweakableParameters.MaxVelocityX;
                _acceleration = (_finalVelocity - _initialVelocity)/MyTweakableParameters.AttackTime;

                _currentHorizontalMovementState = HorizontalMovementState.AttackingRight;
            }

            // ATTACKING RIGHT
            else if (_currentHorizontalMovementState == HorizontalMovementState.AttackingRight)
            {
                _velocity.x += _acceleration*deltaTime;

                // BEGIN SUSTAINING RIGHT
                if (_velocity.x >= MyTweakableParameters.MaxVelocityX)
                {
                    _velocity.x = MyTweakableParameters.MaxVelocityX;
                    _currentHorizontalMovementState = HorizontalMovementState.SustainRight;
                }
            }
            
            // SUSTAINING RIGHT
            else if (_currentHorizontalMovementState == HorizontalMovementState.SustainRight)
            {
                // GO BACK TO ATTACK RIGHT? (due to e.g. air friction)
                if (_velocity.x < MyTweakableParameters.MaxVelocityX && !MyTweakableParameters.KeepGroundMomentumAfterJump)
                {
                    _currentHorizontalMovementState = HorizontalMovementState.AttackingRight;
                    return;
                }

                _velocity.x = MyTweakableParameters.MaxVelocityX;
            }
        }
        // LEFT
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) // left key DOWN
        {
            // FIND TARGET ACCELERATION
            if (_currentHorizontalMovementState != HorizontalMovementState.AttackingLeft
                && _currentHorizontalMovementState != HorizontalMovementState.SustainLeft) 
            {
                // http://www.calculatorsoup.com/calculators/physics/velocity_a_t.php
                // a = (v - v0)/t

                // v0 = initial velocity
                // v = final velocity
                // a = acceleration
                // t = time

                _initialVelocity = _velocity.x;
                _finalVelocity = -MyTweakableParameters.MaxVelocityX;
                _acceleration = (_finalVelocity - _initialVelocity) / MyTweakableParameters.AttackTime;

                _currentHorizontalMovementState = HorizontalMovementState.AttackingLeft;
            }

            // ATTACKING LEFT
            else if (_currentHorizontalMovementState == HorizontalMovementState.AttackingLeft)
            {
                _velocity.x += _acceleration * deltaTime;

                // BEGIN SUSTAINING LEFT
                if (_velocity.x <= -MyTweakableParameters.MaxVelocityX)
                {
                    _velocity.x = -MyTweakableParameters.MaxVelocityX;
                    _currentHorizontalMovementState = HorizontalMovementState.SustainLeft;
                }
            }

            // SUSTAINING LEFT
            else if (_currentHorizontalMovementState == HorizontalMovementState.SustainLeft)
            {
                // GO BACK TO ATTACK LEFT? (due to e.g. air friction)
                if (_velocity.x > -MyTweakableParameters.MaxVelocityX && !MyTweakableParameters.KeepGroundMomentumAfterJump)
                {
                    _currentHorizontalMovementState = HorizontalMovementState.AttackingLeft;
                    return;
                }

                _velocity.x = -MyTweakableParameters.MaxVelocityX;

            }
        }
        // RELEASE
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) // release
        {
            // FIND TARGET DEACCELERATION
            if (_currentHorizontalMovementState != HorizontalMovementState.StandingStill
                && _currentHorizontalMovementState != HorizontalMovementState.ReleaseRight
                && _currentHorizontalMovementState != HorizontalMovementState.ReleaseLeft)
            {
                // http://www.calculatorsoup.com/calculators/physics/velocity_a_t.php
                // a = (v - v0)/t

                // v0 = initial velocity
                // v = final velocity
                // a = acceleration
                // t = time

                _initialVelocity = MyTweakableParameters.MaxVelocityX;
                _finalVelocity = 0;
                _acceleration = (_finalVelocity - _initialVelocity) / MyTweakableParameters.ReleaseTime;

                if (_velocity.x > 0)
                    _currentHorizontalMovementState = HorizontalMovementState.ReleaseRight;
                else
                    _currentHorizontalMovementState = HorizontalMovementState.ReleaseLeft;
            }

            // RELEASING
            if (_currentHorizontalMovementState == HorizontalMovementState.ReleaseRight
                || _currentHorizontalMovementState == HorizontalMovementState.ReleaseLeft)
            {
                bool stop = false; // to find the correct "stop threshold" (over or under 0?)

                // right
                if (_currentHorizontalMovementState == HorizontalMovementState.ReleaseRight)
                {
                    stop = _velocity.x <= 0 ? true : false;

                    if (!stop)
                    _velocity.x += _acceleration * deltaTime;

                    // clamp, so won't move backwards if release time is very small
                    _velocity.x = Mathf.Clamp(_velocity.x, 0, MyTweakableParameters.MaxVelocityX);

                }
                // left
                else if (_currentHorizontalMovementState == HorizontalMovementState.ReleaseLeft)
                {
                    stop = _velocity.x >= 0 ? true : false;
                    if (!stop)
                        _velocity.x -= _acceleration * deltaTime;

                    // clamp, so won't move backwards if release time is very small
                    _velocity.x = Mathf.Clamp(_velocity.x, -MyTweakableParameters.MaxVelocityX, 0);


                }

                //Debug.Log("release acc: " + _acceleration * deltaTime);
                //Debug.Log("release velocity: " + _velocity);

                if (stop) // standing still
                {
                    _velocity.x = 0;

                    _currentHorizontalMovementState = HorizontalMovementState.StandingStill;
                }
            }
        }
        
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && NearlyEqual(_velocity.x, 0f))
        {
            //Debug.Log("backup stop");
            _velocity.x = 0;
            _currentHorizontalMovementState = HorizontalMovementState.StandingStill;
        }
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
            //_animator.SetTrigger("IsTurning");

            return true;
        }
        else
            return false;
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

        //_velocity.x = Mathf.Min(_velocity.x, MyTweakableParameters.MaxVelocityX);

        if (_velocity.x > MyTweakableParameters.MaxVelocityX)
            _velocity.x = MyTweakableParameters.MaxVelocityX;
        else if (_velocity.x < -MyTweakableParameters.MaxVelocityX)
            _velocity.x = -MyTweakableParameters.MaxVelocityX;

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

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Star")
        {
            Star s = coll.GetComponent<Star>();

            if (s != null)
                s.CollectStar();

        }
        else if (coll.tag == "Die")
            Die();
    }

    private int _deaths = 0;
    public GameObject DiePrefab;
    void Die()
    {
        _trailRenderer.time = 0;

        StateManager.Instance.DeathsOnThisLevel++;

        _velocity = Vector2.zero;
        _transform.position = CheckpointPosition;

        _trailRenderer.time = 1;

        Instantiate(DiePrefab);

    }

    public void Restart()
    {
        _trailRenderer.time = 0;
        _velocity = Vector2.zero;
        _transform.position = StartPosition;

        CheckpointPosition = StartPosition;

        StateManager.Instance.Restart();

        _trailRenderer.time = 2;

        Instantiate(FloatingTextRounds, transform.position, Quaternion.identity);

    }

    public GameObject FloatingTextRounds;

    public int StarsCollected;
    public AudioClip[] StarPickupSounds;
    private int _starPickupIndex = 0;

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
            //Debug.Log("Offset: " + offset);

        }


    }
}
