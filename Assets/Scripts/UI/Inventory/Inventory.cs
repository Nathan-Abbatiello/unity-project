using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using UnityEngine.InputSystem;


public class Inventory : MonoBehaviour
{   

    // Cameras
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera inventoryMenuCam;

    public PlayerInput input;
    private StarterAssetsInputs starterAssetsInputs;
    public GameObject menuAsset; 

    public GameObject playerHUD;

    // is the inventory menu open 
    public bool menuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.inventoryMenu){
            ToggleMenu();
            starterAssetsInputs.inventoryMenu = false;
        }
    }

    // decide whether to open or close menu 
    void ToggleMenu(){
        if(menuOpen){
            CloseMenu();
        }
        else{
            OpenMenu();
        }
    }

    void OpenMenu(){
        // pause audio
        AudioListener.pause = true;  
        // change camera 
        inventoryMenuCam.Priority = 20;
        playerCam.Priority = 0;
        // pause time
        // Time.timeScale = 0;
        menuOpen = true;
        playerHUD.SetActive(false);
        // input.SwitchCurrentActionMap("UI");
    }

    void CloseMenu(){
        // pause audio
        AudioListener.pause = false;
        // change camera 
        playerCam.Priority = 20;
        inventoryMenuCam.Priority = 0;
        // resume time
        // Time.timeScale = 1;
        menuOpen = false;
        playerHUD.SetActive(true);
        // input.SwitchCurrentActionMap("Player");
    }
}
