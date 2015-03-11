using UnityEngine;
using System.Collections;

public class ExplosiveForce : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;
    public float upwardsMultiplier = 3F;
    public bool AutoExplode = false;
    void Update()
    {
    	if (AutoExplode || Input.GetKeyDown(KeyCode.Space))
    	{
        	Vector3 explosionPos = transform.position;
        	Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        	foreach (Collider hit in colliders)
        	{
            	if (hit && hit.rigidbody)
            	{
                	hit.rigidbody.AddExplosionForce(power, explosionPos, radius, upwardsMultiplier);
                	Debug.Log("Hit");
            	}
            }
    	}
	}

	void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
