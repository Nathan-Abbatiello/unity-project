using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;

public class Player_Magic : MonoBehaviour
{
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    ///////////////////////////////////////////
    [SerializeField] private Spell spellToCast;

    [SerializeField] private float timeBetweenCasts = 0.25f;
    private bool isCastingMagic = false;
    [SerializeField] private float currentCastTimer;
    public Transform castPoint;
    private Vector3 mouseWorldPosition;

    private Spell childObject;

    public CharacterController _CharacterController;

    // Start is called before the first frame update
    void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        aimSensitivity = 1f;

        isCastingMagic = false;
        _CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        DirectionalAim();
        FollowCastPoint();

        // Cast spell if cooldown finished
        if(!isCastingMagic && starterAssetsInputs.spellCast){

            // animator.SetLayerWeight(1,  0f);
            currentCastTimer = 0;
            isCastingMagic = true;
            
        }
            Cast();

         
            _CharacterController.enabled = true;
        // }

        // Increment cast timer
        if(isCastingMagic){
            // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));
            animator.SetLayerWeight(1,  1f);

            if(!spellToCast.SpellToCast.playerCanMove) _CharacterController.enabled = false;
            currentCastTimer += Time.deltaTime;
            if(currentCastTimer > timeBetweenCasts) isCastingMagic = false;
        }
        else{
            animator.SetLayerWeight(1,  0f);

            // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 20f));

        }
    }

    // get direction to cast spell
    void DirectionalAim(){
        mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) {
            // debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }

    // Instantiate Spell
    void Cast(){
        if (starterAssetsInputs.spellCast) {
            // animator.SetLayerWeight(1,  1f);


            // Projectile Shoot
            Vector3 aimDir = (mouseWorldPosition - castPoint.position).normalized;

            childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimDir, Vector3.up) );
            Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
            starterAssetsInputs.spellCast = false;
        }
    }

    void FollowCastPoint(){
        if(spellToCast.SpellToCast.stickToCastPoint && childObject != null){
            childObject.transform.position = Vector3.Lerp(childObject.transform.position, castPoint.position, Time.deltaTime *5f);
        }
    }
}
