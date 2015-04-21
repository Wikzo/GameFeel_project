using UnityEngine;
using System.Collections;

public class BallAddForce : MonoBehaviour
{

    private Rigidbody _rigidbody;
    public float Speed = 10;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 force, force2;
        Vector3 rotation;

        force = Input.GetAxis("Vertical")*Speed*Time.deltaTime * transform.forward;
        force2 = Input.GetAxis("Horizontal")*Speed*Time.deltaTime * Vector3.up;
        //rotation = Input.GetAxis("Horizontal")*Speed*Time.deltaTime * Vector3.forward;


        _rigidbody.AddTorque(force);
        _rigidbody.AddTorque(force2);
    }
}
