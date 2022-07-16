using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    public SpellScriptableObj SpellToCast;
    private SphereCollider _collider;
    private Rigidbody _rigidbody;

    private void Awake(){
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = SpellToCast.spellRadius;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        Destroy(this.gameObject, SpellToCast.lifetime);
    }

    private void Update(){
        if(SpellToCast.speed > 0 ) transform.Translate(Vector3.forward * SpellToCast.speed *Time.deltaTime);
    }

   private void OnTriggerEnter(Collider other){
        //  apply spell affect to other 
        Destroy(this.gameObject); 
   } 
}
