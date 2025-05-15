using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    [SerializeField] List<GameObject> platforms = new List<GameObject>();
    [SerializeField] bool standingOnButton;

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
        foreach (GameObject platform in platforms)
        {
          if(platform!=null)  platform.SetActive(!platform.activeSelf);
        }
    }
}