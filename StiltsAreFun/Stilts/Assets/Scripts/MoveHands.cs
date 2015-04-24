using UnityEngine;
using System.Collections;

public class MoveHands : MonoBehaviour
{
    public bool CalibrateOnStart = false;
    public bool CubesActive = true;
    public GameObject Cubes;

    // root
    public Transform Root;
    public float RootRotateSpeed = 10f;
    public Vector3 RootRotateUpDownAxis;
    public Vector3 RootRotateLeftRightAxis;
    public Vector3 RootMoveHorizontalAxis;
    public Vector3 RootMoveVerticalAxis;
    public Vector3 RootMoveSideStepAxis;
    public float RootMoveSpeed = 5;

    public Transform LeftHand, RightHand;
    public Vector3 HandsOpenCloseAxis;
    public float HandMoveSpeed = 1;

    //public Vector3 StartHandVectorPosLeft = new Vector3(2.6f, 4.1f, -5.0f);
    //public Vector3 StartHandVectorPosRight = new Vector3(2.6f, 4.1f, 5.0f);

    private bool _calibrate = true;

    void Start()
    {
        //LeftHand.position = StartHandVectorPosLeft;
        //RightHand.position = StartHandVectorPosRight;

        Cubes.SetActive(CubesActive);

    }

    // Update is called once per frame
    private void Update()
    {
        if (_calibrate && CalibrateOnStart) // 5.029198
        {
            LeftHand.Translate(HandsOpenCloseAxis * -HandMoveSpeed * Time.deltaTime);
            RightHand.Translate(HandsOpenCloseAxis * HandMoveSpeed * Time.deltaTime);

            if (RightHand.position.z >= 4.8f)
                _calibrate = false;
            else
                return;
        }

        // restart
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
        else if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        // set time
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            Time.timeScale = 2;
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
            Time.timeScale = 0.5f;
        else
            Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.C))
        {
            CubesActive = !CubesActive;

            Cubes.SetActive(CubesActive);
        }
        
        // ROOT MOVEMENT/ROTATION --------------------------

        // root rotate left/right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Root.Rotate(RootRotateLeftRightAxis, RootRotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Root.Rotate(RootRotateLeftRightAxis, -RootRotateSpeed * Time.deltaTime);
        }

        // root rotate up/down
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Root.Rotate(RootRotateUpDownAxis, -RootRotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Root.Rotate(RootRotateUpDownAxis, RootRotateSpeed * Time.deltaTime);
        }

        // root move forward/backward
        if (Input.GetKey(KeyCode.W))
        {
            Root.Translate(RootMoveHorizontalAxis * -RootMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Root.Translate(RootMoveHorizontalAxis * RootMoveSpeed * Time.deltaTime);
        }

        // root move up/down
        if (Input.GetKey(KeyCode.Q))
        {
            Root.Translate(RootMoveVerticalAxis * RootMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Root.Translate(RootMoveVerticalAxis * -RootMoveSpeed * Time.deltaTime);
        }

        // root move sidestep
        if (Input.GetKey(KeyCode.A))
        {
            Root.Translate(RootMoveSideStepAxis * -RootMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Root.Translate(RootMoveSideStepAxis * RootMoveSpeed * Time.deltaTime);
        }

        // HANDS OPEN/CLOSE -----------------------------------------

        if (Input.GetKey(KeyCode.RightShift)) // && RightHand.position.z > 1.49f
        {
            LeftHand.Translate(HandsOpenCloseAxis * HandMoveSpeed * Time.deltaTime);
            RightHand.Translate(HandsOpenCloseAxis * -HandMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightControl)) // && RightHand.position.z < 5f
        {
            LeftHand.Translate(HandsOpenCloseAxis * -HandMoveSpeed * Time.deltaTime);
            RightHand.Translate(HandsOpenCloseAxis * HandMoveSpeed * Time.deltaTime);
        }


        
    }
}
