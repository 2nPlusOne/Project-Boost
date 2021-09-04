using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Respawn":
                Debug.Log("Collided with launchpad.");
                break;
            case "Finish":
                Debug.Log("Collided with landing pad.");
                break;
            case "Fuel":
                Debug.Log("Collided with fuel canister.");
                break;
            case "Friendly":
                Debug.Log("Collided with something friendly.");
                break;
            default:
                Debug.Log("Collided with an obstacle.");
                StartCoroutine(ReloadLevelAfterDelay());
                break;
        }
    }
  
    IEnumerator ReloadLevelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
