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

        StartNewGame();
    }

    private void StartNewGame()
    {
        NPC[] nPCs = Resources.LoadAll<NPC>("NPCs");
        foreach (NPC npc in nPCs)
        {
            npc.hasDonePresentation = false;
        }

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
        DialogueManager.instance.StartDialogue(dialogue);
    }

    public void AddUpcomingDialogue(Dialogue dialogue)
    {
        upcomingDialogues.Insert(Random.Range(0, upcomingDialogues.Count - 1), dialogue);
    }

    public void AddUpcomingDialogueAtStart(Dialogue dialogue)
    {
        upcomingDialogues.Insert(0, dialogue);
    }
}
