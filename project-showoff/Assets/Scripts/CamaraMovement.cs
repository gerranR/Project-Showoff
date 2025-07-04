using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    GameObject player1;
    GameObject player2;

    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    [SerializeField] float fovMultiplier;

    private float normalFOV;
    private Vector3 normalPos;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = FindFirstObjectByType<PlayerMovementScript>().gameObject;
        player2 = FindFirstObjectByType<Player2MovementScript>().gameObject;
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            normalFOV = cam.fieldOfView;
        }

        normalPos = transform.position;


    }

    public void ZoomIn()
    {
        if (player1 != null && player2 != null)
        {
            Vector3 pos;
            pos = (player1.transform.position + player2.transform.position) / 2;
            pos.z = transform.position.z;

            cam.fieldOfView = Mathf.Clamp(Vector3.Distance(player1.transform.position, player2.transform.position) * fovMultiplier, minZoom, maxZoom);

            transform.position = pos;
        }
    }

    public void ZoomOut()
    {
        cam.fieldOfView = normalFOV;
        transform.position = normalPos;
    }
}
