using UnityEngine;
using System.Collections;

public class RotateAroundScript : MonoBehaviour
{
    public Vector3 AngularVelocity;
    private Transform _myTransform;

    // Use this for initialization
    private void Start()
    {
        _myTransform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        _myTransform.RotateAround(_myTransform.position,
            AngularVelocity,
            AngularVelocity.magnitude * Mathf.Rad2Deg * Time.deltaTime);
    }
}
