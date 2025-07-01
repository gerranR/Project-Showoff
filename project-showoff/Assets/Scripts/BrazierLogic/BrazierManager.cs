using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BrazierManager : MonoBehaviour
{
    [SerializeField] GameObject endLightArea;
    [SerializeField] GameObject dreamSpritesObject; // 1 game object with all sprites to trigger childrened to it
    [SerializeField] GameObject nightmareSpritesObject;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winSound;

    static List<BrazierLighting> braziers = new List<BrazierLighting>();
    static bool levelCompleted;

    private static BrazierManager _instance;
    public static BrazierManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Dreamanager is null");
            }
            return _instance;

        }
    }

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        levelCompleted = false;
        if (endLightArea) endLightArea.SetActive(false);
        if (dreamSpritesObject) dreamSpritesObject.SetActive(false);
        if (nightmareSpritesObject) nightmareSpritesObject.SetActive(true);
        braziers.Clear();
    }

    public void RegisterBrazier(BrazierLighting brazier)
    {
        if (!braziers.Contains(brazier))
            braziers.Add(brazier);
    }

    public void CheckBraziers()
    {
        foreach (var brazier in braziers)
        {
            if (brazier == null || !brazier.IsLit())
            {
                print("false");
                return;
            }
        }


        if (instance != null && AllBraziersLighted())
        {
            levelCompleted = true;
            audioSource.PlayOneShot(winSound);
            if (instance.endLightArea) instance.endLightArea.SetActive(true);
            if (instance.dreamSpritesObject) instance.dreamSpritesObject.SetActive(true);
            if (instance.nightmareSpritesObject) instance.nightmareSpritesObject.SetActive(true);

        }
    }
    public static bool IsLevelCompleted() => levelCompleted;

    private static bool AllBraziersLighted()
    {
        foreach(BrazierLighting brazier in braziers)
        {
            print(brazier.IsLit());
            if(!brazier.IsLit())
            {
                return false;
            }
        }
        return true;
    }
}