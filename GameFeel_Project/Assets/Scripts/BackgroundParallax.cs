using UnityEngine;

class BackgroundParallax : MonoBehaviour
{
    public Transform[] Backgrounds;
    public float ParallaxScale = 2f;
    public float ParallaxReductionFactor = 4f;
    public float Smoothing = 3f;

    private Vector3 lastPosition; // camera's position in last frame

    public void Start()
    {
        lastPosition = transform.position;
    }

    public void Update()
    {
        var parallax = (lastPosition.x - transform.position.x) * ParallaxScale;

        for (var i = 0; i < Backgrounds.Length; i++)
        {
            var backgroundTargetPos = Backgrounds[i].position.x + parallax * (i * ParallaxReductionFactor + 1);

            Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].transform.position,
                new Vector3(backgroundTargetPos, Backgrounds[i].transform.position.y, Backgrounds[i].transform.position.z), 
                Time.deltaTime * Smoothing);

        }

        lastPosition = transform.position;
    }

}
