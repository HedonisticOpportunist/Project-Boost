using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("Delay")]
    [SerializeField] float levelLoadDelay = 2f;

    [Header("Audio")]
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    AudioSource _audioSource;

    [Header("Particles")]
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    // STATES
    bool _isTransitioning = false;
    bool _collisionDisabled = false;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (_isTransitioning || _collisionDisabled)
        {
            return; 
        }

       
       HandleCollisions(collision);
        
    }

    void StartSuccessSequence()
    {
        _isTransitioning = true;
        _audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    void StartCrashSequence()
    {
        _isTransitioning = true;
        _audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadScene), levelLoadDelay);
    }

    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void HandleCollisions(Collision collision)
    {
        switch (collision.gameObject.tag)
        {

            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _collisionDisabled = !_collisionDisabled; 
        }

    }
}
