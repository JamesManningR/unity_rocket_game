using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            rcsDirection += 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rcsDirection -= 1f;
        }

        rigidBody.angularVelocity += (Vector3.forward * rcsDirection * rotationInFrame);
    }
}
