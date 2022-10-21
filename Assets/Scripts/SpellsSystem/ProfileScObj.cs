using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Profile", menuName = "Profile")]

public class ProfileScObj : ScriptableObject
{
  // A scriptable object that contains spells
  [Header("Spells in Profile")]
  public string ProfileName = "NewProfile";
  public Spell PrimarySpell;
  public Spell SecondrySpell;
}
