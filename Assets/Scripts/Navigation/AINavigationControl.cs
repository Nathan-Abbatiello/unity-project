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
    
    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        worldDeltaPosition = agent.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : velocity = Vector2.zero;
        bool shouldMove = velocity.magnitude > 0.025f && agent.remainingDistance > agent.radius;
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx",velocity.x);
        anim.SetFloat("vely",velocity.y);
        
    }

    void OnAnimatorMove(){
        transform.position = agent.nextPosition;
    }
}
