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

    // public Transform castPoint;
    public List<Transform> castPoints = new List<Transform>();

    private Vector3 mouseWorldPosition;

    private Spell childObject;

    private Spell activeSpell;
    
    // cooldowns
    CooldownTimer priCooldown, secCooldown, cast3Timer, cast4Timer;

    public CharacterController _CharacterController;
    public PlayerControls controls;  

    // UI DIsplay 
    [SerializeField] private TextMeshProUGUI profileName;
    [SerializeField] private TextMeshProUGUI priSpellName;
    [SerializeField] private Image priSpellCooldown;
    [SerializeField] private TextMeshProUGUI secSpellName;
    [SerializeField] private Image secSpellCooldown;
    [SerializeField] private Image cast3Cooldown;
    [SerializeField] private Image cast4Cooldown;



    // Start is called before the first frame update
    void Awake()
    {
        // player controls
        controls = new PlayerControls();
        //  get components
        thirdPersonController = GetComponent<ThirdPersonController>();
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

        // Initial Cooldown timers setup
        priSpellCooldown.fillAmount = 0f;
        priCooldown = new CooldownTimer();
        secSpellCooldown.fillAmount = 0f;
        secCooldown = new CooldownTimer();
        cast3Cooldown.fillAmount = 0f;
        cast3Timer = new CooldownTimer();
        cast4Cooldown.fillAmount = 0f;
        cast4Timer = new CooldownTimer();

    }

    void Update()
    {
        // Run spell cooldown timers  
        priCooldown.RunTimer();
        secCooldown.RunTimer();
        cast3Timer.RunTimer();
        cast4Timer.RunTimer();
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
        controls.Player.Cast3.performed += Cast3Input;
        controls.Player.Cast4.performed += Cast4Input;
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
            Cast(activeProfile.profileSpell1);
            priCooldown.ResetTimer(activeProfile.profileSpell1.SpellToCast.coolDown);
        }   
    }
    private void SecondaryCast(InputAction.CallbackContext context){
        if(secCooldown.IsFinished()){
            Cast(activeProfile.profileSpell2);
            secCooldown.ResetTimer(activeProfile.profileSpell2.SpellToCast.coolDown);
        }
    }

    private void Cast3Input(InputAction.CallbackContext context){
        if(cast3Timer.IsFinished()){
            Cast(activeProfile.profileSpell3);
            cast3Timer.ResetTimer(activeProfile.profileSpell3.SpellToCast.coolDown);
        }
    }

    private void Cast4Input(InputAction.CallbackContext context){
        if(cast4Timer.IsFinished()){
            Cast(activeProfile.profileSpell4);
            cast4Timer.ResetTimer(activeProfile.profileSpell4.SpellToCast.coolDown);
        }
    }

    IEnumerator SpellSpawnDelay(float delay, Spell spellToCast){
        yield return new WaitForSeconds(delay);
        // set cast point for spell
        spellToCast.castPoint =  setCastPoint(spellToCast);

        // set aim direction
        Vector3 aimDir = (mouseWorldPosition - spellToCast.castPoint.position).normalized;
        // Vector3 aimDir = (mouseWorldPosition - (spellToCast.castPoint.position-spellToCast.SpellToCast.SpawnOffset)).normalized;

         // Spawn spell
        childObject = Instantiate(spellToCast, spellToCast.castPoint.position, Quaternion.LookRotation(aimDir, Vector3.up) );
        Physics.IgnoreCollision(childObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Things that are done during casting duration
    void Casting(Spell currentSpell){
        // Disable player movement
        if(!currentSpell.SpellToCast.playerCanMove) _CharacterController.enabled = false;
        // Increment cast timer
        currentCastTimer += Time.deltaTime;
        if(currentCastTimer > currentSpell.SpellToCast.coolDown) isCastingMagic = false;
    }

    // Return position for spell to spawn from the spellscriptobj
    private Transform setCastPoint(Spell currSpell){
        for (int i = 0; i < castPoints.Count; i++)
        {
            if(castPoints[i].name == currSpell.SpellToCast.castPoint){
                return castPoints[i].transform;
            }
        }
        return null;
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

        priSpellName.text = activeProfile.profileSpell1.SpellToCast.spellName;
        secSpellName.text = activeProfile.profileSpell2.SpellToCast.spellName;
    }

    private void DisplayMagicStats(){
        if(!priCooldown.IsFinished()) priSpellCooldown.fillAmount = 1 - (priCooldown._currentTime / priCooldown._duration);
        if(priCooldown.IsFinished()) priSpellCooldown.fillAmount = 0f;

        if(!secCooldown.IsFinished()) secSpellCooldown.fillAmount = 1 - (secCooldown._currentTime / secCooldown._duration);
        if(secCooldown.IsFinished()) secSpellCooldown.fillAmount = 0f;

        if(!cast3Timer.IsFinished()) cast3Cooldown.fillAmount = 1 - (cast3Timer._currentTime / cast3Timer._duration);
        if(cast3Timer.IsFinished()) cast3Cooldown.fillAmount = 0f;

        if(!cast4Timer.IsFinished()) cast4Cooldown.fillAmount = 1 - (cast4Timer._currentTime / cast4Timer._duration);
        if(cast4Timer.IsFinished()) cast4Cooldown.fillAmount = 0f;
    }

    // GUI text modifiers
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
