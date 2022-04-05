using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueGroup;
    [SerializeField] private Image _speakerImage;
    [SerializeField] private TextMeshProUGUI _speakerName;
    [SerializeField] private TextMeshProUGUI _speakerDialogue;
    [SerializeField] private float _letterTypeWaitTime = 0.0175f;

    private Queue<string> dialogueQueue = new Queue<string>();
    private DialogueData currentDialogueData;
    private AudioManager audioManager;
    private int currentSentence;

    private void Awake() => audioManager = FindObjectOfType<AudioManager>();

    //Turn on and off the dialogue group
    public void TurnOnDialogueGroup() => _dialogueGroup.SetActive(true);
    public void TurnOffDialogueGroup() => _dialogueGroup.SetActive(false);

    //Opens the dialogue menu,sets the character image and name, and makes a queue for sentences (triggered by DialogueTrigger script)
    public void StartDialogue(DialogueData dialogue)
    {
        GameManager.Instance.ToggleFreezePlayer();
        dialogueQueue.Clear();
        currentSentence = 0;
        currentDialogueData = dialogue;

        foreach (string sentence in dialogue.sentences)
            dialogueQueue.Enqueue(sentence);

        TurnOnDialogueGroup();
        DisplayNextSentence();
    }

    //Displays individual sentences in the dialogue queue and turns off the system if the dialogue is complete (triggered by UI button)
    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            GameManager.Instance.ToggleFreezePlayer();
            TurnOffDialogueGroup();
            return;
        }

        _speakerImage.sprite = currentDialogueData.speakerSprites[currentDialogueData.currentSentenceSpeaker[currentSentence]];
        _speakerName.text = currentDialogueData.speakerNames[currentDialogueData.currentSentenceSpeaker[currentSentence]];
        audioManager.UISound(currentDialogueData.speakerSounds[currentDialogueData.currentSentenceSpeaker[currentSentence]]);

        string sentence = dialogueQueue.Dequeue();
        StopAllCoroutines(); //If the player presses next before the last sentence has finished, stop the previous sentence
        StartCoroutine(TypeSentence(sentence));

        currentSentence++;
    }

    //Slowly "types" out the characters in the sentence rather than all at once.
    IEnumerator TypeSentence (string sentence)
    {
        _speakerDialogue.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _speakerDialogue.text += letter;
            yield return new WaitForSeconds(_letterTypeWaitTime);
        }
    }
}
