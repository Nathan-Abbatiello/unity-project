using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public State chaseState;
    public State patrolState;

    // need to be set in parent script
    public Transform target;
    public EnemyAttributesScObj enemyAttributes;

    private bool patrol;

    public override State RunCurrentState()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        StartCoroutine(patrolInterval(5f));

        if (distance <= enemyAttributes.chaseRadius)
        {
            return chaseState;
        }
        if (patrol)
        {
            return patrolState;
        }
        else{
            return this;
        }
    }

    private IEnumerator patrolInterval(float interval){
        yield return new WaitForSeconds(interval);
        patrol = true;
    }
}
