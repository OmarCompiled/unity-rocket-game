using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static PlayerController Player {get; set;} // Vague naming but works well :)
    public static int SceneIndex {get; set;}
    // primarily because I'm sure only one of each script exists on the Player instance
   
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy does NOT return.
            return;
        }
        Instance = this;
        SceneIndex = 0;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        ProcessDebugKeys();
    }

    void ProcessDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            SceneIndex = (SceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(SceneIndex);
        } 
        else if(Input.GetKeyDown(KeyCode.J))
        {
            SceneIndex = (SceneIndex == 0) ? SceneManager.sceneCountInBuildSettings - 1 : --SceneIndex;
            SceneManager.LoadScene(SceneIndex);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); // This is ignored in the editor :/
        }
    }

    public static
    IEnumerator ReloadCurrentLevel(float waitTime)
    {
        if(!Player.enabled) yield break;
        Player.enabled = false;
        Player.StopAllAudio();
        Player.PlayAudio("Crash");
        Player.StopAllParticles();
        Player.EmmitParticles("Crash", true);
        yield return new WaitForSeconds(waitTime);
        LoadScene(GameManager.SceneIndex);
    }

    public static
    IEnumerator LoadNextLevel(float waitTime)
    {
        if (!Player.enabled) yield break;
        Player.enabled = false;
        Player.StopAllAudio();
        Player.PlayAudio("Success");
        yield return new WaitForSeconds(waitTime);
        LoadScene(++GameManager.SceneIndex);
    }

    public static
    void LoadScene(int sceneIndex)
    {
        if (sceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
        }
        SceneManager.LoadScene(sceneIndex);
    }
}
