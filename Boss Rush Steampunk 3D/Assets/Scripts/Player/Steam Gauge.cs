using UnityEngine;

public class SteamGauge : MonoBehaviour
{
    public Transform spinner; // Reference to the object to rotate
    public Transform indicator; //Reference to the skill check indicator
    public float rotationSpeed = 90f; // Speed of rotation in degrees per second
    public float targetAngleRange = 45f; // Range of degrees within which the spinner stops
    public float targetCriticalRange = 10f; // Range of degrees within which the spinner stops to get a critical strike
    private bool isRotating = true; // Flag to control rotation
    private float randomTargetAngle; // Randomly chosen target angle

    void Start()
    {
        // Choose a random target angle at the start (0 to 360 degrees)
        randomTargetAngle = Random.Range(0f, 360f);
        indicator.Rotate(0, 0, randomTargetAngle);
        Debug.Log($"Target Angle: {randomTargetAngle}°");
    }

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = false; // Stop the rotation

            // Check if the spinner stopped within the target range
            
            float currentAngle = spinner.localEulerAngles.z;
            if (IsWithinTargetRange(currentAngle, randomTargetAngle, targetCriticalRange))
            {
                Debug.Log("Success! Spinner stopped within the CRITICAL target range.");
            }
            else if (IsWithinTargetRange(currentAngle, randomTargetAngle, targetAngleRange))
            {
                Debug.Log("Success! Spinner stopped within the target range.");
            }
            else
            {
                Debug.Log($"Missed! Current angle: {currentAngle}°");
            }
        }

        // Rotate the object if isRotating is true
        if (isRotating && spinner != null)
        {
            spinner.Rotate(0, 0, -rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    private bool IsWithinTargetRange(float currentAngle, float targetAngle, float range)
    {
        // Account for circular angles (0-360 degrees)
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));
        return angleDifference <= range;
    }
}
