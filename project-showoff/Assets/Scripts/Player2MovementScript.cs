using UnityEngine;

public class Player2MovementScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D rb;

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
        Vector2 force = new Vector2(Input.GetAxis("HorizontalP2") * speed * Time.deltaTime, Input.GetAxis("VerticalP2") * speed * Time.deltaTime);

        rb.linearVelocity = force;

        //Vector2 pos = transform.position;

        //transform.LookAt(pos + force);
    }
}
