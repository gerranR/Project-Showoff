using System.Collections;
using UnityEngine;

public class UnwaxedBrazier : BrazierLighting
{
    [SerializeField] private float burnDuration = 5f;
    private bool isWaxed = false;
    private Coroutine burnCoroutine;

    public override void ActivateLight()
    {
        if (isLit) return;

        base.ActivateLight();

        if (!isWaxed)
        {
            if (burnCoroutine != null) StopCoroutine(burnCoroutine);

            burnCoroutine = StartCoroutine(TemporaryBurn());
        }
    }

    private IEnumerator TemporaryBurn()
    {
        yield return new WaitForSeconds(burnDuration);

        DeactivateLight();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Wax") && !isWaxed)
        {
            isWaxed = true;
            Destroy(other.gameObject);

            if (burnCoroutine != null) StopCoroutine(burnCoroutine);

            ReplaceWithBrazier();
        }
    }

    private void ReplaceWithBrazier()
    {
        var brazier = gameObject.AddComponent<BrazierLighting>();
        brazier.InitializeFrom(this);

        if (IsLit())
            brazier.ActivateLight();

        Destroy(this);
    }
}