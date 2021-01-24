using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 40;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleThrust();
        HandleRotation();
    }

    private void HandleThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(0, mainThrust, 0);
        }
    }

    private void HandleRotation()
    {
        float rcsDirection = 0f;
        float rotationInFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.freezeRotation = true;
            rcsDirection += 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidBody.freezeRotation = true;
            rcsDirection -= 1f;
            print(rcsDirection);
        }

        // transform.Rotate(Vector3.forward * rcsDirection * rotationInFrame);

        rigidBody.freezeRotation = false;
    }
}
