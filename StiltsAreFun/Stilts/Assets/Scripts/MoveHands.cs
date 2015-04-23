using UnityEngine;
using System.Collections;

public class MoveHands : MonoBehaviour
{
    public Transform LeftHand, RightHand;
    public float MoveSpeed = 10;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            LeftHand.Translate(LeftHand.forward * MoveSpeed * Time.deltaTime);
            RightHand.Translate(RightHand.forward * -MoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            LeftHand.Translate(LeftHand.forward * -MoveSpeed * Time.deltaTime);
            RightHand.Translate(RightHand.forward * MoveSpeed * Time.deltaTime);
        }
    }
}
