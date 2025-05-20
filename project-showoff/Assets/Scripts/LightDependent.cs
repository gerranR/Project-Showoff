using UnityEngine;
using UnityEngine.SceneManagement;

public class LightDependent : MonoBehaviour
{
    [SerializeField] float maxSecondsOutsideLight = 4;
    private float timeOutsideLight = 0f;
    private int lightCount = 0;

    private void Update()
    {
        if (lightCount <= 0)
        {
            timeOutsideLight += Time.deltaTime;
            print(timeOutsideLight + "out lite");
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
            lightCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            lightCount--;
        }
    }
}