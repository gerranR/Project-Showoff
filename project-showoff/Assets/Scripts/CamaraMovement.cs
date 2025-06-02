using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    [SerializeField] float fovMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;
        pos = (player1.transform.position + player2.transform.position) / 2;
        pos.z = transform.position.z;

        Camera.main.fieldOfView = Mathf.Clamp(Vector3.Distance(player1.transform.position, player2.transform.position) * fovMultiplier, minZoom, maxZoom);

        transform.position = pos;
    }
}
