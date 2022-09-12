using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using UnityEngine.InputSystem;

// this class also handles switching between user inputs
// needs to be separated into separate script

public class Inventory : MonoBehaviour
{   

    // Cameras
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera inventoryMenuCam;

    public PlayerControls playerCtrls;
    public PlayerInput playerInput;

    private StarterAssetsInputs starterAssetsInputs;
    public GameObject menuAsset; 

    public GameObject playerHUD;

    // is the inventory menu open 
    public bool menuOpen = false;
    // Start is called before the first frame update
    void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        playerCtrls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable(){
        playerCtrls.UI.CloseMenu.performed += ToggleMenu;

        playerCtrls.Player.InventoryMenu.performed += ToggleMenu;
        playerCtrls.Player.Enable();
    }

    void OnDisable(){
        playerCtrls.UI.CloseMenu.Disable();
        playerCtrls.Player.InventoryMenu.Disable();
    }
    // decide whether to open or close menu 
    void ToggleMenu(InputAction.CallbackContext context){
        Debug.Log("togglemenu");
        if(menuOpen){
            CloseMenu();
        }
        else{
            OpenMenu();
        }
    }

    void OpenMenu(){
        playerInput.SwitchCurrentActionMap("UI");
        playerCtrls.UI.Enable();
        playerCtrls.Player.Disable();
        // pause audio
        AudioListener.pause = true;  
        // change camera 
        inventoryMenuCam.Priority = 20;
        playerCam.Priority = 0;
        // pause time
        // Time.timeScale = 0;
        menuOpen = true;
        playerHUD.SetActive(false);
        // allow cursor to move and be seen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseMenu(){
        playerInput.SwitchCurrentActionMap("Player");
        playerCtrls.Player.Enable();
        playerCtrls.UI.Disable();
        // pause audio
        AudioListener.pause = false;
        // change camera 
        playerCam.Priority = 20;
        inventoryMenuCam.Priority = 0;
        // resume time
        // Time.timeScale = 1;
        menuOpen = false;
        playerHUD.SetActive(true);
        // lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       
    }
}
