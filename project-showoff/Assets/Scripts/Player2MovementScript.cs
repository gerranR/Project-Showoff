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

    [SerializeField]
    private PlayerMovementScript player1;

    [SerializeField]
    private float attachDistance;

    private bool isAttached;
    private bool isDashing;
    private bool doubleJumped;

    private List<Box> boxes = new List<Box>();
    private GameObject attachedBox;

    [SerializeField] private bool Wow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxes = FindObjectsByType<Box>(FindObjectsSortMode.None).ToList();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wow)
        {
            if (Vector3.Distance(transform.position, player1.transform.position) <= attachDistance)
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
                        transform.parent = player1.transform;
                        transform.position = player1.transform.position + new Vector3(1f, 1f, 0);
                        isAttached = true;
                    }
                }
            }

            if (Input.GetButtonDown("Dash") && !isDashing)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * dashForce * Time.deltaTime;
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
        else
        {
            if (attachedBox != null)
            {
                if (Input.GetButtonDown("Attach"))
                {
                    attachedBox.transform.parent = null;
                    attachedBox = null;
                }
            }
            else
            {
                foreach (Box box in boxes)
                {
                    if (Vector3.Distance(transform.position, box.transform.position) <= attachDistance)
                    {
                        if (Input.GetButtonDown("Attach"))
                        {
                            box.transform.parent = transform;
                            box.transform.position = transform.position + new Vector3(.25f, .25f, 0);
                            attachedBox = box.gameObject;
                            break;
                        }
                    }
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if (!isAvectedByGravity && !isAttached)
        {
            Vector2 force = new Vector2(Input.GetAxis("HorizontalP2") * speed * Time.deltaTime, Input.GetAxis("VerticalP2") * speed * Time.deltaTime);

            if (isDashing)
            {
                print(rb.linearVelocity);
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
            transform.position = player1.transform.position + new Vector3(1f, 1f, 0);
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
}
