using UnityEngine;
using System.Collections;

public class CollisionState
{

    // colliders
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }
    public bool IsGrounded { get; set; }

    public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }

    public void Reset()
    {
        // sets all booleans to false
        // FIX: IsGrounded put in manually
        IsCollidingRight = IsCollidingLeft = IsCollidingAbove = IsCollidingBelow = IsGrounded = false;

    }

    public override string ToString()
    {
        return string.Format("(Controller: r:{0}, l:{1}, a:{2}, b:{3})",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingAbove,
            IsCollidingBelow);
    }

}