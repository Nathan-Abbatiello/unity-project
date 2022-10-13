using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen;

    private void Awake(){
        animator = GetComponent<Animator>(); 
    }

    private void ToggleDoor(){
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }
    public string GetInteractText()
    {
        return "Open/Close Door";
    }

    public void Interact(Transform interactorTransform)
    {
        ToggleDoor();
    }
      public Transform GetTransform(){
        return transform;
    }

}
