using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New StatusEffect", menuName = "StatusEffect")]

public class StatusEffectData : ScriptableObject
{
  // A scriptable object that contains spell attributes
  [Header("Attributes")]

  public float healthOverTime;
  public float lifetime;
  public float tickSpeed; 
  public float movementPenalty;

  public GameObject effectParticles;

}
