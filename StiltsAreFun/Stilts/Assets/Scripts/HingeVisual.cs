using UnityEngine;
using System.Collections;

public enum LookForType
{
    Beginning,
    Middle,
    End
}
public class HingeVisual : MonoBehaviour
{
    public GameObject MyObject;
    public LookForType MyLookForType;
    public Vector3 Offset = new Vector3(0,0,0);

    private HingeJoint _myHinge;
    private bool _foundMyHinge = false;
    private Transform _myObjectTransform;
    private Transform _transform;

    // Use this for initialization
    private void Start()
    {

        _myObjectTransform = MyObject.transform;
        _transform = gameObject.transform;

        foreach (HingeJoint hinge in MyObject.GetComponents<HingeJoint>())
        {
            if (_foundMyHinge)
                return;

            switch (MyLookForType)
            {
                case LookForType.Beginning:
                    if (hinge.connectedAnchor.x < 0)
                    {
                        _myHinge = hinge;
                        //Debug.Log("found anchor" + _myHinge.connectedBody.name);
                        _foundMyHinge = true;
                    }
                    break;

                case LookForType.Middle:
                    if (hinge.connectedAnchor.x == 0)
                    {
                        _myHinge = hinge;
                        //Debug.Log("found anchor" + _myHinge.connectedBody.name);
                        _foundMyHinge = true;
                    }
                    break;

                case LookForType.End:
                    if (hinge.connectedAnchor.x > 0f)
                    {
                        _myHinge = hinge;
                        //Debug.Log("found anchor" + _myHinge.connectedBody.name);
                        _foundMyHinge = true;
                    }
                    break;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_foundMyHinge)
            return;

        //transform.position = MyObject.transform.InverseTransformPoint(_myHinge.connectedAnchor);

        _transform.localPosition = _myHinge.anchor + Offset;

    }
}
