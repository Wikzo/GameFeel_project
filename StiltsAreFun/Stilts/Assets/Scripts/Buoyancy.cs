using UnityEngine;
using System.Collections;

public class Buoyancy : MonoBehaviour
{
    public float Volume;
    private BoxCollider myBoxCollider;////
    public bool UnderWater;

    public float PFluid;
    private float WaterProperty = 1000;

    public float underWater;
    public float bottom;

    public bool UseSphere = true;


    private Rigidbody _rg;

    // Use this for initialization
    private void Start()
    {
        myBoxCollider = GetComponent<BoxCollider>();

        _rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!UseSphere)
        {
            bottom = transform.position.y - transform.localScale.y/2;

            // h = radius-yPos
            // if h > 0 = above water
            // if h > 2r = fully submerged

            // sphere cap

            if (bottom < 0)
            {
                underWater = 0 - bottom;

                if (underWater > myBoxCollider.size.y) // fully submerged
                    Volume = myBoxCollider.size.x*myBoxCollider.size.y*myBoxCollider.size.z;
                else
                    Volume = myBoxCollider.size.x*underWater*myBoxCollider.size.z;

                PFluid = Volume*WaterProperty;

                // F buoyancy = gm displaced fluid e up  == gp fluid V object e up
                var buoyancy = -Physics.gravity.y*PFluid*Volume;
                //Debug.Log(buoyancy);
                _rg.AddForce(new Vector3(0, buoyancy, 0));
            }
        }


        if (UseSphere)
        {
            // Kraus' answer (for sphere):
            var radius = transform.localScale.y/2;
            var h = radius - transform.position.y; // h = part under water

            if (h > 0) // under water
            {
                if (h > 2*radius) // fully submerged
                    h = 2*radius;

                // spherical cap: http://en.wikipedia.org/wiki/Spherical_cap
                var submergedVolume = Mathf.PI*h*h/3*(3*radius - h);

                var density = 1000;
                Vector3 byouancyForceSphere = -Physics.gravity*density*submergedVolume;

                _rg.AddForce(byouancyForceSphere);
            }
        }

    }
}
