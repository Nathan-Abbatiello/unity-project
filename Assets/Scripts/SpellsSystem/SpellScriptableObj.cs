using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellScriptableObj : ScriptableObject
{
  // A scriptable object that contains spell attributes
  [Header("Attributes")]

  public float damage = 10f;
  public float lifetime = 2f;
  public float spellRadius = 1.5f;
  public float speed = 15f; 

  public float coolDown = 1f;

  [Space(10)]
  [Header("Behaviour")]
  public string castPoint = "RHCastPoint";
  public bool stickToCastPoint = false;

  [Range(0, 30)]
  public int stickStrength;
  public bool destroyOnImpact = true;
  public Vector3 SpawnOffset = new Vector3(0,0,0);

  [Space(10)]
  [Header("Player Effects")]
  public bool playerCanMove = true;

  public Transform hitEffect;

  [Space(10)]
  [Header("Animation")]

  public float spawnDelay = 0f; 
  public float ZeroVelocityTime = 0f;

  [Space(10)]
  [Header("Display")]
  public string spellName = "new spell";
  public string description = "Item Description";
  public Texture uiIcon;



}
