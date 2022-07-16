using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    public SpellScriptableObj SpellToCast;
    private SphereCollider collider;
    private Rigidbody rigidbody;

    private void Awake(){
        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = SpellToCast.spellRadius;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
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
