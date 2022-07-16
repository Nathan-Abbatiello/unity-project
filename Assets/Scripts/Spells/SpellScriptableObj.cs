using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class SpellScriptableObj : ScriptableObject
{
  public float lifetime = 2f;
  public float speed = 15f; 
  public float damage = 10f;
  public float spellRadius = 1.5f;

}
