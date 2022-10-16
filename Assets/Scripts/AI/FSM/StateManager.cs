using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;
    void Update()
    {
        RunStateMachine();   
    }

    void RunStateMachine(){
        State nextState = currentState?.RunCurrentState();

        if(nextState != null){
            SwitchToNextState(nextState);
        } 
    }

    private void SwitchToNextState(State nextState){
        currentState = nextState;
    }
}
