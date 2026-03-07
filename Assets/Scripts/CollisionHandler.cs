using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    void Start()
    {
        GameManager.Player.isOnFloor = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            GameManager.Player.isOnFloor = true;
        }
        else if(collision.gameObject.CompareTag("Finish"))
        {
            StartCoroutine(GameManager.LoadNextLevel(3.0f));
        }
        else
        {
            StartCoroutine(GameManager.ReloadCurrentLevel(3.0f));
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            GameManager.Player.isOnFloor = false;
        }
    }
}
