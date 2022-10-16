using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;
        public NavMeshAgent agent;
        private IHealthComponent healthComponent;
        public AINavigationControl aiNav;
        public EnemyAttributesScObj enemyAttributes;
        public Transform target;
        public Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthComponent = GetComponent<IHealthComponent>();
        aiNav = GetComponent<AINavigationControl>();
    }
    void Update()
    {
        RunStateMachine();   
    }

    void RunStateMachine(){
        State nextState = currentState?.RunCurrentState(this);

        if(nextState != null){
            SwitchToNextState(nextState);
        } 
    }

    private void SwitchToNextState(State nextState){
        currentState = nextState;
    }
}
