using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    [SerializeField] private AudioClip crash;
    [SerializeField] private AudioClip success;

    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem crashParticles;

    private AudioSource _audioSource;

    private bool isTransitioning = false;
    private bool collisionDisable = false;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        DebugMenu();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(isTransitioning || collisionDisable) return;
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                print("This is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void DebugMenu()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; // toggle collision
        }
    }

    private void StartCrashSequence()
    {
        isTransitioning = true;
        
        _audioSource.Stop();
        _audioSource.PlayOneShot(crash);
        
        crashParticles.Play();
        
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadLevel), delay);
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        
        successParticles.Play();
        
        _audioSource.Stop();
        _audioSource.PlayOneShot(success);
        
        GetComponent<Movement>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        Invoke(nameof(LoadNextLevel), delay);
    }

    private void LoadNextLevel()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
