using System.Linq;
using UnityEngine;
using System.Collections;

public class CloudMover : MonoBehaviour
{

    private Transform _transform;
    private float _speed = 5f;

    private const float MinSpeed = 1f;
    private const float MaxSpeed = 3f;

    private SpriteRenderer _render;

    public BoxCollider2D Bounds;
    public Vector3 Min { get; private set; }
    public Vector3 Max { get; private set; }

    private void Start()
    {
        _transform = gameObject.transform;
        _render = GetComponent<SpriteRenderer>();

        _speed = Random.Range(MinSpeed, MaxSpeed);


        if (Random.Range(0,2) == 1)
            _render.sortingOrder = -16;
        else
            _render.sortingOrder = -17;


        Min = Bounds.bounds.min;
        Max = Bounds.bounds.max;
            
    }

    // Update is called once per frame
    private void Update()
    {
        _transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x > Max.x)
        {
            transform.position = new Vector3(Min.x, transform.position.y, transform.position.z);
            _speed = Random.Range(MinSpeed, MaxSpeed);
            //_render.sprite = Sprites[Random.Range(0, Sprites.Count())];
        }
    }
}
