using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public Player PlayerScript;
    public Vector2 Margin;
    public Vector2 Smoothing;
    public BoxCollider2D Bounds;
    public float Offset = 12;

    public bool IsFollowing { get; set; }

    public Vector3 Min { get; private set; }
    public Vector3 Max { get; private set; }

    private Transform _playerPos;
    private float _offset;


    //private Vector3 min, max;
    private Camera cam;

    void Awake()
    {
        Min = Bounds.bounds.min;
        Max = Bounds.bounds.max;
        IsFollowing = true;
        cam = GetComponent<Camera>();

        _playerPos = PlayerScript.transform;
    }

    void Update()
    {
        if (PlayerScript.IsFacingRight)
            _offset = Offset;
        else
            _offset = -Offset;

        var x = transform.position.x;
        var y = transform.position.y;

        if (IsFollowing)
        {
            if (Mathf.Abs(x - _playerPos.position.x + _offset) > Margin.x)
                x = Mathf.Lerp(x, _playerPos.position.x + _offset, Smoothing.x * Time.deltaTime);

            if (Mathf.Abs(y - _playerPos.position.y) > Margin.y)
                y = Mathf.Lerp(y, _playerPos.position.y, Smoothing.y * Time.deltaTime);
        }

        var cameraHalfWidth = cam.orthographicSize * cam.aspect;

        x = Mathf.Clamp(x, Min.x + _offset, Max.x - _offset);
        y = Mathf.Clamp(y, Min.y + cam.orthographicSize, Max.y - cam.orthographicSize);

        cam.transform.position = new Vector3(x, y, transform.position.z);
    }
}