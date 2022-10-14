using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    private Animator animator;
    [SerializeField] private string interactText;

    private void Awake(){
        // animator = GetComponent<Animator>();
    }
    
    public void Interact(Transform interactorTransform){
        Debug.Log("Interact"+ interactorTransform.name);
        // animator.SetTrigger("Talk");
    } 
    public string GetInteractText(){
        return interactText;
    }

    public Transform GetTransform(){
        return transform;
    }
}
