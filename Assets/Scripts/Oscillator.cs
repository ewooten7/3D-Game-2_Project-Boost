using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0,1)] float movementFactor; //add Range Movement slider attribute; sin also does this below
    [SerializeField] float period = 2f;

    void Start()
    {
        startingPosition = transform.position;
        
    }

    void Update()
    { 
        if (period <= Mathf.Epsilon) {return; }    //prevents "Not a Number (NaN) error; Period=0 makes obstacle stop moving.
        float cycles = Time.time / period;  //continually growing over time

        const float tau = Mathf.PI * 2;     //tau = pi x 2 (constant values of 6.28)
        float rawSineWave = Mathf.Sin(cycles * tau); //going from -1 to 1

        movementFactor = (rawSineWave + 1f) / 2f;   //recalculated to go from 0 to 1 (better than Range Slider above)


        Vector3 offset = movementVector * movementFactor;   //Offset factor
        transform.position = startingPosition + offset;     //oscillates an object by starting position in numbered intervals from offset calc.
    }
}
