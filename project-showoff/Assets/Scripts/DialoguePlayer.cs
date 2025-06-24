using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;

    [SerializeField] private DialogueScriptableObject dialogueData;
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private Sprite humanSprite, spritSprite;
    private TextMeshProUGUI textMesh;
    private Image portraitSprite;
    private Coroutine currentRoutine;
    private int index;
    private bool isTyping, isDone;

    private void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                DisplayFullLine();
            }
            else
                AdvanceDialogue();
        }
    }

    private void StartDialogue()
    {
        OnDialogueStarted?.Invoke();

        if (dialogueObject)
            dialogueObject.SetActive(true);
        textMesh = dialogueObject.GetComponentInChildren<TextMeshProUGUI>();
        portraitSprite = dialogueObject.GetComponentsInChildren<Image>()[1];
        if (portraitSprite)
            AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        if (index == dialogueData.dialogueEntries.Length)
        {
            StopDialogue();
            return;
        }
        if (portraitSprite)
            SetSprite();
        textMesh.text = dialogueData.dialogueEntries[index].dialogueLine;
        textMesh.maxVisibleCharacters = 0;
        currentRoutine = StartCoroutine(TypeRoutine());
    }

    private IEnumerator TypeRoutine()
    {
        isTyping = true;
        if (dialogueData.dialogueEntries[index].dialogueLine.Length <= textMesh.maxVisibleCharacters)
        {
            isTyping = false;
            index++;
            yield break;
        }
        else
        {
            textMesh.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.05f);
            currentRoutine = StartCoroutine(TypeRoutine());
        }
    }

    private void DisplayFullLine()
    {
        isTyping = false;
        StopCoroutine(currentRoutine);
        textMesh.maxVisibleCharacters = dialogueData.dialogueEntries[index].dialogueLine.Length;
        index++;
    }

    private void SetSprite()
    {
        if (dialogueData.dialogueEntries[index].lineSpeaker == DialogueEntry.Speaker.Human)
        {
            portraitSprite.sprite = humanSprite;
            if (dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1])
                dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Human";
        }
        else
        {
            portraitSprite.sprite = spritSprite;
            if (dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1])
                dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Spirit";
        }
    }

    private void StopDialogue()
    {
        dialogueObject.SetActive(false);
        isDone = true;
        OnDialogueEnded?.Invoke();
    }

    public bool IsDone() { return isDone; }
}
