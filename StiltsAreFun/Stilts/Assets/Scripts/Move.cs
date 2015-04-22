using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{

    public GameObject LeftFoot, RightFoot, LeftHand, RightHand;
    private Rigidbody _leftFootRigidBody, _rightFootRigidBody, _leftHandRigidBody, _rightHandRigidBody;
    private Transform _leftFootTransform, _rightFootTransform, _leftHandTransform, _rightHandTransform;
    public float ForwardForce = 50;
    public float UpForce = 50;
    public float DownForce = 50;

    public Transform CenterOfMass;
    //public Vector3 COMFrontPosition, COMBackPosition;

    ///
    //public Vector3 Offset;

    //private Vector3 COMStartPosition;

    public Vector3 StandardDownForce;
    public Vector3 _forceToAddLeft, _forceToAddRight;

    // Use this for initialization
    private void Start()
    {
        _leftFootTransform = LeftFoot.transform;
        _rightFootTransform = RightFoot.transform;
        _leftHandTransform = LeftHand.transform;
        _rightHandTransform = RightHand.transform;

        _leftFootRigidBody = LeftFoot.GetComponent<Rigidbody>();
        _rightFootRigidBody = RightFoot.GetComponent<Rigidbody>();
        _leftHandRigidBody = LeftHand.GetComponent<Rigidbody>();
        _rightHandRigidBody = RightHand.GetComponent<Rigidbody>();

        //COMStartPosition = CenterOfMass.localPosition;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _forceToAddLeft = StandardDownForce;
        _forceToAddRight = StandardDownForce;


        //Debug.Log("local: " + CenterOfMass.localPosition);
        //Debug.Log("standard pos: " +CenterOfMass.position);

        //Offset = Vector3.zero;

        // left forward
        if (Input.GetKey(KeyCode.A))
        {
            _forceToAddLeft += Vector3.forward*ForwardForce;
        }
        else if (Input.GetKey(KeyCode.D)) // left backward
        {
            _forceToAddLeft += Vector3.forward * -ForwardForce;
        }


        // right forward
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _forceToAddRight += Vector3.forward * ForwardForce;
        }
        // right backward
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _forceToAddRight += Vector3.forward * -ForwardForce;
        }

        // left up
        if (Input.GetKey(KeyCode.W))
        {
            _forceToAddLeft += Vector3.up*UpForce;

        }
        else if (Input.GetKey(KeyCode.S)) // left down
        {
            _forceToAddLeft += Vector3.up * -DownForce;

        }
        // right up
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _forceToAddRight += Vector3.up*UpForce;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) // right down
        {
            _forceToAddRight += Vector3.up * -DownForce;

        }


        _leftFootRigidBody.AddForce(_forceToAddLeft);
        _leftHandRigidBody.AddForce(_forceToAddLeft);

        _rightFootRigidBody.AddForce(_forceToAddRight);
        _rightHandRigidBody.AddForce(_forceToAddRight);


        //CenterOfMass.localPosition = COMStartPosition + Offset;


    }
}
