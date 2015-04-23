using UnityEngine;
using System.Collections;

public class MoveHands : MonoBehaviour
{
    public Transform Root;
    public Vector3 RotateAxis;
    public float RotateSpeed = 10f;

    public Transform LeftHand, RightHand;
    public Vector3 OpenCloseAxis;
    public float MoveSpeed = 10;

    // Update is called once per frame
    private void Update()
    {
        // move whole claw
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Root.Rotate(RotateAxis, RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Root.Rotate(RotateAxis, -RotateSpeed * Time.deltaTime);
        }
        
        // open/close claw
        if (Input.GetKey(KeyCode.UpArrow))
        {
            LeftHand.Translate(OpenCloseAxis * MoveSpeed * Time.deltaTime);
            RightHand.Translate(OpenCloseAxis * -MoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            LeftHand.Translate(OpenCloseAxis * -MoveSpeed * Time.deltaTime);
            RightHand.Translate(OpenCloseAxis * MoveSpeed * Time.deltaTime);
        }
    }
}
