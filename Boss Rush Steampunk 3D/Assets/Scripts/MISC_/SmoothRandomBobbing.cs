using UnityEngine;

public class SmoothRandomBobbingAndRotation : MonoBehaviour
{
    [Header("Movement Settings")]
    public float xAmplitude = 0.2f; // Maximum horizontal distance
    public float yAmplitude = 0.2f; // Maximum vertical distance
    public float xSpeed = 0.5f; // Speed of horizontal movement
    public float ySpeed = 0.5f; // Speed of vertical movement

    [Header("Rotation Settings")]
    public float xRotationAmplitude = 15f; // Maximum rotation on the X-axis
    public float yRotationAmplitude = 15f; // Maximum rotation on the Y-axis
    public float rotationSpeed = 1.0f; // Speed of rotation

    [HideInInspector] public Vector3 initialLocalPosition;
    [HideInInspector] public Quaternion initialLocalRotation;
    private float xNoiseOffset;
    private float yNoiseOffset;
    private float rotationNoiseOffset;

    public bool active = true;

    void Start()
    {
        // Store the initial local position and rotation of the object
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;

        // Generate random offsets for the Perlin noise
        xNoiseOffset = Random.Range(0f, 100f);
        yNoiseOffset = Random.Range(0f, 100f);
        rotationNoiseOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        //  If active, do the wobbling and rotating
        if (active)
        {
            // Calculate offsets for position using Perlin noise
            float x = Mathf.PerlinNoise(Time.time * xSpeed + xNoiseOffset, 0f) - 0.5f; // Centered around 0
            float y = Mathf.PerlinNoise(Time.time * ySpeed + yNoiseOffset, 0f) - 0.5f; // Centered around 0
            Vector3 positionOffset = new Vector3(x * xAmplitude * 2f, y * yAmplitude * 2f, 0f);

            // Update local position
            transform.localPosition = initialLocalPosition + positionOffset;

            // Calculate offsets for rotation using Perlin noise
            float xRotation = (Mathf.PerlinNoise(Time.time * rotationSpeed + rotationNoiseOffset, 0f) - 0.5f) * 2f * xRotationAmplitude;
            float yRotation = (Mathf.PerlinNoise(Time.time * rotationSpeed + rotationNoiseOffset + 50f, 0f) - 0.5f) * 2f * yRotationAmplitude; // Offset ensures different pattern

            // Update local rotation
            Quaternion rotationOffset = Quaternion.Euler(xRotation, yRotation, 0f);
            transform.localRotation = initialLocalRotation * rotationOffset;
        }
        else    //  Otherwise, just return to the initial position and rotation
        {
            transform.localPosition = initialLocalPosition;
            transform.localRotation = initialLocalRotation;
        }
    }
}