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
            rb.linearVelocityY = jumpForce;
            jumped = true;
        }

        if(jumped)
        {
            print(rb.linearVelocityY);
            if(rb.linearVelocityY <= 1f)
            {
                rb.linearVelocityY = 0;
                jumped = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            if(!isGrounded)
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            if (isGrounded)
            {
                isGrounded = false;
            }
        }
    }
}
