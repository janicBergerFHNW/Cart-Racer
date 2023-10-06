using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    private TMP_Text _lapTimerText;

    private readonly Stopwatch _stopwatch = new Stopwatch();
    
    void Start()
    {
        _lapTimerText = GetComponentInChildren<TMP_Text>();
        _stopwatch.Start();
        FindObjectOfType<TrackManager>().LapDoneEvent += LapFinished;
    }

    private void LapFinished(object sender, EventArgs e)
    {
        _stopwatch.Stop();
        _lapTimerText.text = _stopwatch.Elapsed.ToString();
        _stopwatch.Restart();
    }

}
