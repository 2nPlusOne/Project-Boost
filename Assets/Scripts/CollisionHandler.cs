using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // PARAMETERS
    [SerializeField] float levelLoadDelay = 2.0f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField, Range(0,1)] float explosionVolume = 0.25f;
    [SerializeField] AudioClip successSound;
    [SerializeField, Range(0,1)] float successVolume = 0.5f;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem successParticles;

    // CACHE REFERENCES
    AudioSource audioSource;
    PlayerMovement pMovement;

    // MEMBER VARIABLES
    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pMovement = GetComponent<PlayerMovement>();
        pMovement.enabled = true;
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
            DebugLoadNextLevel();

        if (Input.GetKeyDown(KeyCode.C))
            collisionDisabled = !collisionDisabled;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled) { return; }
        switch (collision.gameObject.tag)
        {
            case "Respawn":
                Debug.Log("Collided with launchpad.");
                break;
            case "Finish":
                StartNextLevelSequence();
                break;
            case "Friendly":
                Debug.Log("Collided with something friendly.");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartNextLevelSequence()
    {
        isTransitioning = true;
        pMovement.enabled = false;
        audioSource.Stop();
        audioSource.volume = successVolume;
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        StartCoroutine(LoadNextLevelAfterDelay());
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        pMovement.enabled = false;
        audioSource.Stop();
        audioSource.volume = explosionVolume;
        audioSource.PlayOneShot(explosionSound);
        explosionParticles.Play();
        StartCoroutine(ReloadLevelAfterDelay());
    }

    IEnumerator ReloadLevelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);
    }

    void DebugLoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);
    }

}
