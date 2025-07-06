using UnityEngine;

public abstract class PickupObject : MonoBehaviour
{
    public enum PickupPermission { HumanOnly, SpiritOnly, Both }
    [SerializeField] PickupPermission allowedPickup = PickupPermission.Both;
    [SerializeField] public bool isThrowable = true;
    public abstract string HeldLayerName { get; }
    public abstract string DroppedLayerName { get; }

    bool isPickedup = false;

    protected Rigidbody2D rb;
    protected Collider2D col;
    private LineRenderer lr;

    [SerializeField] float throwForceMultiplyer;
    public int invertThrow = 1;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (isPickedup && (Input.GetAxis("TorchAimHorizontal") != 0f || Input.GetAxis("TorchAimVertical") != 0f))
        {
            Vector2 dragEndPos = new Vector2(Input.GetAxis("TorchAimHorizontal"), -Input.GetAxis("TorchAimVertical"));
            Vector2 _velocity = dragEndPos * throwForceMultiplyer;

            Vector2[] trajectory = plot(rb, (Vector2)transform.position, _velocity, 500);
            lr.positionCount = trajectory.Length;
            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = trajectory[i];
                positions[i].z = GetParentsZ(gameObject);
            }
            positions[0] = transform.position;

            lr.SetPositions(positions);
        }
        else
        {
            lr.positionCount = 0;
        }
    }

    public Vector2[] plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timeStep * timeStep;
        float drag = 1f - timeStep * rigidbody.linearDamping;
        Vector2 moveStep = velocity * timeStep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }
        return results;
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
        isPickedup = true;

        transform.SetParent(holdPoint);
        rb.MovePosition(Vector3.zero);
        transform.rotation = Quaternion.identity;
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
        rb.MovePosition(holdPoint.position);
        isPickedup = false;

        if (isThrowable)
        {
            Vector2 force = new Vector2(Input.GetAxis("TorchAimHorizontal"), -Input.GetAxis("TorchAimVertical")) * throwForceMultiplyer * invertThrow;

/*            Vector2 force = new Vector2(
                throwForce.x * direction,
                throwForce.y + holderVelocity.y * 0.5f
            );*/

            rb.linearVelocity = force;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private float GetParentsZ(GameObject child)
    {
        if (child.transform.parent != null)
        {
            GameObject parent = child.transform.parent.gameObject;
            return parent.transform.localPosition.z + GetParentsZ(parent);
        }
        return 0;
    }
}
