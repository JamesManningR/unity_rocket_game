using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] float rcsThrust = 50f;
    [SerializeField] float mainThrust = 10000;
    [SerializeField] Vector3 centerOfMass = new Vector3(0, -.2f, 0);

    AudioSource audioSource;

    public AudioClip[] deathClips;
    public AudioClip[] winClips;
    public AudioClip[] completeClips;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        deathClips = Resources.LoadAll<AudioClip>("Sounds/Death");
        winClips = Resources.LoadAll<AudioClip>("Sounds/Win");
        completeClips = Resources.LoadAll<AudioClip>("Sounds/Complete");

        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    void Update()
    {

        if (state != State.Alive) { return; };

        HandleThrust();
        HandleRotation();
        HandleRestart();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; };

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                PlayWinSound();
                state = State.Transcending;
                Invoke("LoadNextScene", 2f);
                break;

            default:
                PlayDeathSound();
                rigidBody.freezeRotation = false;
                rigidBody.mass = 10000000000000000;
                state = State.Dying;
                Invoke("ReloadScene", 2f);
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
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings == nextSceneIndex)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void HandleThrust()
    {
        float thrustDirection = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            thrustDirection++;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            thrustDirection -= 0.5f;
        }

        rigidBody.AddRelativeForce(0, thrustDirection * mainThrust, 0);
    }

    private void HandleRotation()
    {
        float rcsDirection = 0f;
        float rotationInFrame = rcsThrust * Time.deltaTime;

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

    private void PlayDeathSound()
    {
        AudioClip selectedSound = deathClips[Random.Range(0, deathClips.Length)];
        audioSource.clip = selectedSound;
        audioSource.Play();
    }
    private void PlayWinSound()
    {
        AudioClip selectedSound = winClips[Random.Range(0, winClips.Length)];
        audioSource.clip = selectedSound;
        audioSource.Play();
    }
    private void PlayCompleteSound()
    {
        AudioClip selectedSound = completeClips[Random.Range(0, completeClips.Length)];
        audioSource.clip = selectedSound;
        audioSource.Play();
    }
}
