using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightDependent : MonoBehaviour
{
    [SerializeField] float maxSecondsOutsideLight = 4;
    private float timeOutsideLight = 0f;
    private int lightCount = 0;

    private List<GameObject> lights = new List<GameObject>();
    [SerializeField] LayerMask rayMask;

    private void Update()
    {
        bool canSeeLight = false;
        foreach (GameObject obj in lights)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (obj.transform.position - transform.position).normalized, Mathf.Infinity, rayMask);

            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Torch" || hit.transform.gameObject.tag == "Brazier" || hit.transform.gameObject.tag == "Human")
                {
                    canSeeLight = true;
                    break;
                }
            }
        }
        if (lights.Count <= 0 || !canSeeLight)
        {
            timeOutsideLight += Time.deltaTime;
            if (timeOutsideLight >= maxSecondsOutsideLight)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {

            timeOutsideLight = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            lights.Add(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            if (lights.Contains(other.transform.parent.gameObject))
            {
                lights.Remove(other.transform.parent.gameObject);
            }
        }
    }
}