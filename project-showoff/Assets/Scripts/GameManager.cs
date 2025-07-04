using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int collectebles;

    public float volume;
    public float inverted;
    private GameObject optionsObj;

    private static GameManager _instance;
    public static GameManager instance
    {
        get 
        {
            if( _instance == null )
            {
                Debug.LogError("Gamemanager is null");
            }    
            return _instance; 
        
        }
    }

    public void applyOptions(Scene scene, LoadSceneMode loadSceneMode)
    {
        Options options = FindAnyObjectByType<Options>();
        if (options != null)
        {
            optionsObj = options.gameObject;
            options.masterMixer.SetFloat("Volume", volume);
            options.masterMixer.GetFloat("Volume", out float a);
            print(a);
            options.slider.value = Mathf.Pow(10f, volume / 20f);
            print(options.slider.value);
            options.torch = FindFirstObjectByType<TorchObject>();
            if (options.torch != null)
            {
                if (inverted == -1)
                {
                    options.torch.invertThrow = -1;
                    options.toggle.isOn = true;
                }
                else
                {
                    options.torch.invertThrow = 1;
                    options.toggle.isOn = false;
                }
                print(options.toggle.isOn);
            }
            options.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("OpenMenu"))
        {
            if(optionsObj != null)
                optionsObj.SetActive(true);
        }
    }

    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += applyOptions;
    }
    public void addCollectebles()
    {
        collectebles++;
        print("new amount of collectables: " + collectebles);
    }

    public int getCollectebles()
    {
        return collectebles;
    }
}
