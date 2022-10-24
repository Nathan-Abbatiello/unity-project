using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine.UI;

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

    private bool isCastingMagic = false;
    [SerializeField] private float currentCastTimer;

    public Transform castPoint;
    public List<Transform> castPoints = new List<Transform>();

    private Vector3 mouseWorldPosition;

    private Spell childObject;
    private List <Spell> activeSpells = new List<Spell>();
    private Spell activeSpell;
    
    // cooldowns
    CooldownTimer priCooldown, secCooldown;

    public CharacterController _CharacterController;
    public PlayerControls controls;  

    // UI DIsplay 
    [SerializeField] private TextMeshProUGUI profileName;
    [SerializeField] private TextMeshProUGUI priSpellName;
    [SerializeField] private Image priSpellCooldown;
    [SerializeField] private TextMeshProUGUI secSpellName;
    [SerializeField] private Image secSpellCooldown;


    // Start is called before the first frame update
    void Awake()
    {
        // player controls
        controls = new PlayerControls();
        //  get components
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        _CharacterController = GetComponent<CharacterController>();
        
        aimSensitivity = 1f;
        
        // Spell Profile vars
        currentProfileIndex = 0;
        activeProfile = profiles[currentProfileIndex];
        UpdateProfile();

        // current spell scriptableObj
        spellProperties = spellToCast.SpellToCast;
        isCastingMagic = false;
        setCastPoint();

        // Initial Cooldown timers setup
        priSpellCooldown.fillAmount = 0f;
        priCooldown = new CooldownTimer();
        secSpellCooldown.fillAmount = 0f;
        secCooldown = new CooldownTimer();
    }

    void Update()
    {
        // Run spell cooldown timers  
        priCooldown.RunTimer();
        secCooldown.RunTimer();
        // display coolsdowns and other stats
        DisplayMagicStats();
        
        // Character aiming
        DirectionalAim();  

        // currently casting    
        if(isCastingMagic){
            Casting(activeSpell);
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
    void Cast(Spell currentSpell){
        // if(!isCastingMagic){
       
        // setCastPoint();
        activeSpell = currentSpell;
        currentCastTimer = 0;
        isCastingMagic = true;
        animator.Play("Magic Heal");
        StartCoroutine(SpellSpawnDelay(currentSpell.SpellToCast.spawnDelay, currentSpell));
        _CharacterController.enabled = true;
        // }
    }

    private void PrimaryCast(InputAction.CallbackContext context){     
        if(priCooldown.IsFinished()){
            Cast(activeProfile.PrimarySpell);
            priCooldown.ResetTimer(activeProfile.PrimarySpell.SpellToCast.coolDown);
        }   
    }
    private void SecondaryCast(InputAction.CallbackContext context){
        if(secCooldown.IsFinished()){
            Cast(activeProfile.SecondrySpell);
            secCooldown.ResetTimer(activeProfile.SecondrySpell.SpellToCast.coolDown);
        }
    }

    IEnumerator SpellSpawnDelay(float delay, Spell spellToCast){
        yield return new WaitForSeconds(delay);
        Vector3 aimDir = (mouseWorldPosition - castPoint.position).normalized;
         // Spawn spell
        childObject = Instantiate(spellToCast, castPoint.position, Quaternion.LookRotation(aimDir, Vector3.up) );
        childObject.castPoint = castPoint;
        activeSpells.Add(childObject);
        Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
        starterAssetsInputs.PrimaryCast = false;
    }

    // Things that are done during casting duration
    void Casting(Spell currentSpell){
        // Disable player movement
        if(!currentSpell.SpellToCast.playerCanMove) _CharacterController.enabled = false;
        // Increment cast timer
        currentCastTimer += Time.deltaTime;
        if(currentCastTimer > currentSpell.SpellToCast.coolDown) isCastingMagic = false;
    }
    
    // Set position for spell to spawn 
    void setCastPoint(){
         // assign the cast point to use from the spellscriptobj
        for (int i = 0; i < 3; i++)
        {
            if(castPoints[i].name == activeProfile.PrimarySpell.SpellToCast.castPoint){
                castPoint = castPoints[i];
                castPoint.position += activeProfile.PrimarySpell.SpellToCast.SpawnOffset;
            }
        }
        
    }

    private void NextProfile(InputAction.CallbackContext context){
        if(currentProfileIndex+1 <= profiles.Length-1){
            currentProfileIndex += 1;
            activeProfile = profiles[currentProfileIndex]; 
        }
        UpdateProfile();
    }
    private void PreviousProfile(InputAction.CallbackContext context){
        if(currentProfileIndex-1 >= 0){
            currentProfileIndex -= 1;
            activeProfile = profiles[currentProfileIndex];
        }
        UpdateProfile();
    }

    void UpdateProfile(){
        profileName.text = activeProfile.ProfileName;
        //  set profile name visible then fade out
        profileName.color = new Color(profileName.color.r, profileName.color.g, profileName.color.b, 1);
        StartCoroutine(FadeTextToZeroAlpha(3f, profileName));

        priSpellName.text = activeProfile.PrimarySpell.SpellToCast.spellName;
        secSpellName.text = activeProfile.SecondrySpell.SpellToCast.spellName;
    }

    private void DisplayMagicStats(){
        if(!priCooldown.IsFinished()) priSpellCooldown.fillAmount = 1 - (priCooldown._currentTime / priCooldown._duration);
        if(priCooldown.IsFinished()) priSpellCooldown.fillAmount = 0f;
        if(!secCooldown.IsFinished()) secSpellCooldown.fillAmount = 1 - (secCooldown._currentTime / secCooldown._duration);
        if(secCooldown.IsFinished()) secSpellCooldown.fillAmount = 0f;
    }

    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
