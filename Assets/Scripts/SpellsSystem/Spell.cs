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
    
    // Cast Point 
    public Transform castPoint;
    private Transform castPointWOffset;

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

        if(SpellToCast.SpawnOffset.x != 0 || SpellToCast.SpawnOffset.y != 0 || SpellToCast.SpawnOffset.z != 0){
            // Debug.Log("offset detected");
            castPointWOffset = new GameObject(SpellToCast.spellName+" castoffset").transform;
            castPointWOffset.transform.parent = castPoint.transform;
            castPointWOffset.localPosition = SpellToCast.SpawnOffset; 
        }
    }

    private void Update(){
        // applies constant forward velocity if spell has initial velocity 
        if(SpellToCast.speed > 0 ) transform.Translate(Vector3.forward * speed *Time.deltaTime);
        FollowCastPoint(castPointWOffset);
    }

    IEnumerator ZeroVelocityTime(){
        yield return new WaitForSeconds(SpellToCast.ZeroVelocityTime);
        speed = SpellToCast.speed;
    }

    public void FollowCastPoint(Transform castPoint){
        //  move the transform of the castpoint relative to the player, as otherwise the spawnoffset is added to negative numbers
        if(SpellToCast.stickToCastPoint){
            transform.position = Vector3.Lerp(transform.position, castPoint.position, Time.deltaTime *SpellToCast.stickStrength);
        }
    }

    private void DestroySpell(){
        Transform hitvfx = Instantiate(SpellToCast.hitEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(hitvfx.gameObject, 1f);
    }

    // Spell hits something with a collider
   private void OnTriggerEnter(Collider other){
        Debug.Log(SpellToCast.name + " trigger " + other.name);

        IEffectable effectable = other.GetComponent<IEffectable>();
        IHealthComponent healthComponent = other.GetComponent<IHealthComponent>();

        //  apply spell effect to other 
        if(effectable != null) effectable.ApplyEffect(_data);

        // apply health change
        if(healthComponent != null) healthComponent.AlterHealth(SpellToCast.damage);
        
        // Destroy spell
        if ( (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Environment")) &&SpellToCast.destroyOnImpact) DestroySpell();
   } 

   void OnDestroy(){
    if(SpellToCast.SpawnOffset.x != 0 || SpellToCast.SpawnOffset.y != 0 || SpellToCast.SpawnOffset.z != 0){
        Destroy(castPointWOffset.gameObject);
    }
   }
}
