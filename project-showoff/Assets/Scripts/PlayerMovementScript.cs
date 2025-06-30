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

    [SerializeField] ParticleSystem groundDust;
    [SerializeField] Animator animator;
    private float jumpStartY;
    private float maxJumpY;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip[] walkSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector2 force = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);

        rb.linearVelocityX = force.x;
        if (rb.linearVelocityX > .25 || rb.linearVelocityX < -.25)
        {
            if(!AudioSource.isPlaying)
            {
                AudioSource.PlayOneShot(walkSound[Random.Range(0, walkSound.Length)]);
            }
            animator.SetBool("Walking", true);
            if(rb.linearVelocityX > .25)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    private void Update()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            Jump();
        }

        if(jumped)
        {
            if (rb.linearVelocity.y > 0f)
            {
                if (transform.position.y > maxJumpY)
                {
                    maxJumpY = transform.position.y;
                }
            }

            if (rb.linearVelocityY <= 1f)
            {
                rb.linearVelocityY = 0;
                jumped = false;
            }
        }
    }

    public void Jump()
    {
        DoDustParticles(6f);
        AudioSource.PlayOneShot(jumpSound);
        animator.SetTrigger("Jump");
        animator.SetBool("HitGround", false);
        rb.linearVelocityY = jumpForce;
        jumped = true;
        isGrounded = false;

        jumpStartY = transform.position.y;
        maxJumpY = transform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            if (collision.GetContact(0).normal == Vector2.up && !isGrounded)
            {
                isGrounded = true;

                float landingY = transform.position.y;
                int jumpHeight = (int)(maxJumpY - jumpStartY);

                animator.SetBool("HitGround", true);

                DoDustParticles(jumpHeight);
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
                    animator.SetBool("HitGround", false);
                }
            }
        }
    }

    public bool hasJumped()
    {
        return !isGrounded;
    }

    void DoDustParticles(float intensity = 1)
    {
        if (groundDust == null) return;

        int particleCount = Mathf.Clamp(Mathf.RoundToInt(intensity), 0,20);

        var emission = groundDust.emission;
        if (emission.burstCount > 0)
        {
            var burst = emission.GetBurst(0);
            burst.count = new ParticleSystem.MinMaxCurve(particleCount);
            emission.SetBurst(0, burst);
        }

        groundDust.Play();
    }
}
