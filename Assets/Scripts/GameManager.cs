using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get {return instance;}
    }

    private static int sceneIndex;
    public static int SceneIndex
    {
        get{return sceneIndex;}
        set{sceneIndex = value;}
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance != null || instance != this)
        {
            Destroy(this); // Destroy does NOT return.
            return;
        }
        instance = this;
        SceneIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
