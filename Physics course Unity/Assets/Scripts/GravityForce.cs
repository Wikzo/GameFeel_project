using UnityEngine;
using System.Collections;

public class GravityForce : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 TerminalVelocity = new Vector3(0,-10,0);

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += velocity*Time.fixedDeltaTime;

        // Newton N = kg * (m/s) / s --> kg*m/s^2
        // force required to accelerate one kg in one second by 1 m/s

        // F_gravity =−g * me *up
        // F = m*a
        // a = F/m

        Vector3 force = Physics.gravity*rigidbody.mass;
        acceleration = force/rigidbody.mass;

        velocity = velocity + acceleration*Time.fixedDeltaTime;

        if (velocity.y <= TerminalVelocity.y)
            velocity = TerminalVelocity;
    }
}
