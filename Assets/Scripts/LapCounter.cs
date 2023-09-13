using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    private int _laps = 0;
    private float _lastLapTime = 0;
    
    void Start()
    {
        useGUILayout = true;
        FindObjectOfType<TrackManager>().LapDoneEvent += (_, _) =>
        {
            _laps++;
            _lastLapTime = Time.timeSinceLevelLoad;
        };
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 300, 300));
        GUILayout.Label($"Laps: {_laps}");
        GUILayout.Label($"Time: {Time.timeSinceLevelLoad - _lastLapTime}");
        if (GUILayout.Button("Cheat"))
        {
            _laps++;
        }
        GUILayout.EndArea();
    }
}
