using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Magic : MonoBehaviour
{
    [SerializeField] private Spell spellToCast;

    [SerializeField] private float timeBetweenCasts = 0.25f;
    private bool isCastingMagic = false;
    private float currentCastTimer;
    public Transform castPoint;

    bool inputCast = false;

    public PlayerControls playerControls; 
    // Start is called before the first frame update
    void Awake()
    {
        isCastingMagic = false;
        playerControls = new PlayerControls();
     
    }
    void Start(){
    }
    private void onEnable(){
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    public void OnSpellCast(){
        inputCast = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool isSpellCastHeldDown = playerControls.Player.SpellCast.ReadValue<float>() > .1f;
        if(inputCast && !isCastingMagic){
            Debug.Log("input click");
            
            inputCast = false;
            isCastingMagic = true;
            currentCastTimer = 0;
            CastSpell();
        }
        // if(!isCastingMagic){
        //     // Debug.Log("casting a spell");
        //     CastSpell();
        //     isCastingMagic = true;
        //     currentCastTimer = 0;
        // }

        if(isCastingMagic){
            currentCastTimer += Time.deltaTime;
            if(currentCastTimer > timeBetweenCasts) isCastingMagic = false;
        }
    }

    void CastSpell(){
        Instantiate(spellToCast, castPoint.position, castPoint.rotation );
    }
}
