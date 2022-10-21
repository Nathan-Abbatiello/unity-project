using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using TMPro;

public class Player_Magic : MonoBehaviour
{
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    ///////////////////////////////////////////
    
    // Profile variables
    private ProfileScObj activeProfile;
    private int currentProfileIndex;
    [SerializeField] private ProfileScObj[] profiles;

    // Spell Variables
    [SerializeField] private Spell spellToCast;
    private SpellScriptableObj spellProperties;

    private float timeBetweenCasts;
    private bool isCastingMagic = false;
    [SerializeField] private float currentCastTimer;

    public Transform castPoint;
    public List<Transform> castPoints = new List<Transform>();

    private Vector3 mouseWorldPosition;

    private Spell childObject;

    public CharacterController _CharacterController;
    public PlayerControls controls;  

    // UI DIsplay 
    [SerializeField] private TextMeshProUGUI profileName;
    [SerializeField] private TextMeshProUGUI priSpellName;
    [SerializeField] private TextMeshProUGUI secSpellName;

    // Start is called before the first frame update
    void Awake()
    {
        currentProfileIndex = 0;
        activeProfile = profiles[currentProfileIndex];
        // player controls
        controls = new PlayerControls();
        //  get components
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        _CharacterController = GetComponent<CharacterController>();
        
        aimSensitivity = 1f;
        // current spell scriptableObj
        spellProperties = spellToCast.SpellToCast;
        isCastingMagic = false;
        timeBetweenCasts = spellProperties.coolDown;
        UpdateProfile();
        setCastPoint();
    }

    // Update is called once per frame
    void Update()
    {
        DirectionalAim();
        if(childObject != null) spellToCast.FollowCastPoint(castPoint, childObject);

        // Cast spell if cooldown finished
        if(!isCastingMagic && starterAssetsInputs.PrimaryCast){
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

    void OnEnable(){
        controls.Enable();
        controls.Player.PrimaryCast.performed += PrimaryCast;
        controls.Player.SecondaryCast.performed += SecondaryCast;
        controls.Player.NextProfile.performed += NextProfile;
        controls.Player.PreviousProfile.performed += PreviousProfile;
    }

    void OnDisable(){
        controls.Disable();
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
        // if (starterAssetsInputs.PrimaryCast) {
        //     StartCoroutine(SpellSpawnDelay(spellProperties.spawnDelay, spellToCast));
        // }
        
    }

    private void PrimaryCast(InputAction.CallbackContext context){
        StartCoroutine(SpellSpawnDelay(activeProfile.PrimarySpell.SpellToCast.spawnDelay, activeProfile.PrimarySpell));
    }
    private void SecondaryCast(InputAction.CallbackContext context){
        StartCoroutine(SpellSpawnDelay(activeProfile.SecondrySpell.SpellToCast.spawnDelay, activeProfile.SecondrySpell));
    }

    IEnumerator SpellSpawnDelay(float delay, Spell spellToCast){
        yield return new WaitForSeconds(delay);
        Vector3 aimDir = (mouseWorldPosition - castPoint.position).normalized;
         // Spawn spell
        childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimDir, Vector3.up) );
        Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
        starterAssetsInputs.PrimaryCast = false;

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

    private void NextProfile(InputAction.CallbackContext context){
        if(currentProfileIndex+1 <= profiles.Length-1){
            currentProfileIndex += 1;
            activeProfile = profiles[currentProfileIndex];
            UpdateProfile();
        }
    }
    private void PreviousProfile(InputAction.CallbackContext context){
        if(currentProfileIndex-1 >= 0){
            currentProfileIndex -= 1;
            activeProfile = profiles[currentProfileIndex];
            UpdateProfile();
        }
    }

    void UpdateProfile(){
        profileName.text = activeProfile.ProfileName;
        priSpellName.text = activeProfile.PrimarySpell.SpellToCast.spellName;
        secSpellName.text = activeProfile.SecondrySpell.SpellToCast.spellName;
    }
}
