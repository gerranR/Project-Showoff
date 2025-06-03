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

    [SerializeField] bool useRaycast = true;

    private ContactFilter2D filter;
    private Collider2D[] results = new Collider2D[10];

    private void Start()
    {
        if (slider != null) slider.maxValue = maxSecondsOutsideLight;

        filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Light"));
        filter.useTriggers = true;
    }

    private void FixedUpdate()
    {
        CircleCollider2D triggerZone = GetComponent<CircleCollider2D>();
        Physics2D.SyncTransforms();
        int count = triggerZone.Overlap(filter, results);

        lights.Clear();


        for (int i = 0; i < count; i++)
        {
            var light = results[i].transform.parent?.gameObject;
            if (light != null && !lights.Contains(light))
            {
                lights.Add(light);
            }
        }

        Debug.Log("Lights in zone: " + lights.Count);

        bool canSeeLight = false;
        if (useRaycast)
        {
            foreach (GameObject obj in lights)
            {
                RaycastHit2D hit;
                Vector2 direction = (obj.transform.position - transform.position).normalized;
                float distance = Vector2.Distance(transform.position, obj.transform.position);
                if (obj.transform.tag != "Brazier")
                {
                    Physics2D.SyncTransforms();
                    hit = Physics2D.Raycast(transform.position, direction, distance, rayMaskNoSconce);
                }
                else
                {
                    hit = Physics2D.Raycast(transform.position, direction, distance, rayMask);
                }


                Debug.DrawRay(transform.position, direction * distance, Color.red, 1f);
                if (hit.transform != null)
                {
                    print(hit.collider.name);
                    if (hit.transform.gameObject == obj)
                    {
                        canSeeLight = true;
                        break;
                    }
                }
            }
        }
        else if(lights.Count > 0)
        {
            canSeeLight = true;
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
/*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            lights.Add(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            GameObject lightObj = collision.transform.parent.gameObject;
            if (!lights.Contains(lightObj))
            {
                lights.Add(lightObj);
            }
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
    }*/
}