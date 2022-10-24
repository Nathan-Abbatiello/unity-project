using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer
{
    // length of the timer
    public float _duration;
    // current count in timer
    public float _currentTime;

    // runs the timer (put in an update for the script using the timer)
    // returns true if timer is complete
    public void RunTimer(){
        if(_currentTime != _duration){
            _currentTime += Time.deltaTime;
        }
        if(_currentTime >= _duration) {
            _currentTime = 0;
            _duration = 0;
        }
    } 

    // resets the timer with a certain value
    public void ResetTimer(float duration){
        _duration = duration;
    }

    public bool IsFinished(){
        if(_currentTime == _duration) return true;
        else return false;
    }

    //  returns the remaining time of the timer
    public float TimeRemaining(){
        return _duration - _currentTime;
    }
}
