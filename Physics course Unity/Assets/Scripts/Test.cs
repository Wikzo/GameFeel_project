using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public GameObject Ball;
    public AnimationCurve c;
    public Gradient MyGradient;

    void Start()
    {
        rigidbody.mass = Random.Range(1f, 2f);

        float scale = Random.Range(0.5f, 2f);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void Update()
    {
        if (transform.position.y < -51) // respawn
        {
            //transform.position = new Vector3(transform.position.x, 2, transform.position.z);



            GameObject clone =
                (GameObject)
                    Instantiate(Ball,
                        new Vector3(Random.Range(-52f, 61f), Random.Range(-3f, 19f), Random.Range(12f, 20f)),
                        Quaternion.identity);

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        transform.renderer.material.color = MyGradient.Evaluate(Random.Range(0f, 1f));

        //if (col.relativeVelocity.magnitude > 2)
        //  Debug.Log("Big hit!");


    }
}
