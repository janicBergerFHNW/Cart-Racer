using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    private Goal[] _goals;
    private int _nextGoal = 1;
    private int _prevGoal => (_nextGoal + _goals.Length - 1) % _goals.Length;
    
    void Start()
    {
        _goals = GetComponentsInChildren<Goal>();
        foreach (var goal in _goals)
        {
           goal.GoalPassed += OnGoalPassed;
        }
        
        _goals[_nextGoal].MarkAsNext();
    }

    private void OnGoalPassed(object sender, Goal.GoalEA e)
    {
        Debug.Log("goal passed in manager");
        var goalPassed = e.Goal;

        if (_goals[_nextGoal] != goalPassed)
        {
            Debug.Log("Goal skipped!");
            return;
        }
        
        
        e.Cart.ResetPosition = e.Goal.ResetPosition;
        e.Cart.ResetRotation = e.Goal.transform.rotation;
        
        Debug.Log($"passed goal {_nextGoal}");
        _nextGoal = (_nextGoal + 1) % _goals.Length;
        if (_nextGoal == 1)
        {
            Debug.Log("Lap done");
            LapDoneEvent?.Invoke(this, EventArgs.Empty);
        }

        MarkNextGoal();
    }

    public event EventHandler LapDoneEvent;

    void MarkNextGoal()
    {
        var nextGoal = _goals[_nextGoal];
        nextGoal.MarkAsNext();
        var prevGoal = _goals[_prevGoal];
        prevGoal.Unmark();
    }
}
