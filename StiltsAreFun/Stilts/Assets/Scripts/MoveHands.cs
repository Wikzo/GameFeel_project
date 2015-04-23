using UnityEngine;
using System.Collections;

public class MoveHands : MonoBehaviour
{
    public Transform Root;
    public float RotateSpeed = 10f;

    public Transform LeftHand, RightHand;
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
            Root.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Root.Rotate(Vector3.forward, -RotateSpeed * Time.deltaTime);
        }

        // rotate up/down
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Root.Rotate(Vector3.up, -RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Root.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
        }
        
        // open/close claw
        if (Input.GetKey(KeyCode.M))
        {
            LeftHand.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
            RightHand.Translate(Vector3.forward * -MoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.N))
        {
            LeftHand.Translate(Vector3.forward * -MoveSpeed * Time.deltaTime);
            RightHand.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
    }
}
