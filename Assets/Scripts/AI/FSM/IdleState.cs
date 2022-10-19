using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public State chaseState;
    public State patrolState;

    private bool patrol;

    private StateManager _manager;


    public override State RunCurrentState(StateManager manager)
    {
        _manager = manager;

        float distance = Vector3.Distance(manager.target.position, transform.position);
        // StartCoroutine(patrolInterval(5f));

        if (distance <= manager.enemyAttributes.chaseRadius)
        {
            return chaseState;
        }
        // if (patrol)
        // {
        //     return patrolState;
        // }
        else{
            return this;
        }
    }

    private IEnumerator patrolInterval(float interval){
        yield return new WaitForSeconds(interval);
        patrol = true;
    }
}
