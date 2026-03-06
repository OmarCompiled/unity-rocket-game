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
            StartCoroutine(LoadNextLevel(3.0f));
        }
        else
        {
            StartCoroutine(ReloadCurrentLevel(3.0f));
        }
    }

    IEnumerator ReloadCurrentLevel(float waitTime)
    {
        if(!player.enabled) yield break;
        player.enabled = false;
        player.StopAllAudio();
        player.PlayAudio("Crash");
        player.StopAllParticles();
        player.EmmitParticles("Crash", true);
        yield return new WaitForSeconds(waitTime);
        LoadScene(GameManager.Instance.SceneIndex);
    }

    IEnumerator LoadNextLevel(float waitTime)
    {
        if (!player.enabled) yield break;
        player.enabled = false;
        player.StopAllAudio();
        player.PlayAudio("Success");
        yield return new WaitForSeconds(waitTime);
        LoadScene(++GameManager.Instance.SceneIndex);
    }

    void LoadScene(int sceneIndex)
    {
        if (sceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            player.isOnFloor = false;
        }
    }
}
