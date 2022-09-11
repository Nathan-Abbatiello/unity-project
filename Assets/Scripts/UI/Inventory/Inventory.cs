using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class Inventory : MonoBehaviour
{   

    private StarterAssetsInputs starterAssetsInputs;
    public GameObject menuAsset; 

    // is the inventory menu open 
    public bool menuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
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
        Time.timeScale = 0;
        AudioListener.pause = true;
        menuOpen = true;
        menuAsset.SetActive(true);
    }

    void CloseMenu(){
        Time.timeScale = 1;
        AudioListener.pause = false;
        menuOpen = false;
        menuAsset.SetActive(false);
    }
}
