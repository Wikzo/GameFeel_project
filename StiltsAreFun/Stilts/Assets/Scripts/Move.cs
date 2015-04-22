using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{

    public Rigidbody LeftFootRigidbody, RightFootRigidBody;
    public float ForwardForce = 50;
    public float UpForce = 50;
    public float DownForce = 50;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // left forward
        if (Input.GetKey(KeyCode.LeftArrow))
            LeftFootRigidbody.AddForce(Vector3.forward*ForwardForce);

        // right forward
        if (Input.GetKey(KeyCode.RightArrow))
            RightFootRigidBody.AddForce(Vector3.forward * ForwardForce);

        // left up/down
        if (Input.GetKey(KeyCode.A))
            LeftFootRigidbody.AddForce(Vector3.up * UpForce);
        else if (Input.GetKey(KeyCode.S))
            LeftFootRigidbody.AddForce(Vector3.up * -DownForce);

        // right up/down
        if (Input.GetKey(KeyCode.UpArrow))
            RightFootRigidBody.AddForce(Vector3.up * UpForce);
        else if (Input.GetKey(KeyCode.DownArrow))
            RightFootRigidBody.AddForce(Vector3.up * -DownForce);
 


    }
}
