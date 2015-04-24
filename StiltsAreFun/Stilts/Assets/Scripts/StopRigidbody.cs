using UnityEngine;
using System.Collections;

public class StopRigidbody : MonoBehaviour
{
    private Rigidbody _rb;
    // Use this for initialization
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _rb.angularVelocity = Vector3.zero;
            _rb.velocity = Vector3.zero;
        }
    }
}
