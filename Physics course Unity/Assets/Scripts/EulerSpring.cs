using UnityEngine;
using System.Collections;

public enum EulerMethod
{
    Explicit,
    Midpoint,
    Modified,
    SemiImplicit
}

public class EulerSpring : MonoBehaviour
{

    public Vector3 velocity;
    public float Mass;
    public float SpringConstant;
    public EulerMethod MyEulerMethod;
    public Vector3 Anchor;

    // Update is called once per frame
    private void FixedUpdate()
    {
        switch (MyEulerMethod)
        {
            case EulerMethod.Explicit:
            {
                // explicit euler (inaccurate)

                var newPosition = transform.position + Time.fixedDeltaTime*velocity;
                velocity = velocity + Time.fixedDeltaTime*ComputeForce(transform.position)/Mass;

                transform.position = newPosition;

                break;
            }

            case EulerMethod.Midpoint:
            {
                // midpoint euler (more accurate/stable, little more expensive)

                var halfPosition = transform.position + Time.fixedDeltaTime/2.0f*velocity;
                var halfVelocity = velocity + Time.fixedDeltaTime/2.0f*ComputeForce(transform.position)/Mass;

                transform.position = transform.position + Time.fixedDeltaTime*halfVelocity;
                velocity = velocity + Time.fixedDeltaTime*ComputeForce(halfPosition)/Mass;

                break;
            }

            case EulerMethod.Modified:
            {
                // modified euler

                var originalAcceleration = ComputeForce(transform.position)/Mass;

                var tempPosition = transform.position + Time.fixedDeltaTime*velocity;
                var temptVelocity = velocity + Time.fixedDeltaTime*originalAcceleration;

                transform.position = transform.position + Time.fixedDeltaTime*(velocity + temptVelocity)/2f;
                velocity = velocity +
                           Time.fixedDeltaTime*(originalAcceleration + ComputeForce(tempPosition)/Mass)/2f;

                break;

            }
            case EulerMethod.SemiImplicit:
            {
                // semi-implicit euler (inaccurate)
                // "forget" the newPosition buffer
                // update velocity, then update position

                velocity = velocity + Time.fixedDeltaTime * ComputeForce(transform.position) / Mass;
                transform.position = transform.position + Time.fixedDeltaTime * velocity;

                break;
            }
        }
    }

    Vector3 ComputeForce(Vector3 pos)
    {
        // HOOKS SPRING

        /*// mine:
        // F_dampedSpring = -k * (|r| - r_0) * r/|r|

        var r = (transform.position - AnchorPoint);
        var dampedSpring = -SpringConstant * (r.magnitude - RestLength) * r.normalized;

        return dampedSpring;*/

        // Kraus:
        // easiets way is to anchor at origin and restLength of 0
        // F = -k *x(t)

        var damp = -SpringConstant*(pos - Anchor);
        return damp;

    }
}
