using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/NPC")]
public class NPC : ScriptableObject
{
    public string npcName;
    public bool hasDonePresentation = false;
    public Dialogue presentation;
}
