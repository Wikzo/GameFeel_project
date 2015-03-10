using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CenterOfMassTest : MonoBehaviour
{
    public Vector3 Offset;
    public GameObject MassObject;

    public Vector3 CenterOfMass;

    public Vector3 defaultCenterOfMass;

    void Start()
    {
        defaultCenterOfMass = rigidbody.centerOfMass;
    }

    void Update()
    {
        // from world to local pos
        Offset = transform.InverseTransformPoint(MassObject.transform.position);
        //Offset = transform.InverseTransformPoint(Offset);
        //MassObject.transform.position = Offset;


        rigidbody.centerOfMass = Offset;
        CenterOfMass = rigidbody.centerOfMass;

        //Debug.Log(rigidbody.worldCenterOfMass);
    }
}
