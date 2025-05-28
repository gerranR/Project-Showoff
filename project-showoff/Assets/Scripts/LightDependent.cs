using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LightDependent : MonoBehaviour
{
    [SerializeField] float maxSecondsOutsideLight = 4;
    private float timeOutsideLight = 0f;
    private int lightCount = 0;

    private List<GameObject> lights = new List<GameObject>();
    [SerializeField] LayerMask rayMask;
    [SerializeField] LayerMask rayMaskNoSconce;
    [SerializeField] Slider slider;

    private void Start()
    {
        if (slider != null) slider.maxValue = maxSecondsOutsideLight;
    }

    private void Update()
    {
        bool canSeeLight = false;
        foreach (GameObject obj in lights)
        {
            RaycastHit2D hit;
            if (obj.transform.tag != "Brazier")
            {
                hit = Physics2D.Raycast(transform.position, (obj.transform.position - transform.position).normalized, Mathf.Infinity, rayMaskNoSconce);
            }
            else
            {
                hit = Physics2D.Raycast(transform.position, (obj.transform.position - transform.position).normalized, Mathf.Infinity, rayMask);
            }
             
            Debug.DrawRay(transform.position, (obj.transform.position - transform.position).normalized, Color.red, 5);
            if (hit.transform != null)
            {
                if (hit.transform.gameObject == obj)
                {
                    canSeeLight = true;
                    break;
                }
            }
        }
        if (lights.Count <= 0 || !canSeeLight)
        {
            timeOutsideLight += Time.deltaTime;
            slider.value = maxSecondsOutsideLight - timeOutsideLight;
            if (timeOutsideLight >= maxSecondsOutsideLight)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            if(slider != null) slider.value = slider.maxValue;
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