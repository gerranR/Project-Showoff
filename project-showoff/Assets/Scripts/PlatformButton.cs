using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    [SerializeField] List<GameObject> platforms = new List<GameObject>();

    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            spriteRenderer.sprite = down;
            ToggleVisibility();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            spriteRenderer.sprite = up;
            ToggleVisibility();
        }
    }
    void ToggleVisibility()
    {
        foreach (GameObject platform in platforms)
        {
          if(platform!=null)  platform.GetComponent<VineAnim>().Toggle();
        }
    }
}