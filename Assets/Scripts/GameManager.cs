using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public int SceneIndex {get; set;}
   
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy does NOT return.
            return;
        }
        Instance = this;
        Instance.SceneIndex = 0;
        DontDestroyOnLoad(gameObject);
    }
}
