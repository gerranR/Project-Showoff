using UnityEngine;
using System.Collections.Generic;

public class BrazierLighting : MonoBehaviour
{
    protected bool isLit = false;

   protected Collider2D torchInRange = null;
    protected readonly List<Collider2D> withinRange = new();

    [SerializeField] protected GameObject lightArea;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite unlitSprite;
    [SerializeField] protected Sprite litSprite;

    public void InitializeFrom(BrazierLighting other)
    {
        this.lightArea = other.lightArea;
        this.spriteRenderer = other.spriteRenderer;
        this.unlitSprite = other.unlitSprite;
        this.litSprite = other.litSprite;
    }

    protected virtual void Start()
    {
        BrazierManager.RegisterBrazier(this);
    }

    protected virtual void Update()
    {
        if (isLit || torchInRange == null) return;

        foreach (var player in withinRange)
        {
            if (player == null) continue;

            int layer = player.gameObject.layer;

            //bool isHumanToggle = layer == LayerMask.NameToLayer("Human") && Input.GetKeyDown(KeyCode.W);
            bool isSpiritToggle = layer == LayerMask.NameToLayer("Spirit") && Input.GetButtonDown("SpiritLight");

            if (isSpiritToggle)
            {
                ActivateLight();
                break;
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isLit) return;

        int layer = other.gameObject.layer;

        if (layer == LayerMask.NameToLayer("HeldTorch"))
        {
            torchInRange = other;
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

  protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (torchInRange == other)
        {
            torchInRange = null;
        }

        withinRange.Remove(other);
    }

    public virtual void ActivateLight()
    {
        if (isLit) return;

        isLit = true;
        lightArea.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer("LitSconce");

        if (spriteRenderer && litSprite)
            spriteRenderer.sprite = litSprite;

        BrazierManager.CheckBraziers();
    }

    public virtual void DeactivateLight()
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
