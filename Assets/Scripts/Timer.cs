using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public static Timer instance;

    public Text timerText;
    private float startTime;

    private TimeSpan timePlaying;
    private bool isTiming;

    private float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timerText.text = "Time: 00:00.00";
        isTiming = false;
    }

    public void StartTimer()
    {
        isTiming = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void PauseTimer()
    {
        isTiming = false;
    }

    public void ContinueTimer()
    {
        isTiming = true;
    }

    private IEnumerator UpdateTimer()
    {
        while (isTiming)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timerText.text = timePlayingStr;

            yield return null;
        }
    }
}