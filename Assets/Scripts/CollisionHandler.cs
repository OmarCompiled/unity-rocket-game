using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private PlayerController player; // Vague naming but works well :)
    // primarily because I'm sure only one of each script exists on the player instance

    void Start()
    {
        player = GetComponent<PlayerController>();
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
            StartCoroutine(LoadNextLevel(2.0f));
        }
        else
        {
            StartCoroutine(ReloadCurrentScene(2.0f));
        }
    }

    IEnumerator ReloadCurrentScene(float waitTime)
    {
        player.enabled = false;
        player.StopAllAudio();
        player.PlayAudio("Crash");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(GameManager.SceneIndex);
    }

    IEnumerator LoadNextLevel(float waitTime)
    {
        player.enabled = false;
        player.StopAllAudio();
        //player.PlayAudio("Finish");
        yield return new WaitForSeconds(waitTime);
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
