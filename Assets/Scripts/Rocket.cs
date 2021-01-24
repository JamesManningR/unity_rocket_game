using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] float rcsThrust = 50f;
    [SerializeField] float mainThrust = 10000;
    [SerializeField] Vector3 centerOfMass = new Vector3(0, -.2f, 0);



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.centerOfMass = centerOfMass;
        HandleThrust();
        HandleRotation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                SceneManager.LoadScene(1);
                break;

            default:
                SceneManager.LoadScene(0);
                break;
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
}
