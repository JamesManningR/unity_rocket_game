using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] float rcsThrust = 50f;
    [SerializeField] float mainThrust = 10000;
    [SerializeField] Vector3 centerOfMass = new Vector3(0, -.2f, 0);

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.centerOfMass = centerOfMass;
        switch (state)
        {
            case State.Alive:
                HandleThrust();
                HandleRotation();
                HandleRestart();
                break;

            case State.Dying:
                rigidBody.AddRelativeForce(0, mainThrust, 0);
                float rotationInFrame = rcsThrust * Time.deltaTime;
                rigidBody.angularVelocity += (Vector3.forward);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                state = State.Transcending;
                LoadNextScene();
                break;

            default:
                state = State.Dying;
                ReloadScene();
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
}
