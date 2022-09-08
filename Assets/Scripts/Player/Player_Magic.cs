using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;

public class Player_Magic : MonoBehaviour
{
    //////////////////////////////////////////// camera testing for directional magic shooting
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHit;

      private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    ///////////////////////////////////////////
    [SerializeField] private Spell spellToCast;

    [SerializeField] private float timeBetweenCasts = 0.25f;
    private bool isCastingMagic = false;
    private float currentCastTimer;
    public Transform castPoint;

    public CharacterController _CharacterController;

    bool inputCast = false;

    // Start is called before the first frame update
    void Awake()
    {
        //  magic directional shooting
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        aimSensitivity = 1f;

        isCastingMagic = false;
        _CharacterController = GetComponent<CharacterController>();
     
    }


    public void OnSpellCast(){
        inputCast = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(inputCast && !isCastingMagic){
            Debug.Log("input click");
            
            inputCast = false;
            isCastingMagic = true;
            currentCastTimer = 0;
           
            // CastSpell();
        }
         DirectionalCast();
        // if(!isCastingMagic){
        //     // Debug.Log("casting a spell");
        //     CastSpell();
        //     isCastingMagic = true;
        //     currentCastTimer = 0;
        // }

        if(isCastingMagic){
            if(!spellToCast.SpellToCast.playerCanMove) _CharacterController.enabled = false;
            currentCastTimer += Time.deltaTime;
            if(currentCastTimer > timeBetweenCasts) isCastingMagic = false;
        }
         if(!isCastingMagic) _CharacterController.enabled = true;
    }

// instantiate spell 
    void CastSpell(){

        Spell childObject = Instantiate(spellToCast, castPoint.position, castPoint.rotation );
        if(spellToCast.SpellToCast.stickToCastPoint){
            childObject.transform.parent = gameObject.transform;
        }
        
    }

    void DirectionalCast(){
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

        if (starterAssetsInputs.spellCast) {
            // Projectile Shoot
            Vector3 aimDir = (mouseWorldPosition - castPoint.position).normalized;

            Spell childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimDir, Vector3.up) );
            if(spellToCast.SpellToCast.stickToCastPoint){
                childObject.transform.parent = gameObject.transform;
            }
            starterAssetsInputs.spellCast = false;
        }
    }
}
