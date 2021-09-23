using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    const float TAU = Mathf.PI * 2; // 6.283...

    Vector3 startingPosition;

    [SerializeField] Vector3 movementVector;
    [SerializeField, Range(0,1)] float movementFactor;
    [SerializeField] float period = 2f;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // Continually growing over time

        float rawSinWave = Mathf.Sin(cycles * TAU); // Values from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; // Recalculate rawSineWave to go from 0 to 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
