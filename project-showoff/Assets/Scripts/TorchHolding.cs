using UnityEngine;

public class TorchHolding : MonoBehaviour
{
    [Header("Torch Settings")]
    [SerializeField] Transform torchHoldPoint;
    [SerializeField] float pickupRange = 1f;
    [SerializeField] Vector2 throwForce = new Vector2(5f, 2f);
    [SerializeField] float throwForceMultiplyer = 1;

    [Header("Layer Settings")]
    [SerializeField] LayerMask torchLayerMask;
    [SerializeField] string heldTorchLayerName = "HeldTorch";
    [SerializeField] string droppedTorchLayerName = "Torch";

    bool isTouchingBrazier = false;

    private GameObject heldTorch = null;
    private Rigidbody2D rb;
    private int lastDirection = 1;

    private int heldTorchLayer;
    private int droppedTorchLayer;

    [Tooltip("make -1 to invert")]
    [SerializeField] private int invertThrow = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        heldTorchLayer = LayerMask.NameToLayer(heldTorchLayerName);
        droppedTorchLayer = LayerMask.NameToLayer(droppedTorchLayerName);
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0) lastDirection = -1;
        else if (Input.GetAxis("Horizontal") > 0) lastDirection = 1;

        if (Input.GetButtonDown("pickUp"))
        {
            if (heldTorch == null) TryPickupTorch();
            else ThrowTorch();
        }

        if (heldTorch != null) UpdateHeldTorch();

        UpdateHoldPointDirection();
    }

    void TryPickupTorch()
    {
        if (isTouchingBrazier)
        {
            GameObject foundTorch = GameObject.FindGameObjectWithTag("Torch");
            if (foundTorch != null && foundTorch.layer == droppedTorchLayer)
            {
                heldTorch = foundTorch;
                AttachTorch(heldTorch);
                return;
            }
        }

        Collider2D torchColl = Physics2D.OverlapCircle(transform.position, pickupRange, torchLayerMask);
        if (torchColl != null)
        {
            heldTorch = torchColl.gameObject;
            AttachTorch(heldTorch);
        }
    }
    void AttachTorch(GameObject torch)
    {
        Rigidbody2D torchRb = torch.GetComponent<Rigidbody2D>();

        torchRb.linearVelocity = Vector2.zero;
        torchRb.angularVelocity = 0f;
        torchRb.rotation = 0f;
        torchRb.freezeRotation = true;

        torch.transform.SetParent(torchHoldPoint);
        torch.transform.localPosition = Vector3.zero;
        torch.transform.localRotation = Quaternion.identity;
        torch.layer = heldTorchLayer;

        UpdateHeldTorch();
    }

    void ThrowTorch()
    {
        Rigidbody2D torchRb = heldTorch.GetComponent<Rigidbody2D>();
        Collider2D torchColl = heldTorch.GetComponent<Collider2D>();

        heldTorch.transform.SetParent(null);
        heldTorch.layer = droppedTorchLayer;

        torchRb.freezeRotation = false;

        //Vector2 force = new Vector2((Input.GetAxis("TorchAimHorizontal") * invertThrow) * (throwForce.x * lastDirection), (Input.GetAxis("TorchAimVertical") * invertThrow) * (throwForce.y + rb.linearVelocity.y * 0.5f));
        Vector2 force = new Vector2(Input.GetAxis("TorchAimHorizontal"), -Input.GetAxis("TorchAimVertical")) * throwForceMultiplyer * invertThrow ;

/*        Vector2 force = new Vector2(
            throwForce.x * lastDirection,
            throwForce.y + rb.linearVelocity.y * 0.5f
        )*/;

        torchRb.transform.position = torchHoldPoint.position;
        torchRb.linearVelocity = force;

        heldTorch = null;
    }

    void UpdateHeldTorch()
    {
        Vector3 offset = new Vector3(lastDirection * 0.5f, 0f, 0f);
        heldTorch.transform.position = torchHoldPoint.position + offset;

        Vector3 scale = heldTorch.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * lastDirection;
        heldTorch.transform.localScale = scale;
    }

    void UpdateHoldPointDirection()
    {
        Vector3 holdPos = torchHoldPoint.localPosition;
        holdPos.x = Mathf.Abs(holdPos.x) * lastDirection;
        torchHoldPoint.localPosition = holdPos;
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