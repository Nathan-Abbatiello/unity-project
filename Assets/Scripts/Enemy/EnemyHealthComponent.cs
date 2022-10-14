using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthComponent : MonoBehaviour, IHealthComponent
{
    private float currentHealth;
    private float maxHP;
    public Slider healthBar;  
    private Camera _cam;

    private void Awake(){
        _cam = Camera.main;
    }

    private void Update(){
        CheckDeath();
        DisplayStats();
    }

    public void SetMaxHealth(float maxHealth){
        currentHealth = maxHealth;
        maxHP = maxHealth;
    }

    public void AlterHealth(float hpAmount)
    {
        currentHealth += hpAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHP);
    }

    public float GetCurrentHealth(){
        return currentHealth;
    }

    public void CheckDeath(){
        if(currentHealth<= 0 ) Destroy(this.gameObject);
    }

    public void DisplayStats()
    {
        healthBar.value = GetCurrentHealth() / maxHP;
        healthBar.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
