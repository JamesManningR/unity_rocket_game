using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 50f;
    [SerializeField] float mainThrust = 10000;
    [SerializeField] Vector3 centerOfMass = new Vector3(0, -.2f, 0);

    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip reverseEngineSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem reverseEngineParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.centerOfMass = centerOfMass;

        switch (state)
        {
            case State.Alive: // Check for inputs
                HandleThrust();
                HandleRotation();
                HandleRestart();
                break;

            case State.Dying: // Spin out of control
                rigidBody.AddRelativeForce(0, mainThrust, 0);
                float rotationInFrame = rcsThrust * Time.deltaTime;
                rigidBody.angularVelocity += (Vector3.forward);
                ReloadScene();
                break;

            case State.Transcending:
                LoadNextScene();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore any collisions if player is dead or transcending
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        { // Set state based oncollision type
            case "Friendly":
                break;

            case "Finish":
                state = State.Transcending;
                break;

            default:
                state = State.Dying;
                break;
        }
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int levelCount = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % levelCount;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void HandleThrust()
    {
        float thrustDirection = 0f; // Start thrust direction at 0

        if (Input.GetKey(KeyCode.Space))
        {
            mainEngineParticles.Play();
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngineSound, 0.7f);
            }
            thrustDirection++; // Add to thrust in forward direction
        }
        else
        {
            mainEngineParticles.Stop();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            reverseEngineParticles.Play();
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(reverseEngineSound, 0.7f);
            }
            thrustDirection -= 0.5f; // Add to thrust in reverse direction
        }
        else
        {
            reverseEngineParticles.Stop();
        }

        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.Space))
        {
            audioSource.Stop();
        }

        rigidBody.AddRelativeForce(0, thrustDirection * mainThrust, 0); // Apply final thrust direction
    }

    private void HandleRotation()
    {
        float rcsDirection = 0f;
        float rotationInFrame = rcsThrust * Time.deltaTime; // Keep consistant RCS thrust regardless of frame rate

        if (Input.GetKey(KeyCode.A))
        {
            rcsDirection++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rcsDirection--;
        }

        rigidBody.angularVelocity += (Vector3.forward * rcsDirection * rotationInFrame);
    }

    private void HandleRestart()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
