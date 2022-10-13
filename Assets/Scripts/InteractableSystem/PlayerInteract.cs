using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    void Update()
    {
        // if(Input){
        IInteractable interactable = GetInteractableObject();
        if(interactable != null){
            interactable.Interact(transform);
        }
    // ]
    }

    public IInteractable GetInteractableObject(){
        List<IInteractable> interactableList  = new List<IInteractable>();
         float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if(collider.TryGetComponent(out IInteractable interactable)){
                interactableList.Add(interactable);
            }    
        }
        
        IInteractable closestinteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if(closestinteractable == null){
                closestinteractable = interactable;
            }
            else{
                if(Vector3.Distance(transform.position, interactable.GetTransform().position) <
                Vector3.Distance(transform.position, closestinteractable.GetTransform().position)){
                    closestinteractable = interactable;
                }
            }
        }
            return closestinteractable;

    }
}
