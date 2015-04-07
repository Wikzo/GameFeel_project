using UnityEngine;
using System.Collections;

public class RotateAroundQuaternionScript : MonoBehaviour
{

    // http://docs.unity3d.com/ScriptReference/Quaternion.html
    // http://www.blog.radiator.debacle.us/2015/02/how-to-make-stuff-look-at-stuff.html

    public Vector3 AngularVelocity;
    private Transform _myTransform;

    // Use this for initialization
    private void Start()
    {
        _myTransform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        Quaternion oldQ = _myTransform.rotation;
        Quaternion w = new Quaternion(AngularVelocity.x, AngularVelocity.y, AngularVelocity.z, 0);

        Quaternion deltaQ = new Quaternion(0,0,0,0.5f) * w * oldQ;

        Quaternion newQ = new Quaternion(
            oldQ.x + deltaQ.x*Time.deltaTime,
            oldQ.y + deltaQ.y*Time.deltaTime,
            oldQ.y + deltaQ.y*Time.deltaTime,
            oldQ.w + deltaQ.w*Time.deltaTime);

        _myTransform.rotation = newQ;
    }
}
