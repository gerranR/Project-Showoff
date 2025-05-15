using UnityEngine;

public class SconceLighting : MonoBehaviour
{
    public GameObject lightArea;
    private bool isLit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLit) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Torch") || other.gameObject.layer == LayerMask.NameToLayer("HeldTorch"))
        {
            ActivateLight();
        }
    }

    public void ActivateLight()
    {
        if (isLit) return;

        lightArea.SetActive(true);
        isLit = true;
    }
}
