using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellScriptableObj : ScriptableObject
{
  [Header("Attributes")]

  public float damage = 10f;
  public float lifetime = 2f;
  public float spellRadius = 1.5f;
  public float speed = 15f; 

  [Space(10)]
  [Header("Behaviour")]
  public bool stickToCastPoint = false;
  public bool destroyOnImpact = true;

  [Space(10)]
  [Header("Player Effects")]
  public bool playerCanMove = true;
  // public status activestatus = ""



}
