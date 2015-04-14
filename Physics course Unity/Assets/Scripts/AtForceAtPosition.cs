using UnityEngine;
using System.Collections;

public class AtForceAtPosition : MonoBehaviour
{
    public Vector3 OffsetForWorldPos;
    public GameObject UpForcePosition;

    private Transform _upTransform;
    private Rigidbody _rigidbody;

    public Vector3 Force;

    // Use this for initialization
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _upTransform = UpForcePosition.transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 force = -Physics.gravity * _rigidbody.mass;

        force = Force;

        // local position (correct)
        _rigidbody.AddForceAtPosition(force, _upTransform.position);

        // transform from local to world (correct)
        //_rigidbody.AddForceAtPosition(force, transform.TransformPoint(new Vector3(1,1,0)));
        
        // world position (incorrect)
        //_rigidbody.AddForceAtPosition(force, transform.position + OffsetForWorldPos);
    }
}
