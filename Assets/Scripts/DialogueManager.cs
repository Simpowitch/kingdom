using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager instance;
    private void Awake()
    {
        if (DialogueManager.instance == null)
        {
            DialogueManager.instance = this;
        }
        else
        {
            Debug.LogWarning("another instance of " + this + " was tried to be created, but is now destroyed from gameobject" + this.gameObject.name);
            Destroy(this);
        }
    }
    #endregion

    private Dialogue activeDialogue;
    private Queue<string> sentences = new Queue<string>();
    [SerializeField] TextMeshProUGUI npcNameText = null;
    [SerializeField] TextMeshProUGUI dialogueText = null;

    [SerializeField] Button continueButton = null;
    [SerializeField] GameObject choiceBox = null;
    [SerializeField] Button[] choiceButtons = new Button[4];


    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with: " + dialogue.npcTalking.npcName);

        //Clear any leftover sentences
        sentences.Clear();

        //Save the started dialogue
        activeDialogue = dialogue;


        //Buttons
        continueButton.interactable = true;
        HideChoiceButtons();

        //if havent done presentation, do that first
        if (!dialogue.npcTalking.hasDonePresentation && dialogue.npcTalking.presentation.Length > 0)
        {
            foreach (string sentence in dialogue.npcTalking.presentation)
            {
                sentences.Enqueue(sentence);
            }
            dialogue.npcTalking.hasDonePresentation = true;
        }

        //Save the sentences
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        //Display the NPC name
        npcNameText.text = dialogue.npcTalking.npcName;

        //Start first dialogue
        DisplayNextSentence();
    }

    public void Continue()
    {
        if (activeDialogue)
        {
            DisplayNextSentence();
        }
        else
        {
            EndDialogue();
        }
    }

    //Display one sentence
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, sentences.Count == 0));
    }


    //Display the sentence letter by letter
    [SerializeField] float writeSpeed = 0.01f;
    IEnumerator TypeSentence(string sentence, bool responseNeeded)
    {
        dialogueText.text = "";

        //Add player "name" sometimes
        int rng = Random.Range(0, 4);
        if (rng == 0)
        {
            //Start with name of player
            sentence = Player.instance.GetName() + " " + sentence;
        }

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(writeSpeed);
        }

        //If last sentence is provided, player need to respond
        if (responseNeeded)
        {
            ShowChoices();
        }
    }

    //Display the player choices
    private void ShowChoices()
    {
        //If there are no choices to display
        if (activeDialogue.dialogueChoices[0].choice == "")
        {
            continueButton.interactable = true;
            activeDialogue = null;
            return;
        }

        Debug.Log("Choice needed");

        //Hide continue button
        continueButton.interactable = false;

        //Show other buttons
        choiceBox.SetActive(true);

        for (int i = 0; i < activeDialogue.dialogueChoices.Length; i++)
        {
            if (activeDialogue.dialogueChoices[i].choice == "")
            {
                break;
            }
            choiceButtons[i].gameObject.SetActive(true);

            choiceButtons[i].interactable = true;

            //If change is money and we can't afford the choice, make the button non interactable
            foreach (StatChange statChange in activeDialogue.dialogueChoices[i].changes)
            {
                if (statChange.stat == Stat.Money && statChange.change < 0 && Mathf.Abs(statChange.change) > Player.instance.CheckGold())
                {
                    choiceButtons[i].interactable = false;
                }
            }
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = activeDialogue.dialogueChoices[i].choice;
        }
    }

    private void HideChoiceButtons()
    {
        choiceBox.SetActive(false);
        foreach (var item in choiceButtons)
        {
            item.interactable = false;
            item.gameObject.SetActive(false);
        }
    }

    public void UseDialogueChoice(int index)
    {
        Debug.Log("Choice index " + index + " was used");

        DialogueChoice choice = activeDialogue.dialogueChoices[index];

        //Display answer/response
        StartCoroutine(TypeSentence(choice.response, false));

        HideChoiceButtons();

        //Perform choice actions
        Player.instance.ChangeStat(choice.changes);
        if (choice.followUpDialogue)
        {
            GameManager.instance.AddUpcomingDialogue(choice.followUpDialogue);
        }

        //End dialogue button enable
        activeDialogue = null;
        continueButton.interactable = true;
    }

    public void EndDialogue()
    {
        Debug.Log("Dialogue ended");
        GameManager.instance.InitiateNextDialogue();
    }
}
