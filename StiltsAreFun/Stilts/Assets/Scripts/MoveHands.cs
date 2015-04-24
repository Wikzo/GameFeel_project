using UnityEngine;
using System.Collections;

public class MoveHands : MonoBehaviour
{
    public Transform Root;
    public float RotateSpeed = 10f;
    public Vector3 RotateUpDownAxis;
    public Vector3 RotateLeftRightAxis;

    public Transform LeftHand, RightHand;
    public Vector3 MoveHandsAxis;
    public float MoveSpeed = 10;

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
        else if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();


        // rotate left/right
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Root.Rotate(RotateLeftRightAxis, RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Root.Rotate(RotateLeftRightAxis, -RotateSpeed * Time.deltaTime);
        }

        // rotate up/down
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Root.Rotate(RotateUpDownAxis, -RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Root.Rotate(RotateUpDownAxis, RotateSpeed * Time.deltaTime);
        }
        
        // open/close claw
        if (Input.GetKey(KeyCode.M))
        {
            LeftHand.Translate(MoveHandsAxis * MoveSpeed * Time.deltaTime);
            RightHand.Translate(MoveHandsAxis * -MoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.N))
        {
            LeftHand.Translate(MoveHandsAxis * -MoveSpeed * Time.deltaTime);
            RightHand.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
    }
}
