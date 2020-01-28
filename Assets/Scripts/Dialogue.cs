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
    public int moneyChange;
    public int populationChange;
    public int happinessChange;

    public Dialogue followUpDialogue;

    [TextArea(2, 4)]
    public string response;
}
