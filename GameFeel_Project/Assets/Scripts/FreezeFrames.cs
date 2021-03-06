﻿using UnityEngine;
using System.Collections;

public class FreezeFrames : MonoBehaviour
{

    public int NumberOfFramesToSkip = 10;

    public void Freeze(int? frames)
    {
        if (frames.HasValue)
            StartCoroutine(FreezeGame(frames.Value));   
        else
            StartCoroutine(FreezeGame(NumberOfFramesToSkip));
        }


    private IEnumerator FreezeGame(int numFramesToSkip)
    {
        // slow it down to a stop
        Time.timeScale = 0.01f;

        // wait for it to take effect
        yield return null;

        // skip X frames
        for (int x = 0; x < numFramesToSkip; x++)
            yield return null;

        // return to normal
        Time.timeScale = 1;

    }
}
