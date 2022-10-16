using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    public EnemyAttributesScObj enemyAttributes;

    public Transform target;
    private NavMeshAgent agent;

    [SerializeField] private Spell spellToCast;
    public Transform castPoint;

    private bool attacking;

    private IHealthComponent healthComponent;

    private AINavigationControl aiNav;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        healthComponent = GetComponent<IHealthComponent>();
        healthComponent.SetMaxHealth(enemyAttributes.maxHealth);
        aiNav = GetComponent<AINavigationControl>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        //  chase state
        if(distance <= enemyAttributes.chaseRadius){
            agent.SetDestination(target.position);
        }
        // if(distance<= agent.stoppingDistance){
        //     FaceTarget();
        // }
        if(distance<= 7){  
            aiNav.AllowMovement(false);
        }
        if(distance > 7){
            aiNav.AllowMovement(true);
        }
        // attack state
        if(distance <= enemyAttributes.attackRadius && attacking == false){
            attacking = true;
            StartCoroutine(SpellSpawnDelay(2f));
        }
    }

    
    IEnumerator SpellSpawnDelay(float delay){
        yield return new WaitForSeconds(delay);
        Vector3 aimdir = new Vector3(target.transform.position.x, target.transform.position.y +1, target.position.z); 
        Spell childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimdir-castPoint.position, Vector3.up) );
        Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
        attacking = false;
    }

    private void FaceTarget(){
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttributes.chaseRadius);
    } 
}
