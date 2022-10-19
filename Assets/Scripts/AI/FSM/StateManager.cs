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
       
        if(healthComponent.CheckDeath()){
            aiNav.AllowMovement(false);
            anim.SetLayerWeight(0,0);
            anim.SetLayerWeight(1,0);
            anim.SetLayerWeight(2,1);
            anim.SetTrigger("dead");
            StartCoroutine(Destroy(10f));
        }
        else{
            RunStateMachine();       
        }
    }
    
    IEnumerator Destroy(float delay){
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
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
