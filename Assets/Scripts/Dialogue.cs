using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/Dialogue")]
[System.Serializable]
public class Dialogue : ScriptableObject
{
    public NPC npcTalking;
    [TextArea(3, 3)]
    public string[] sentences;
    public DialogueChoice[] dialogueChoices = new DialogueChoice[4];
}

[System.Serializable]
public class DialogueChoice
{
    [TextArea(2, 4)]
    public string choice;

    //effects
    public StatChange[] changes;

    public Dialogue followUpDialogue;

    [TextArea(2, 4)]
    public string response;
}

public enum Stat { Money, Population, Happiness, Army, Naval}
[System.Serializable]
public struct StatChange
{
    public Stat stat;
    public int change;
}
