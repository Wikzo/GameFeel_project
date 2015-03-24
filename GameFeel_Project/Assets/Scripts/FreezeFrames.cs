using UnityEngine;
using System.Collections;

public class FreezeFrames : MonoBehaviour
{

    public Renderer PlayerRender;
    public int NumberOfFramesToSkip = 10;
    private Color _defaultPlayerColor;

    // Use this for initialization
    private void Start()
    {
        _defaultPlayerColor = PlayerRender.material.color;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(FreezeGame(NumberOfFramesToSkip));

    }

    public void Freeze(int? frames)
    {
        if (frames.HasValue)
            StartCoroutine(FreezeGame(frames.Value));   
        else
            StartCoroutine(FreezeGame(NumberOfFramesToSkip));

    }


    private IEnumerator FreezeGame(int numFramesToSkip)
    {
        PlayerRender.material.color = Color.white;
        // slow it down to a stop
        Time.timeScale = 0.01f;

        // wait for it to take effect
        yield return null;

        // skip X frames
        for (int x = 0; x < numFramesToSkip; x++)
            yield return null;

        // return to normal
        Time.timeScale = 1;
        PlayerRender.material.color = _defaultPlayerColor;

    }
}
