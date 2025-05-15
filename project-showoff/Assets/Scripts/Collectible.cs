using UnityEngine;

public class DestroyOnHumanTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask humanLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            Destroy(gameObject);
        }
    }
}
