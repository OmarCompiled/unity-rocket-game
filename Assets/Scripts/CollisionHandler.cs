using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private MovementHandler player; // Vague naming but works well :)
    // primarily because I'm sure only one of each script exists on the player instance

    void Start()
    {
        player = GetComponent<MovementHandler>();
        player.isOnFloor = true;
    }

    void OnCollisionEnter(Collision collision)
    {


        if(collision.gameObject.CompareTag("Floor"))
        {
            player.isOnFloor = true;
        }
        else if(collision.gameObject.CompareTag("Finish"))
        {
            LoadNextLevel();
        }
        else
        {
            ReloadCurrentScene();
        }
    }

    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(GameManager.SceneIndex);
    }

    void LoadNextLevel()
    {
        if(++GameManager.SceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            GameManager.SceneIndex = 0;
        }
        SceneManager.LoadScene(GameManager.SceneIndex);
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            player.isOnFloor = false;
        }
    }
}
