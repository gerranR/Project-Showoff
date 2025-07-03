using UnityEngine;
using UnityEngine.UI;

public class FirstButton : MonoBehaviour
{
    Button button;
    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.Select();
    }
}
