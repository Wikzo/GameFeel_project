using UnityEngine;
using System.Collections;

public class SpeedTest : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 2.0F;
    private float startTime;
    private float journeyLength;
    public Transform target;
    public float smooth = 5.0F;
    void Start()
    {
        startTime = Time.time;
        journeyLength = velocity;
    }
    void Update()
    {
        if (velocity <= 0)
            return;

         distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLength;
        velocity = Mathf.Lerp(5, 0, fracJourney);
    }

    private float velocity = 0.2f;
    private float fracJourney;
    private float distCovered;

    void OnGUI()
    {
        GUI.Label(new Rect((int)Screen.width / 2, (int)Screen.height / 2, 200, 200), "T: " + velocity);
        
    }
}