using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;
    private StateManager _manager; 
    private bool attacking;
    [SerializeField] private Spell spellToCast;
    [SerializeField] private float extraRadius = 0.5f;
    public Transform castPoint;

    public override State RunCurrentState(StateManager manager)
    {
        _manager = manager;
        float distance = Vector3.Distance(_manager.target.position, transform.position);

        if(distance > _manager.enemyAttributes.attackRadius + extraRadius){
            _manager.anim.SetLayerWeight(0,1);
            _manager.anim.SetLayerWeight(1,0);
            return chaseState;
        }
        else{
            if(distance <= _manager.enemyAttributes.attackRadius + extraRadius && attacking == false){
                attacking = true;
                StartCoroutine(SpellSpawnDelay(spellToCast.SpellToCast.spawnDelay));
            }
            if(distance <= _manager.enemyAttributes.attackRadius + extraRadius){  
                _manager.aiNav.AllowMovement(false);
                _manager.agent.SetDestination(_manager.target.position);
            }
            return this;    
        }
    }
    IEnumerator SpellSpawnDelay(float delay){
        _manager.anim.SetLayerWeight(0,0);
        _manager.anim.SetLayerWeight(1,1);
        _manager.anim.SetTrigger("attacking");
        yield return new WaitForSeconds(delay);
        Vector3 aimdir = new Vector3(_manager.target.transform.position.x, _manager.target.transform.position.y +1, _manager.target.position.z); 
        Spell childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimdir-castPoint.position, Vector3.up) );
        Physics.IgnoreCollision(childObject.GetComponent<Collider>(), _manager.GetComponent<Collider>());
        // needs to be dynamic to length of clip 
        // yield return new WaitForSeconds(_manager.anim.GetCurrentAnimatorStateInfo(1).length);
        yield return new WaitForSeconds(3f);
        attacking = false;
        _manager.anim.SetLayerWeight(0,1);
        _manager.anim.SetLayerWeight(1,0);
    }

    private void FaceTarget(){
        Vector3 direction = (_manager.target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _manager.enemyAttributes.chaseRadius);
    } 

}
