using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Rigidbody2D rb;

    private bool isGrounded;
    private bool jumped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 force = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);

        rb.linearVelocityX = force.x;
    }

    private void Update()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            Jump();
        }

        if(jumped)
        {
            if(rb.linearVelocityY <= 1f)
            {
                rb.linearVelocityY = 0;
                jumped = false;
            }
        }
    }

    public void Jump()
    {
        rb.linearVelocityY = jumpForce;
        jumped = true;
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            if(collision.GetContact(0).normal == Vector2.up && !isGrounded)
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            if (collision.contactCount > 0)
            {
                if (isGrounded && collision.GetContact(0).collider == null)
                {
                    isGrounded = false;
                }
            }
            else if(collision.contactCount == 0)
            {
                if(isGrounded)
                {
                    isGrounded = false;
                }
            }
        }
    }

    public bool hasJumped()
    {
        return !isGrounded;
    }
}
