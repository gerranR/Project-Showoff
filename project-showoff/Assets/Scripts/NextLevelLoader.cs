using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NextLevelLoader : MonoBehaviour
{
    [SerializeField] string nextSceneName;

    List<GameObject> playersInTrigger = new();

    private void Update()
    {
        if (!BrazierManager.IsLevelCompleted()) return;
        
        bool humanReady = false;
        bool spiritReady = false;

        foreach (var player in playersInTrigger)
        {
            if (player == null) continue;

            if (player.CompareTag("Human"))
            {
                humanReady = true;
            }
            else if (player.CompareTag("Spirit"))
            {
                spiritReady = true;
            }
        }

        if (humanReady && spiritReady)
        {
            LoadNextScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Human") || other.CompareTag("Spirit"))
        {
            playersInTrigger.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Human") || other.CompareTag("Spirit"))
        {
            playersInTrigger.Remove(other.gameObject);
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
