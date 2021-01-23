using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start(){
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        ProcessInput();
    }

    private void ProcessInput() {
        if (Input.GetKey(KeyCode.Space)) {
            Thrust();
        } else if (Input.GetKey(KeyCode.A)) {
            print("Rorating left");
        }
        else if (Input.GetKey(KeyCode.D)) {
            print("Rorating right");
        }
    }

    private void Thrust() {
        rigidBody.AddRelativeForce(0,20,0);
    }
}
