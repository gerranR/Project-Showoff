using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    public TorchObject torch;
    public AudioMixer masterMixer;

    public Toggle toggle;
    public Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        torch = FindFirstObjectByType<TorchObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InvertThrow()
    {
        if (torch != null)
        {
            if (toggle.isOn)
            {
                torch.invertThrow = -1;
                GameManager.instance.inverted = -1;
            }
            else
            {
                torch.invertThrow = 1;
                GameManager.instance.inverted = 1;
            }
        }
    }

    public void ChangeVolume(float volumeLvl)
    {
        float volume = Mathf.Log10(Mathf.Max(volumeLvl, 0.0001f)) * 20;
        masterMixer.SetFloat("Volume", volume);
        GameManager.instance.volume = volume;
    }

    public void MainMenu(string mainMenuSceneName)
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestardlVl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
