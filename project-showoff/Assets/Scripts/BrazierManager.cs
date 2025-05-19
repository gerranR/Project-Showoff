using UnityEngine;
using System.Collections.Generic;

public class BrazierManager : MonoBehaviour
{
    [SerializeField] GameObject endLightArea;
    [SerializeField] GameObject dreamSpritesObject; // 1 game object with all sprites to trigger childrened to it
    [SerializeField] GameObject nightmareSpritesObject;

    static List<BrazierLighting> braziers = new();
    static BrazierManager instance;
    static bool levelCompleted;

    private void Awake()
    {
        instance = this;
        levelCompleted = false;
        if (endLightArea) endLightArea.SetActive(false);
        if (dreamSpritesObject) dreamSpritesObject.SetActive(false);
        if (nightmareSpritesObject) nightmareSpritesObject.SetActive(true);
    }

    public static void RegisterBrazier(BrazierLighting brazier)
    {
        if (!braziers.Contains(brazier))
            braziers.Add(brazier);
    }

    public static void CheckBraziers()
    {
        foreach (var brazier in braziers)
        {
            if (brazier == null || !brazier.IsLit())
                return;
        }

        if (instance != null)
        {
            levelCompleted = true;
            if (instance.endLightArea) instance.endLightArea.SetActive(true);
            if (instance.dreamSpritesObject) instance.dreamSpritesObject.SetActive(true);
            if (instance.nightmareSpritesObject) instance.nightmareSpritesObject.SetActive(true);

        }
    }
    public static bool IsLevelCompleted() => levelCompleted;
}