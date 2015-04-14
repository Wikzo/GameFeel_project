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
        Vector3 force;
        Vector3 rotation;

        force = Input.GetAxis("Horizontal")*Speed*Time.deltaTime * Vector3.forward;
        //rotation = Input.GetAxis("Horizontal")*Speed*Time.deltaTime * Vector3.forward;


        _rigidbody.AddTorque(force);
    }
}
