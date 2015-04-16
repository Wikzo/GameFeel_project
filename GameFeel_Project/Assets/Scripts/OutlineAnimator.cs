using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OutlineAnimator : MonoBehaviour
{
    private Outline _myOutline;
    public Color ColorA, ColorB;
    public float LerpTime;

    private float currentLerpTime;
    private bool _shouldLerp;

    // Use this for initialization
    private void Start()
    {
        _myOutline = GetComponent<Outline>();
        _shouldLerp = false;
        countUp = true;
    }

    public void StartColorAnimation()
    {
        _shouldLerp = true;
        countUp = true;
    }

    public void StopColorAnimation()
    {
        currentLerpTime = 0;
        _myOutline.effectColor = ColorA;
        _shouldLerp = false;
        countUp = true;
    }

    private bool countUp;
    // Update is called once per frame
    private void Update()
    {
        if (!_shouldLerp)
            return;

        if (countUp)
        {
            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > LerpTime)
            {
                currentLerpTime = 0;
                countUp = false;
            }
        }

        else if (!countUp)
        {
            currentLerpTime -= Time.deltaTime;

            if (currentLerpTime <= 0)
            {
                currentLerpTime = 0;
                countUp = true;
            }
        }

        float perc = currentLerpTime / LerpTime;

        _myOutline.effectColor = Color.Lerp(ColorA, ColorB, perc);

    }
}
