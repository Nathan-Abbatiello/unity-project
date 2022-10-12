using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public EnemyAttributesScObj enemyAttributes;

    //  Health Stats
    private float maxHealth;
    private float currentHealth;

    public float lookRadius = 10f;
    public Transform target;
    private NavMeshAgent agent;

    void OnDrawGizmoSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    } 

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        maxHealth = enemyAttributes.maxHealth;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= lookRadius){
            agent.SetDestination(target.position);
        }
        
    }

    public void TakeDamage(float damage){
        // currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if(currentHealth<= 0 ) Destroy(this.gameObject);
    }
}
