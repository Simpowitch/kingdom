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

    [SerializeField] Button continueButton;
    [SerializeField] GameObject choiceBox = null;
    [SerializeField] Button[] choiceButtons = new Button[4];


    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with: " + dialogue.npcTalking.npcName);

        //Save the started dialogue
        activeDialogue = dialogue;

        //Clear any leftover sentences
        sentences.Clear();

        //Buttons
        continueButton.interactable = true;
        HideChoiceButtons();

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
            choiceButtons[i].interactable = !(activeDialogue.dialogueChoices[i].moneyChange < 0 && Mathf.Abs(activeDialogue.dialogueChoices[i].moneyChange) > Player.instance.CheckGold());
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
        Player.instance.ChangeGold(choice.moneyChange);
        Player.instance.ChangeHappiness(choice.happinessChange);
        Player.instance.ChangePopulation(choice.populationChange);


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
