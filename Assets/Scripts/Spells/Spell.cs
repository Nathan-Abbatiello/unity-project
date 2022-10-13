using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    // A script to handle and apply spell attributes
    [SerializeField] StatusEffectData _data;
    public SpellScriptableObj SpellToCast;
    private SphereCollider _collider;
    private Rigidbody _rigidbody;

    private float speed;

    private void Awake(){
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = SpellToCast.spellRadius;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        // maximum lifetime of spell before destroy
        Destroy(this.gameObject, SpellToCast.lifetime);

        speed = 0;
        StartCoroutine(ZeroVelocityTime());
    }

    private void Update(){
        // applies constant forward velocity if spell has initial velocity 
        if(SpellToCast.speed > 0 ) transform.Translate(Vector3.forward * speed *Time.deltaTime);
    }

    IEnumerator ZeroVelocityTime(){
        yield return new WaitForSeconds(SpellToCast.ZeroVelocityTime);
        speed = SpellToCast.speed;
    }

    public void FollowCastPoint(Transform castPoint, Spell instance){
        if(SpellToCast.stickToCastPoint && instance != null){
            instance.transform.position = Vector3.Lerp(instance.transform.position, castPoint.position, Time.deltaTime *SpellToCast.stickStrength);
        }
    }

   private void OnTriggerEnter(Collider other){
        Debug.Log(SpellToCast.name + " trigger");
        var effectable = other.GetComponent<IEffectable>();
        if(effectable != null){
            effectable.ApplyEffect(_data);
        } 

        //  apply spell affect to other 
        if (other.gameObject.CompareTag("Enemy")){
            EnemyManager enemyHealth = other.GetComponent<EnemyManager>();
            enemyHealth.TakeDamage(SpellToCast.damage);
        }
        // Destroy spell
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Environment")) && SpellToCast.destroyOnImpact){ 
            Transform hitvfx = Instantiate(SpellToCast.hitEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Destroy(hitvfx.gameObject, 5f);
        } 
   } 
}
