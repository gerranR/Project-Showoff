using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2MovementScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dashForce;
    [SerializeField] private float LinearDampeningDuringDash;
    [SerializeField] private float dashThrashold;

    [SerializeField]
    private Rigidbody2D rb;

    private bool isAvectedByGravity = false;

    private ContactPoint2D[] lastContectpoint = new ContactPoint2D[5] ;

    private PlayerMovementScript player1;

    [SerializeField]
    private float attachDistance;

    private bool isAttached;
    private bool isDashing;
    private bool doubleJumped;

    private List<Box> boxes = new List<Box>();
    private GameObject attachedBox;

    [SerializeField] float lerpSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxes = FindObjectsByType<Box>(FindObjectsSortMode.None).ToList();

        player1 = FindAnyObjectByType<PlayerMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attach"))
        {
            if (isAttached)
            {
                transform.parent = null;
                isAttached = false;
            }
            else
            {
                if (Vector3.Distance(transform.position, player1.transform.position) <= attachDistance)
                {
                    print("smth arttacher");
                    transform.parent = player1.transform;
                    transform.position = Vector3.Lerp(transform.position, player1.transform.position + new Vector3(1f, 1f, 0), lerpSpeed * Time.deltaTime);
                    isAttached = true;
                }
            }
        }

        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * dashForce;
            rb.linearDamping = LinearDampeningDuringDash;
            isDashing = true;
        }

        if (isAttached && player1.hasJumped())
        {
            if (Input.GetButtonDown("DubbleJump") && !doubleJumped)
            {
                player1.Jump();
                doubleJumped = true;
            }
        }

        if (doubleJumped && !player1.hasJumped())
        {
            doubleJumped = false;
        }
        
    }


    private void FixedUpdate()
    {
        if (!isAvectedByGravity && !isAttached)
        {
            Vector2 force = new Vector2(Input.GetAxis("HorizontalP2") * speed * Time.deltaTime, Input.GetAxis("VerticalP2") * speed * Time.deltaTime);

            if (isDashing)
            {
                if (rb.linearVelocity.magnitude <= dashThrashold)
                {
                    isDashing = false;
                    rb.linearDamping = 0f;
                }
            }
            else
            {
                rb.linearVelocity = force;
            }
        }
        else if (isAttached)
        {
            rb.linearVelocity = Vector2.zero;
            
            transform.position = Vector3.Lerp(transform.position, player1.transform.position + new Vector3(1f, 1f, 0), lerpSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetContacts(lastContectpoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "SplitscreenBarrier")
        {
            if (isAvectedByGravity == false && lastContectpoint[0].point.y < transform.position.y)
            {
                rb.gravityScale = 1f;
                isAvectedByGravity=true;
            }
            else 
            {
                rb.gravityScale = 0f;
                isAvectedByGravity = false;
                rb.AddForce(rb.linearVelocity);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(isAttached && collision.gameObject.layer == LayerMask.NameToLayer("SpiritBarrier"))
        {
            transform.parent = null;
            isAttached = false;
        }
    }
}
