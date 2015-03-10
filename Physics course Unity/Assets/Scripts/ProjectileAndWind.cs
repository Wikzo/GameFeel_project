using UnityEngine;
using System.Collections;

public class ProjectileAndWind : MonoBehaviour
{
    // aerodynamic drag:
    // F_drag = -(1/2)*p*v^2*A*C_d*(v/|v)
    // p = density of fluid/air
    // v = relative velocity of object = v_obj - v_wind
    // A = area of object
    // C_d = drag constant (Reynolds number)
    
    // simpler way:
    // F_drag = -C * v^2 * (v/|v|)

    public Vector3 WindVelocity;
    public float DragConstant;
    public Vector3 MyVelocity;

    private Rigidbody _myRigidbody;

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var relativeVelocity = _myRigidbody.velocity - WindVelocity;
        var dragForce = -DragConstant * relativeVelocity.sqrMagnitude * relativeVelocity.normalized;

        MyVelocity = _myRigidbody.velocity;

        _myRigidbody.AddForce(dragForce);
    }
}
