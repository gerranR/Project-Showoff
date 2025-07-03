using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadyScreenScript : MonoBehaviour
{

    private bool player1Ready;
    private bool player2Ready;

    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private TextMeshProUGUI player2Text;

    [SerializeField] private string readyTextPlayer1;
    [SerializeField] private string notReadyTextPlayer1;
    [SerializeField] private string readyTextPlayer2;
    [SerializeField] private string notReadyTextPlayer2;

    [SerializeField] private string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1Text.text = notReadyTextPlayer1;
        player2Text.text = notReadyTextPlayer2;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 1 button " + i))
            {
                Debug.Log("Joystick 1 input detected");
                player1Ready = !player1Ready;

                if(player1Text.text == notReadyTextPlayer1)
                {
                    player1Text.text = readyTextPlayer1;
                }
                else
                {
                    player1Text.text = notReadyTextPlayer1;
                }

                break;
            }
        }
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 2 button " + i))
            {
                Debug.Log("Joystick 2 input detected");
                player2Ready = !player2Ready;

                if (player2Text.text == notReadyTextPlayer2)
                {
                    player2Text.text = readyTextPlayer2;
                }
                else
                {
                    player2Text.text = notReadyTextPlayer2;
                }
                break;
            }
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            player1Ready = !player1Ready;

            if (player1Text.text == notReadyTextPlayer1)
            {
                player1Text.text = readyTextPlayer1;
            }
            else
            {
                player1Text.text = notReadyTextPlayer1;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player2Ready = !player2Ready;

            if (player2Text.text == notReadyTextPlayer2)
            {
                player2Text.text = readyTextPlayer2;
            }
            else
            {
                player2Text.text = notReadyTextPlayer2;
            }
        }


        if (player1Ready && player2Ready)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
