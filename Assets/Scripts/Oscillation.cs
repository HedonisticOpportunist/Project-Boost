using UnityEngine;

public class Oscillation : MonoBehaviour
{
    [Header("Movement Vectors")]
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;



    void Start()
    {
       startingPosition = transform.position;
       
    }

    void Update()
    {
        MoveObstacle();
    }

    void MoveObstacle()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave + 1f) / 2f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
