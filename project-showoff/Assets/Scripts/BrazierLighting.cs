using UnityEngine;
using System.Collections.Generic;

public class BrazierLighting : MonoBehaviour
{
    public GameObject lightArea;
    private bool isLit = false;

    private Collider2D torchInRange = null;
    private readonly List<Collider2D> withinRange = new();

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite unlitSprite;
    [SerializeField] Sprite litSprite;


    private void Update()
    {
        if (isLit || torchInRange == null) return;

        foreach (var player in withinRange)
        {
            if (player == null) continue;

            int layer = player.gameObject.layer;

            bool isHumanToggle = layer == LayerMask.NameToLayer("Human") && Input.GetKeyDown(KeyCode.W);
            bool isSpiritToggle = layer == LayerMask.NameToLayer("Spirit") && Input.GetKeyDown(KeyCode.DownArrow);

            if (isHumanToggle || isSpiritToggle)
            {
                ActivateLight();
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLit) return;

        int layer = other.gameObject.layer;

        if (layer == LayerMask.NameToLayer("HeldTorch"))
        {
            ActivateLight();
            return;
        }
        else if (layer == LayerMask.NameToLayer("Torch"))
        {
            torchInRange = other;
        }
        else if (layer == LayerMask.NameToLayer("Human") || layer == LayerMask.NameToLayer("Spirit"))
        {
            if (!withinRange.Contains(other))
                withinRange.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (torchInRange == other)
        {
            torchInRange = null;
        }

        withinRange.Remove(other);
    }

    public void ActivateLight()
    {
        if (isLit) return;

        isLit = true;
        lightArea.SetActive(true);


        if (spriteRenderer && litSprite)
            spriteRenderer.sprite = litSprite;

        BrazierManager.CheckBraziers();
    }

    public void DeactivateLight()
    {
        if (!isLit) return;

        isLit = false;
        lightArea.SetActive(false);

        if (spriteRenderer && unlitSprite)
            spriteRenderer.sprite = unlitSprite;

        BrazierManager.CheckBraziers(); 
    }
    public bool IsLit() => isLit;
}
