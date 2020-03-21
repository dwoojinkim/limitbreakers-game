﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private bool isWaiting = false;

    public void Stop(float duration)
    {
        if (isWaiting)
            return;

        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        isWaiting = false;
    }
}
