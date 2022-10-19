using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigationControl : MonoBehaviour
{
    [SerializeField] Animator anim;
    NavMeshAgent agent;
    Vector3 worldDeltaPosition;
    Vector2 groundDeltaPosition;
    Vector2 velocity = Vector2.zero;

    [SerializeField] private float pvely;
    [SerializeField] private float pvelx;


    bool allowMove;
    
    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        allowMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        worldDeltaPosition = agent.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : velocity = Vector2.zero;
        if(allowMove){
            bool shouldMove = velocity.magnitude > 0.025f && agent.remainingDistance > agent.radius;
            anim.SetBool("move", shouldMove);
            anim.SetFloat("vely",velocity.y);
            pvely = velocity.y;
        }
        else if(!allowMove){
            anim.SetFloat("vely",0);
            pvely = 0;
            anim.SetBool("move", false);
            agent.nextPosition = transform.position;
        }
        anim.SetFloat("velx",velocity.x);

        pvelx = velocity.x;
        

        
    }

    public void AllowMovement(bool setMove){
        allowMove = setMove;
    }

    void OnAnimatorMove(){
        if(allowMove) transform.position = agent.nextPosition;
    }
}
