using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogueData;
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private Sprite humanSprite, spritSprite;
    private TextMeshProUGUI textMesh;
    private Image portraitSprite;
    private Coroutine currentRoutine;
    private int index;
    private bool isTyping, isDone;
    [SerializeField] bool zoomCam;
    [SerializeField] CamaraMovement[] movements;
    [SerializeField] Player2MovementScript movementPlayer1;
    [SerializeField] PlayerMovementScript movementPlayer2;


    private void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
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
        if (dialogueObject)
            dialogueObject.SetActive(true);
        textMesh = dialogueObject.GetComponentInChildren<TextMeshProUGUI>();
        portraitSprite = dialogueObject.GetComponentsInChildren<Image>()[1];

        if(zoomCam)
        {
            foreach(CamaraMovement movement in movements)
            {
                movement.ZoomIn();
            }

            movementPlayer1.enabled = false;
            movementPlayer2.enabled = false;
        }

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
        string tempLine = dialogueData.dialogueEntries[index].dialogueLine;
        if (tempLine.Contains("[Spirit]"))
            tempLine = tempLine.Replace("[Spirit]", "Luna");
        if (tempLine.Contains("[Girl]"))
            tempLine = tempLine.Replace("[Girl]", "Serena");
        textMesh.text = tempLine;
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
            portraitSprite.sprite = dialogueData.GirlExpressionPortraitSet.expressionPortraitSets[(int)dialogueData.dialogueEntries[index].linePortrait].sprite;
            if (dialogueObject.GetComponentsInChildren<TextMeshProUGUI>().Length == 2)
            {
                if (dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1])
                    dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Serena";
            }
        }
        else
        {
            portraitSprite.sprite = dialogueData.SpiritExpressionPortraitSet.expressionPortraitSets[(int)dialogueData.dialogueEntries[index].linePortrait].sprite;
            if (dialogueObject.GetComponentsInChildren<TextMeshProUGUI>().Length == 2)
            {
                if (dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1])
                    dialogueObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Luna";
            }
        }
    }

    private void StopDialogue()
    {
        dialogueObject.SetActive(false);
        isDone = true;
        if (zoomCam)
        {
            foreach (CamaraMovement movement in movements)
            {
                movement.ZoomOut();
            }
            movementPlayer1.enabled = true;
            movementPlayer2.enabled = true;
        }
    }

    public bool IsDone() { return isDone; }
}
