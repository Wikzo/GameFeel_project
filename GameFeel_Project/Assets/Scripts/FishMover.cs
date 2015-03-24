using UnityEngine;
using System.Collections;

public class FishMover : MonoBehaviour
{
    public float Speed;
    private Transform _transform;

    void Start()
    {
        _transform = gameObject.transform;
    }

    private void Update()
    {
        transform.Translate(Vector3.left*Speed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Reverse")
        {
            Speed *= -1;
            _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.y);

        }
    }
}
