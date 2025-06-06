using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] dialogueText dialogueText = new dialogueText();
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] GameObject dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        //make opening button based
    }

    public void ShowText()
    {
        dialogue.SetActive(true);
        text.text = dialogueText.text;
    }

    public void hideText()
    {
        dialogue.SetActive(false);
        text.text = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dialogue.activeSelf)
        {
            if (collision.tag == "Human" || collision.tag == "Spirit")
            {
                ShowText();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (dialogue.activeSelf)
        {
            if (collision.tag == "Human" || collision.tag == "Spirit")
            {
                hideText();
            }
        }
    }
}
