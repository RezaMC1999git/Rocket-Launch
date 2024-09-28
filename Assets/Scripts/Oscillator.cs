using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    [SerializeField] [Range(0, 1)] float movementFactor;
    Vector3 startingPos;
    private void Start()
    {
        startingPos = transform.position;
    }
    private void Update()
    {
        if(period <= Mathf.Epsilon) { return; }
        float Cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(Cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offSet = (movementVector * movementFactor);
        transform.position = startingPos + offSet;
    }
}
