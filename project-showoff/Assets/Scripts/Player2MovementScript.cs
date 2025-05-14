using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player2MovementScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D rb;

    private bool isAvectedByGravity = false;

    private ContactPoint2D[] lastContectpoint = new ContactPoint2D[5];


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!isAvectedByGravity)
        {
            Vector2 force = new Vector2(Input.GetAxis("HorizontalP2") * speed * Time.deltaTime, Input.GetAxis("VerticalP2") * speed * Time.deltaTime);

            rb.linearVelocity = force;
        }
        //Vector2 pos = transform.position;

        //transform.LookAt(pos + force);
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
