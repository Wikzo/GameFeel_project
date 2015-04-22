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
    public Vector3 Offset;

    public float OffsetSpeed = 0.01f;
    public float OffsetMax = 0.4f;

    private Vector3 COMStartPosition;

    public Vector3 StandardDownForce;
    public Vector3 _forceToAddLeft, _forceToAddRight;

    // -0.4f COM offset

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

        COMStartPosition = CenterOfMass.localPosition;
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
            //_forceToAddLeft += _leftFootTransform.up * UpForce; // relative

        }
        else if (Input.GetKey(KeyCode.S)) // left down
        {
            _forceToAddLeft += Vector3.up * -DownForce;
            //_forceToAddLeft += _leftFootTransform.up * -DownForce; // relative


        }
        // right up
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _forceToAddRight += Vector3.up*UpForce;
            //_forceToAddRight += _rightFootTransform.up*UpForce; // relative

            //Gizmos.DrawLine(_rightFootTransform.position, _rightFootTransform.up * UpForce);
        }
        else if (Input.GetKey(KeyCode.DownArrow)) // right down
        {
            _forceToAddRight += Vector3.up * -DownForce;
            //_forceToAddRight += _rightFootTransform.up * -DownForce; // relative


        }


        

        // Center of Mass offset
        if (Input.GetKey(KeyCode.Keypad2)) // forward
        {
            //if (Offset.z < OffsetMax)
                Offset.y += OffsetSpeed * Time.deltaTime;
          //else
            //Offset.z = OffsetMax;
        }
        else if (Input.GetKey(KeyCode.Keypad8)) // backward
        {
            //if (Offset.z > -OffsetMax)
                Offset.y -= OffsetSpeed * Time.deltaTime;//
                //else
            //   Offset.z = -OffsetMax;
        }
        CenterOfMass.localPosition = COMStartPosition + Offset;//

        _leftFootRigidBody.AddForce(_forceToAddLeft);
        //_leftHandRigidBody.AddForce(_forceToAddLeft);

        _rightFootRigidBody.AddForce(_forceToAddRight);
        //_rightHandRigidBody.AddForce(_forceToAddRight);


    }
}
