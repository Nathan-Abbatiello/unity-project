using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : MonoBehaviour, IHealthComponent
{
    private float currentHealth;
    private float maxHP;

    private void Update(){
        CheckDeath();
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
}
