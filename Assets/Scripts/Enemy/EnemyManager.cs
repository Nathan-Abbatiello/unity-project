using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class EnemyManager : MonoBehaviour
{
    public EnemyAttributesScObj enemyAttributes;

    //  Health Stats
    private float maxHealth;
    private float currentHealth;
    public Slider healthBar;  

    private Camera _cam;

    private float chaseRadius;
    private float attackRadius;

    public Transform target;
    private NavMeshAgent agent;

    [SerializeField] private Spell spellToCast;
    public Transform castPoint;

    public Transform Player;

    private bool attacking;



    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    } 

    // Start is called before the first frame update
    private void Awake()
    {
        _cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        maxHealth = enemyAttributes.maxHealth;
        currentHealth = maxHealth;
        chaseRadius = enemyAttributes.chaseRadius;
        attackRadius = enemyAttributes.attackRadius;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        //  chase state
        if(distance <= chaseRadius){
            agent.SetDestination(target.position);
        }
        if(distance<= agent.stoppingDistance){
            FaceTarget();
        }

        // attack state
        if(distance <= attackRadius && attacking == false){
            attacking = true;
            StartCoroutine(SpellSpawnDelay(2f));
        }
        DisplayStats();

    }

    
    IEnumerator SpellSpawnDelay(float delay){
        yield return new WaitForSeconds(delay);
        Vector3 aimdir = new Vector3(Player.transform.position.x, Player.transform.position.y +1, Player.position.z); 
        Spell childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimdir-castPoint.position, Vector3.up) );
        Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
        attacking = false;

    }

    void DisplayStats()
    {
        healthBar.value = currentHealth / maxHealth;
        healthBar.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }

    private void FaceTarget(){
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(float damage){
        // currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if(currentHealth<= 0 ) Destroy(this.gameObject);
    }
}
