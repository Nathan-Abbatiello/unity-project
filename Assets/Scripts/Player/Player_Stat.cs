using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player_Stat : MonoBehaviour
{
    public float maxHealth = 100; public float currentHealth; public Slider healthBar;  
    public float regenTime = 0.05f; public float tempTime; // Regeneration time 
    public bool godMode; public bool canDie;
    public PlayerInput input; 


    void Awake(){
        godMode = false;
        canDie = true;
        currentHealth = maxHealth;
        tempTime = regenTime;
        TakeDamage(50f);  
        input = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = AlterStatsDur(0.05f, currentHealth, maxHealth, true);
        if (currentHealth <= 0 && godMode == false && canDie) {
            StartCoroutine(PlayerDeath());
        }
        DisplayStats();  
    }

    public void TakeDamage(float damage){
        currentHealth = AlterStats(-damage, currentHealth, maxHealth);
        Debug.Log("take damage"+ damage);
    }

    public float AlterStats(float changeVal, float currStatVal, float maxStatVal)
    {
        // Health change
        if (currStatVal + changeVal > maxStatVal)
        { // allows current stat to be set to max value if the current stat ends in an odd number
            return currStatVal = maxStatVal;
        }
        if (currStatVal + changeVal < 0)
        {  // allows current stat to be set to zero if the current stat ends in an odd number
            return currStatVal = 0;
        }
        if (currStatVal + changeVal <= maxStatVal && currStatVal + changeVal >= 0)
        { //makes sure current stat plus change stat cant go beyond max stat or below zero
            return currStatVal += changeVal; // applies change value to the current stat
        }
        return 0f;
    }

    public float AlterStatsDur(float regenAmount, float statToChange, float maxStat,  bool active)
    {
        if (active && statToChange != maxStat)
        {
            if ((tempTime -= Time.deltaTime) <= 0)
            {
                tempTime = regenTime;
                return AlterStats(regenAmount, statToChange, maxStat);
            }
        }
        return statToChange;
    }

     void DisplayStats()
    {
        healthBar.value = currentHealth / maxHealth;
    }
      IEnumerator PlayerDeath() {
        input.actions.Disable();
        Scene scene = SceneManager.GetActiveScene();
        canDie = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }

}
