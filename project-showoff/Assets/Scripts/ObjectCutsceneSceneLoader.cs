using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCutsceneSceneLoader : MonoBehaviour
{
    private DialoguePlayer player;
    [SerializeField]
    private string sceneName;

    private void Start()
    {
        player = GetComponent<DialoguePlayer>();
    }

    private void Update()
    {
        //no events terrible sadness, must do this poop
        if (player.IsDone())
            SceneManager.LoadScene(sceneName);
    }
}
