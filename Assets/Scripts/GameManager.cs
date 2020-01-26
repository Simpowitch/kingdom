using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
        }
        else
        {
            Debug.LogWarning("another instance of " + this + " was tried to be created, but is now destroyed from gameobject" + this.gameObject.name);
            Destroy(this);
        }
    }
    #endregion

    public List<Dialogue> upcomingDialogues = new List<Dialogue>();
    Dialogue[] allDialogues;

    private void Start()
    {
        allDialogues = Resources.LoadAll<Dialogue>("Dialogues");

        InitiateNextDialogue();
    }

    public void InitiateNextDialogue()
    {
        if (upcomingDialogues.Count == 0)
        {
            InitiateRandomDialogue();
        }
        else
        {
            StartDialogue(upcomingDialogues[0]);
            upcomingDialogues.RemoveAt(0);
        }
    }


    private void InitiateRandomDialogue()
    {
        int index = Random.Range(0, allDialogues.Length);
        StartDialogue(allDialogues[index]);
    }

    private void StartDialogue(Dialogue dialogue)
    {
        if (dialogue.npcTalking.presentation != null && !dialogue.npcTalking.hasDonePresentation)
        {
            dialogue.npcTalking.hasDonePresentation = true;
            DialogueManager.instance.StartDialogue(dialogue.npcTalking.presentation);
        }
        else
        {
            DialogueManager.instance.StartDialogue(dialogue);
        }
    }
}
