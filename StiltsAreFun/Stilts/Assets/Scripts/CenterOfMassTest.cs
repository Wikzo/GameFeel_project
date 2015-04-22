using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CenterOfMassTest : MonoBehaviour
{
    private Vector3 Offset;
    public GameObject MassObject;

    public Vector3 CenterOfMass;

    public Vector3 defaultCenterOfMass;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        defaultCenterOfMass = _rigidbody.centerOfMass;
    }

    void Update()
    {
        // from world to local pos
        Offset = transform.InverseTransformPoint(MassObject.transform.position);
        //Offset = transform.InverseTransformPoint(Offset);
        //MassObject.transform.position = Offset;


        _rigidbody.centerOfMass = Offset;
        CenterOfMass = _rigidbody.centerOfMass;

        //Debug.Log(rigidbody.worldCenterOfMass);
    }
}
