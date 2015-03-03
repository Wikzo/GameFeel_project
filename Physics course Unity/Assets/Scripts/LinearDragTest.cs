using UnityEngine;
using System.Collections;

public class LinearDragTest : MonoBehaviour
{

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 TerminalVelocity = new Vector3(0, -10, 0);

    public float dragConstant = 0.2f; // should be positive

    // linear drag --> terminal velocity --> acceleration goes towards 0

    // F_linearDrag =−C_linear drag *v

    // Update is called once per frame
    private void Update()
    {

        // Force_linearDrag = -C_dragConstant * velocity

        // mg +(-Force_linearDrag) = 0
        // -mg+(-C_dragConstant * velocity)
        // Force_linearDrag = = mg
        // C_dragConstant * velocity = mg
        // Velocity = mg / C_dragConstant

        // add gravity
        transform.position += velocity*Time.fixedDeltaTime;
        Vector3 force = Physics.gravity * rigidbody.mass;

        // add linear drag - dependent on the velocity (which is dependent on gravity force, which is dependent on mass)
        Vector3 dragForce = -dragConstant*velocity;
        force += dragForce;

        // combine
        acceleration = force / rigidbody.mass;
        velocity = velocity + acceleration * Time.fixedDeltaTime;

        // -------- Kraus' answer: ---------------
        /*
         * var gravityForce = Physics.gravity*rigidbody.mass;
        var linearDragConst = 10f;
        var linearDragForce = -linearDragConst*rigidbody.velocity;
        rigidbody.AddForce(gravityForce);
        rigidbody.AddForce(linearDragForce);
        Debug.Log(rigidbody.velocity);
         * */

        // Terminal velocity is the highest velocity attainable by an object in free fall. 
        // It occurs once the sum of the drag force (Fd) and buoyancy equals the downward force of gravity
        // (FG) acting on the object. Since the net force on the object is zero, the object
        // has zero acceleration.
         
    }
}