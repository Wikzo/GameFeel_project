using UnityEngine;
using System.Collections;

public class HingeJointInformation : MonoBehaviour
{
    private HingeJoint _myHingeJoint;

    public float Angle;

    // Use this for initialization
    private void Start()
    {
        foreach (HingeJoint hinge in GetComponents<HingeJoint>())
        {
            if (hinge.axis == Vector3.forward)
            {
                _myHingeJoint = hinge;
                break;
            }
        }

    }

    // Update is called once per frame
    private void Update()
    {
       //Debug.Log(_myHingeJoint.angle);

        Angle = _myHingeJoint.angle;


        return;
        if (Angle < 0)
        {
            JointLimits limits = _myHingeJoint.limits;

            //limits.min = -90.5f;
            limits.min = -91f;
            limits.max = -89f;

            _myHingeJoint.limits = limits;
        }
        else
        {
            JointLimits limits = _myHingeJoint.limits;

            //limits.min = 90.5f;
            limits.min = 89f;
            limits.max = 91f;


            _myHingeJoint.limits = limits;
        }
    }
}
