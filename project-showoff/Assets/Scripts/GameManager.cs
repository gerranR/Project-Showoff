using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int collectebles;



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

    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
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
