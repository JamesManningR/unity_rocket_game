using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PositionOscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(2f, 2f, 2f);
    [SerializeField] float period = 2f;

    [Range(0, 1)] float movementFactor;
    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        // Make sure period is more than 0
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + .5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
