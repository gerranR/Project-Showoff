using Unity.VisualScripting;
using UnityEngine;

public class DoPickupObject : MonoBehaviour
{
    [SerializeField] Transform holdPoint;
    [SerializeField] float pickupRange = 1f;
    [SerializeField] Vector2 throwForce = new(5f, 2f);
    [SerializeField] LayerMask layersWithPickupObjects;

    private PickupObject heldItem = null;
    private Rigidbody2D rb;
    private int lastDirection = 1;

    private string playerTag;
    private string pickupButton;

    private string joystickAxis;

    private bool isTouchingBrazier;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SummonSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTag = gameObject.tag;

        if (playerTag == "Human")
        {
            pickupButton = "pickUp";
            joystickAxis = "Horizontal";
        }
        else if (playerTag == "Spirit")
        {
            pickupButton = "pickUpSpirit";
            joystickAxis = "HorizontalP2";
        }
        else
        {
            pickupButton = "";
            joystickAxis = "";
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(joystickAxis))
        {
            float move = Input.GetAxis(joystickAxis);
            if (move != 0) lastDirection = (int)Mathf.Sign(move);
        }

        if (!string.IsNullOrEmpty(pickupButton) && Input.GetButtonDown(pickupButton))
        {
            if (heldItem == null) TryPickup();
            else Drop();
        }

        UpdateHeldPosition();
    }

    void TryPickup()
    {
        if (isTouchingBrazier)
        {
            GameObject foundTorch = GameObject.FindGameObjectWithTag("Torch");
            if (foundTorch != null && foundTorch.GetComponent<PickupObject>().CanBePickedUpBy(playerTag))
            {
                heldItem = foundTorch.GetComponent<PickupObject>();
                heldItem.Pickup(holdPoint, lastDirection);
                audioSource.PlayOneShot(SummonSound);
            }
            return;
        }

        Collider2D coll = Physics2D.OverlapCircle(transform.position, pickupRange, layersWithPickupObjects);
        if (coll == null || coll.GetComponent<PickupObject>() == null) return;
        var pickup = coll.GetComponent<PickupObject>();

        if (pickup != null && pickup.CanBePickedUpBy(playerTag))
        {
            heldItem = pickup;
            heldItem.Pickup(holdPoint, lastDirection);
        }
    }

    void Drop()
    {
        heldItem.Drop(throwForce, lastDirection, rb.linearVelocity, holdPoint);
        heldItem = null;
    }

    void UpdateHeldPosition()
    {
        if (heldItem == null) return;
        heldItem.transform.position = holdPoint.position + new Vector3(lastDirection * 0.5f, 0f, 0f);
        holdPoint.localPosition = new Vector3(Mathf.Abs(holdPoint.localPosition.x) * lastDirection, holdPoint.localPosition.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Brazier"))
        {
            isTouchingBrazier = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Brazier"))
        {
            isTouchingBrazier = false;
        }
    }
}