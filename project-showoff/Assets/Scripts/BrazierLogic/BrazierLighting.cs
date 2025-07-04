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

    [SerializeField]private Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip lightSound;
    [SerializeField] float radius;
    [SerializeField] LayerMask layer1;


    public void InitializeFrom(BrazierLighting other)
    {
        this.lightArea = other.lightArea;
        this.spriteRenderer = other.spriteRenderer;
        this.unlitSprite = other.unlitSprite;
        this.litSprite = other.litSprite;

    }

    protected virtual void Start()
    {
        BrazierManager.instance.RegisterBrazier(this);
    }

    protected virtual void Update()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z + 5);
        Collider2D hitColliders = Physics2D.OverlapCircle(pos, radius, layer1);
        if (hitColliders != null)
        {
            TorchObject myComponent = hitColliders.GetComponent<TorchObject>();
            if (myComponent != null)
            {
                torchInRange = hitColliders;
            }   
        }

        if (isLit || torchInRange == null)  return;

        foreach (var player in withinRange)
        {
            if (player == null) continue;

            if (player.gameObject.layer == LayerMask.NameToLayer("Spirit") && Input.GetButtonDown("SpiritLight"))
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
        else if (layer == LayerMask.NameToLayer("Spirit"))
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
        else
        {
            if (withinRange.Contains(other))
                withinRange.Remove(other);
        }
    }

    public virtual void ActivateLight()
    {
        if (isLit) return;

        isLit = true;
        lightArea.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer("LitSconce");

        audioSource.PlayOneShot(lightSound);

        /*        if (spriteRenderer && litSprite)
                    spriteRenderer.sprite = litSprite;*/

        if (animator != null)
        {
            animator.SetTrigger("OpenBrazier");
        }
        BrazierManager.instance.CheckBraziers();
    }

    public virtual void DeactivateLight()
    {
        if (!isLit) return;

        isLit = false;
        lightArea.SetActive(false);

        if (spriteRenderer && unlitSprite)
            spriteRenderer.sprite = unlitSprite;

        BrazierManager.instance.CheckBraziers(); 
    }
    public bool IsLit() => isLit;
}
