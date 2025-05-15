using UnityEngine;

public class TorchHandler : MonoBehaviour
{
    [SerializeField] Transform torchHoldPoint;
    [SerializeField] float pickupRange;
    public Vector2 throwForce = new Vector2(5f, 2f);

    [SerializeField] LayerMask torchLayer;
    Rigidbody2D rb;

    private GameObject heldTorch = null;

    private int lastDirection = 1; // 1 = right, -1 = left

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) lastDirection = -1;
        else if (Input.GetKeyDown(KeyCode.D)) lastDirection = 1;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (heldTorch == null)
                TryPickupTorch();
            else
                ThrowTorch();
        }

        if (heldTorch != null)
            UpdateTorch();

        Vector3 holdPos = torchHoldPoint.localPosition;
        holdPos.x = Mathf.Abs(holdPos.x) * lastDirection;
        torchHoldPoint.localPosition = holdPos;
    }
    void TryPickupTorch()
    {
        Collider2D torchColl = Physics2D.OverlapCircle(transform.position, pickupRange, torchLayer);

        if (torchColl != null)
        {
            heldTorch = torchColl.gameObject;

            Rigidbody2D torchRb = heldTorch.GetComponent<Rigidbody2D>();

            torchColl.enabled = false;
            torchRb.linearVelocity = Vector2.zero;
            torchRb.angularVelocity = 0f;
            torchRb.rotation = 0f;
            torchRb.freezeRotation = true;

            heldTorch.transform.SetParent(torchHoldPoint);
            heldTorch.transform.localPosition = Vector3.zero;
            heldTorch.transform.localRotation = Quaternion.identity;

            Vector3 torchScale = heldTorch.transform.localScale;
            torchScale.x = Mathf.Abs(torchScale.x) * lastDirection;
            heldTorch.transform.localScale = torchScale;
        }
    }
    void ThrowTorch()
    {
        Rigidbody2D torchRb = heldTorch.GetComponent<Rigidbody2D>();
        Collider2D torchColl = heldTorch.GetComponent<Collider2D>();

        heldTorch.transform.SetParent(null);
        torchColl.enabled = true;
        torchRb.freezeRotation = false;

        Vector2 force = new Vector2(
            throwForce.x * lastDirection,
            throwForce.y + rb.linearVelocity.y * 0.5f
        );

        torchRb.transform.position = torchHoldPoint.position;
        torchRb.linearVelocity = force;

        heldTorch = null;
    }

    void UpdateTorch()
    {
        Vector3 offset = new Vector3(lastDirection * 0.5f, 0f, 0f);

        heldTorch.transform.position = torchHoldPoint.position + offset;

        Vector3 torchScale = heldTorch.transform.localScale;
        torchScale.x = Mathf.Abs(torchScale.x) * lastDirection;
        heldTorch.transform.localScale = torchScale;
    }

}