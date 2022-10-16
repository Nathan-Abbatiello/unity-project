using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public IdleState idleState;
    private StateManager _manager;

    public override State RunCurrentState(StateManager manager)
    {
        _manager = manager;
        float distance = Vector3.Distance(_manager.target.position, transform.position);
        if (distance <= _manager.enemyAttributes.attackRadius){
            return attackState;
        }
        if(distance > _manager.enemyAttributes.chaseRadius){
            return idleState;
        }
        else{
            //  chase state
            if(distance <= _manager.enemyAttributes.chaseRadius){
                _manager.agent.SetDestination(_manager.target.position);  
                _manager.aiNav.AllowMovement(true);
            }
            return this;
        }
    }
}
