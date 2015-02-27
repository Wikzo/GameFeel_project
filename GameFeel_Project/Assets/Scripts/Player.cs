using System;
using UnityEngine;
using System.Collections;

// http://mpolney.galineer.com/smb.html

public enum State
{
    IsGrounded,
    IsInAir
}

public class Player : MonoBehaviour
{
    // tweakable jump parameters
    public Vector3 Gravity = new Vector3(0, -1f,0);
    public float MaxVelocityY = -5f;
    public float MaxVelocityX = 15f;
    public float JumpPower = 10;

    public LayerMask PlatformMask;

    // alisasing some standard data
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider2D;

    // behind-the-scenes state data
    private State _state;
    private Vector3 _velocity = new Vector3(0, 0, 0);

    // collision detection via rays
    private float _verticalDistanceBetweenRays, _horizontalDistanceBetweenRays;
    private const float SkinWidth = 0.02f;//0.001f; // change this if character gets stuck in geometry (default = 0.02)
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;
    private Vector3 raycastTopLeft, raycastBottomRight, raycastBottomLeft;
    
    void Awake()
    {
        _transform = transform;
        _localScale = transform.localScale;
        _boxCollider2D = GetComponent<BoxCollider2D>();

        // horizontal collision rays
        float colliderWidth = _boxCollider2D.size.x*Mathf.Abs(_localScale.x) - (2*SkinWidth);
        _horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

        // horizontal collision rays
        float colliderHeight = _boxCollider2D.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);

    }

    private void Update()
    {
        //ApplyGravity();

        CheckInput();
        //Move(_velocity * Time.deltaTime);

    }

    void CheckInput()
    {
        InputJump();
        InputLeftRight();

        if (_velocity.x > MaxVelocityX)
            _velocity.x = MaxVelocityX;
        else if (_velocity.x < -MaxVelocityX)
            _velocity.x = -MaxVelocityX;

        transform.position += new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime;

    }

    void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _velocity.y = JumpPower;
    }

    private float moveSpeedX = 2;

    public static bool NearlyEqual(float f1, float f2)
    {
        // Equal if they are within 0.00001 of each other
        return Math.Abs(f1 - f2) < 0.00001;
    }

    void InputLeftRight()
    {
        int movingDirection = 0;
        int lastMovingDirection = 0;

        // press
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (acc == false && acc2 == false)
            {
                acc = true;
                acc2 = true;

                v_acc = MaxVelocityX;
                v0_acc = _velocity.x;
                a_acc = (v_acc - v0_acc) / t_ac;
            }

            _velocity.x = a_acc * Time.deltaTime;
            movingDirection = 1;
            timer_acc += Time.deltaTime;

            if (_velocity.x >= MaxVelocityX)
            {
                _velocity.x = MaxVelocityX;

                if (acc)
                {
                    Debug.Log("Time to reach right acc: " + timer_acc);
                    timer_acc = 0;
                    acc = false;
                }

            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (acc == false && acc2 == false)
            {
                acc = true;
                acc2 = true;

                v_acc = -MaxVelocityX;
                v0_acc = _velocity.x;
                a_acc = (v_acc - v0_acc) / t_ac;
            }

            _velocity.x += a_acc * Time.deltaTime;
            movingDirection = -1;

            timer_acc += Time.deltaTime;

            if (_velocity.x <= -MaxVelocityX && acc)
            {
                _velocity.x = -MaxVelocityX;

                if (acc)
                {
                    Debug.Log("Time to reach left acc: " + timer_acc);
                    timer_acc = 0;
                    acc = false;
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.RightArrow) && Input.GetKeyUp(KeyCode.LeftArrow))
            acc2 = false;
        

        


        // http://www.calculatorsoup.com/calculators/physics/velocity_a_t.php
        // Given v_deacc, v0_deacc and t_deac calculate a_deacc
        // Given velocity, initial velocity and time calculate the acceleration.
        // a_deacc = (v_deacc - v0_deacc)/t_deac
        // release
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if (NearlyEqual(_velocity.x, 0f))
            {
                deacc = false;
                Debug.Log("still");
                return;
            }

            if (deacc == false)
            {
                deacc = true;

                v_deacc = 0;
                v0_deacc = _velocity.x;
                a_deacc = (v_deacc - v0_deacc)/t_deac;
            }

            if (deacc == true)
            {
                //if (_velocity.x > 0.01f) // going right
                  //  _velocity.x -= a_deacc*Time.deltaTime;
                if (_velocity.x < 0.01f)
                {
                    _velocity.x += a_deacc*Time.deltaTime;
                    timer_deacc += Time.deltaTime;
                }
                else
                {
                    _velocity.x = 0f;
                    deacc = false;
                    Debug.Log("Time to reach left deacc: "+ timer_deacc);
                    timer_deacc = 0;
                }
            }




        }
    }

    private float v_deacc, v0_deacc, a_deacc = 0;
    private float timer_deacc = 0;
    public float t_deac = 5;

    private bool deacc, acc, acc2;

    private float v_acc, v0_acc, a_acc = 0;
    private float timer_acc = 0;
    public float t_ac = 5;

    void OnGUI()
    {
        GUI.Label(new Rect((int)Screen.width/2, (int)Screen.height/2, 200,200), "Velocity Y:\n" + _velocity);
    }

    void Move(Vector2 deltaMovement)
    {
        // calculate collision checks
        CalculateRayOrigins();
        if (Mathf.Abs(_velocity.x) > 0.01f)
            MoveHorizontally(ref deltaMovement);
        MoveVertically(ref deltaMovement);


        if (Time.deltaTime > 0) // update velocity
            _velocity = deltaMovement / Time.deltaTime;

        // clamp velocities
        if (_velocity.y < -MaxVelocityY)
            _velocity.y = -MaxVelocityY;
        else if (_velocity.y > MaxVelocityY)
            _velocity.y = MaxVelocityY;

        if (_velocity.x < -MaxVelocityX)
            _velocity.x = -MaxVelocityX;
        else if (_velocity.x > MaxVelocityX)
            _velocity.x = MaxVelocityX;

        transform.position += new Vector3(deltaMovement.x, deltaMovement.y, 0);

        
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
            }
            else
            {
                deltaMovement.x += SkinWidth;
                //State.IsCollidingLeft = true;
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
                //State.IsGrounded = true; // FIX: IsGrounded put in manually
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
}
