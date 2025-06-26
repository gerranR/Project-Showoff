using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadyScreenScript : MonoBehaviour
{

    private bool player1Ready;
    private bool player2Ready;

    [SerializeField] private Image player1Img;
    [SerializeField] private Image player2Img;

    [SerializeField] private string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

                if(player1Img.color == Color.red)
                {
                    player1Img.color = Color.blue;
                }
                else
                {
                    player1Img.color = Color.red;
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

                if (player2Img.color == Color.red)
                {
                    player2Img.color = Color.blue;
                }
                else
                {
                    player2Img.color = Color.red;
                }
                break;
            }
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            player1Ready = !player1Ready;

            if (player1Img.color == Color.red)
            {
                player1Img.color = Color.blue;
            }
            else
            {
                player1Img.color = Color.red;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player2Ready = !player2Ready;

            if (player2Img.color == Color.red)
            {
                player2Img.color = Color.blue;
            }
            else
            {
                player2Img.color = Color.red;
            }
        }


        if (player1Ready && player2Ready)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
