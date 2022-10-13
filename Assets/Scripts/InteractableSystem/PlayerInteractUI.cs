using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProGUI;

    private void Update(){
        if(playerInteract.GetInteractableObject() != null){
            Show(playerInteract.GetInteractableObject());
        }
        else{
            Hide();
        }
    }


    private void Show(IInteractable interactable){
        containerGameObject.SetActive(true);
        interactTextMeshProGUI.text = interactable.GetInteractText();
    }
    private void Hide(){
        containerGameObject.SetActive(false);
    }
}
