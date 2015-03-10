using UnityEngine;
using System.Collections;

public class BouncingBallEnergyConservation : MonoBehaviour
{
    public float KineticEnergy;
    public float PotentialEnergy;
    public float TotalEnergyCurrent;
    public float TotalEnergyInitial;
    public float KineticThreshold = 0.1f;
    public float TargetVelocitySquared;
    public Vector3 GroundLevel = new Vector3(0, 0, 0);

    private Rigidbody _myRigidbody;

    // Kinetic (T): 1/2 * m * Velocity.sqrtMagnitude
    // Potential (U): m*g (h-h0)

    // Use this for initialization
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody>();

        KineticEnergy = 0.5f*_myRigidbody.mass*_myRigidbody.velocity.sqrMagnitude;
        PotentialEnergy = _myRigidbody.mass*(-Physics.gravity.y)*(transform.position.y - GroundLevel.y);

        TotalEnergyInitial = KineticEnergy + PotentialEnergy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        KineticEnergy = 0.5f * _myRigidbody.mass * _myRigidbody.velocity.sqrMagnitude;
        PotentialEnergy = _myRigidbody.mass * (-Physics.gravity.y) * (transform.position.y - GroundLevel.y);

        TotalEnergyCurrent = KineticEnergy + PotentialEnergy;

        // wanted kinetic 
        TargetVelocitySquared = 2f/_myRigidbody.mass*(TotalEnergyInitial - PotentialEnergy);
        if (TargetVelocitySquared <= KineticThreshold || _myRigidbody.velocity.magnitude <= 0.0001f)
            return;

        // problems with rotation??

        _myRigidbody.AddForce(
            _myRigidbody.velocity.normalized*Mathf.Sqrt(TargetVelocitySquared) - _myRigidbody.velocity,
            ForceMode.VelocityChange);
    }
}
