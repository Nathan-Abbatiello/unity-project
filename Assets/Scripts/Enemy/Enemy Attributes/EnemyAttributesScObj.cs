using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Enemy Attributes", menuName = "EnemyAttributes")]

public class EnemyAttributesScObj : ScriptableObject
{
  // A scriptable object that contains spell attributes
  [Header("Attributes")]

  public float maxHealth = 100f;
  public float damage = 10f;

  [Space(10)]
  [Header("Behaviour")]
  public float chaseRadius = 10f;
  public float attackRadius = 2f;



}
