using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] List<GameObject> textObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Human")
            ToggleVisibility();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Human")
            ToggleVisibility();
    }
    void ToggleVisibility()
    {
        foreach (GameObject textObject in textObjects)
        {
            if (textObject != null) textObject.SetActive(!textObject.activeSelf);
        }
    }
}
