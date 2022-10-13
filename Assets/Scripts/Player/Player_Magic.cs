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
    private SpellScriptableObj spellProperties;

    [SerializeField] private float timeBetweenCasts = 0.25f;
    private bool isCastingMagic = false;
    [SerializeField] private float currentCastTimer;

    public Transform castPoint;
    public List<Transform> castPoints = new List<Transform>();

    private Vector3 mouseWorldPosition;

    private Spell childObject;

    public CharacterController _CharacterController;

  

    // Start is called before the first frame update
    void Awake()
    {
        //  get components
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        _CharacterController = GetComponent<CharacterController>();
        
        aimSensitivity = 1f;
        // current spell scriptableObj
        spellProperties = spellToCast.SpellToCast;
        isCastingMagic = false;
       
       setCastPoint();
    }

    // Update is called once per frame
    void Update()
    {
        DirectionalAim();
        if(childObject != null) spellToCast.FollowCastPoint(castPoint, childObject);

        // Cast spell if cooldown finished
        if(!isCastingMagic && starterAssetsInputs.spellCast){
            currentCastTimer = 0;
            isCastingMagic = true;
            animator.Play("Magic Heal");
            Cast();
            _CharacterController.enabled = true;
        }
      
        // currently casting    
        if(isCastingMagic){
            Casting();
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
            // Vector3 aimDir = (mouseWorldPosition - castPoint.position).normalized;
            // // Spawn spell
            // childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimDir, Vector3.up) );
            // Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
            // starterAssetsInputs.spellCast = false;
            StartCoroutine(SpellSpawnDelay(spellProperties.spawnDelay));
        }
    }

    IEnumerator SpellSpawnDelay(float delay){
        yield return new WaitForSeconds(delay);
        Vector3 aimDir = (mouseWorldPosition - castPoint.position).normalized;
         // Spawn spell
        childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimDir, Vector3.up) );
        Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
        starterAssetsInputs.spellCast = false;

    }

    // Things that are done during casting duration
    void Casting(){
        // Disable player movement
        if(!spellProperties.playerCanMove) _CharacterController.enabled = false;
        // Increment cast timer
        currentCastTimer += Time.deltaTime;
        if(currentCastTimer > timeBetweenCasts) isCastingMagic = false;
    }
    
    // Set position for spell to spawn 
    void setCastPoint(){
         // assign the cast point to use from the spellscriptobj
        for (int i = 0; i < 3; i++)
        {
            if(castPoints[i].name == spellProperties.castPoint){
                castPoint = castPoints[i];
            }
        }
        castPoint.position += spellProperties.SpawnOffset;
    }
}
