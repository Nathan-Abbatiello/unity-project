using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    private void Awake(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage){
        // currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if(currentHealth<= 0 ) Destroy(this.gameObject);
    }
}
