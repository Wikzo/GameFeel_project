using UnityEngine;
using System.Collections;

public class PrintVelocity : MonoBehaviour
{
    public float YVelocity;
    private SpringJoint SpringJoint;
    public float DistanceBetweenTwoPoints;

    // Use this for initialization
    void Start()
    {
        SpringJoint = GetComponent<SpringJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        YVelocity = rigidbody.velocity.y;
        DistanceBetweenTwoPoints = Mathf.Abs(SpringJoint.connectedAnchor.y - transform.position.y);

    }
}
