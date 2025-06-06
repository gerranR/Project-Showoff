using System.Collections;
using TMPro;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogueData;
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private Sprite humanSprite, spritSprite;
    private TextMeshPro textMesh;
    private SpriteRenderer portraitSprite;
    private Coroutine currentRoutine;
    private int index;
    private bool isTyping;

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
        dialogueObject.SetActive(true);
        textMesh = dialogueObject.GetComponentInChildren<TextMeshPro>();
        portraitSprite = dialogueObject.GetComponentsInChildren<SpriteRenderer>()[1];
        AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        if (index == dialogueData.dialogueEntries.Length)
        {
            StopDialogue();
            return;
        }            
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
        }
        else
        {
            portraitSprite.sprite = spritSprite;
        }
    }

    private void StopDialogue()
    {
        dialogueObject.SetActive(false);
    }
}
