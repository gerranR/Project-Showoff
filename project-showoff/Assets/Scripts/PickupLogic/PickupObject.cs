using UnityEngine;

public abstract class PickupObject : MonoBehaviour
{
    public enum PickupPermission { HumanOnly, SpiritOnly, Both }
    [SerializeField] PickupPermission allowedPickup = PickupPermission.Both;
    [SerializeField] public bool isThrowable = true;
    public abstract string HeldLayerName { get; }
    public abstract string DroppedLayerName { get; }

    protected Rigidbody2D rb;
    protected Collider2D col;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public bool CanBePickedUpBy(string tag)
    {
        if (allowedPickup == PickupPermission.Both) return true;
        if (allowedPickup == PickupPermission.HumanOnly && tag == "Human") return true;
        if (allowedPickup == PickupPermission.SpiritOnly && tag == "Spirit") return true;
        return false;
    }

    public void Pickup(Transform holdPoint, int direction)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.freezeRotation = true;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.layer = LayerMask.NameToLayer(HeldLayerName);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    public void Drop(Vector2 throwForce, int direction, Vector2 holderVelocity, Transform holdPoint)
    {
        transform.SetParent(null);
        gameObject.layer = LayerMask.NameToLayer(DroppedLayerName);
        rb.freezeRotation = false;
        transform.position = holdPoint.position;

        if (isThrowable)
        {
            Vector2 force = new Vector2(
                throwForce.x * direction,
                throwForce.y + holderVelocity.y * 0.5f
            );

            rb.linearVelocity = force;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
