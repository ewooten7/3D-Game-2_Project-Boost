using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f; //Parameterizing timed delay of level loadout
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip failure;
//Particle Effects

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    



//Cache
    AudioSource audioSource;
//Booleans: 
     bool isTransitioning = false; //Audio fix
     bool collisionDisable = false;
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }
//Debug Methods:
    void Update() 
    {
        RespondToDebugKeys();
    }
    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) //Press L to load new level
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; //Toggles Collision feature; flips T/F!
        }
    }




    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisionDisable) {return; } //Bool: If game is transitioning, IGNORE the following below

/*Switch Case*/
        switch (other.gameObject.tag) 
        {
            case "Friendly": 
                Debug.Log("This thing is Friendly");
                break; /*Break: A statement has concluded*/
            
            case "Finish": //ALL that happens when you finish a level
                StartSuccessSequence();
                break;

            default: //default: like "ELSE"; message if player TOUCHES anything else NOT tagged above.
                StartCrashSequence();
                break;
        }
    }
/*Methods*/

    void StartSuccessSequence()
    {
        isTransitioning = true; //Boolean for fixing audio SFX; transitioning states
        audioSource.Stop(); //stops thruster SFX after crash/success
        audioSource.PlayOneShot(success);
        successParticles.Play(); //Particle Effect

        GetComponent<Movement>().enabled = false; //stops movement 
        Invoke("LoadNextLevel", levelLoadDelay); //loads next level
    }

    void StartCrashSequence() /*removes control from player after crash & before Level Reset*/
    {
        isTransitioning = true; //Boolean for fixing audio SFX; transitioning states
        audioSource.Stop();
        audioSource.PlayOneShot(failure);
        crashParticles.Play(); //Particle Effect

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
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

    void ReloadLevel() /*Game Reload Method*/
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //variable
        SceneManager.LoadScene(currentSceneIndex); //Index of the Scene in the build Settings to load
    }
}
