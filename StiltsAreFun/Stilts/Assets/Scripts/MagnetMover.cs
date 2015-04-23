using UnityEngine;
using System.Collections;

public class MagnetMover : MonoBehaviour
{
    private Rigidbody _rb;
    public Transform Goal;
    public float MaxSpeed = 10f;
    public float Distance;

    public float CurrentSpeed;
    private Transform _transform;
    private SphereCollider _collider;

    private bool _pickedUp;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();

        _transform = gameObject.transform;

        _pickedUp = false;
    }

    void Update()
    {
        if (!_pickedUp)
        {
            Distance = Vector3.Distance(Goal.position, _transform.position);

            if (Distance < 1.18)
            {
                _rb.useGravity = false;
                _rb.isKinematic = true;
                _collider.isTrigger = true;

                _pickedUp = true;

            }
        }

        if (_pickedUp)
        {
            _transform.position = Goal.transform.position;            
        }

    }

    void FixedUpdate_OLD()
    {
        Distance = Vector3.Distance(Goal.position, _transform.position);

        if (Distance > 1.18)
            return;

        Vector3 dir = Goal.position - _transform.position;
        CurrentSpeed = Mathf.Max(5, MaxSpeed - Distance);
        _rb.AddForce(dir * CurrentSpeed * Time.fixedDeltaTime);
    }
}
