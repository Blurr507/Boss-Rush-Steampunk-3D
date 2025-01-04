using UnityEditor.IMGUI.Controls;
using UnityEditor.TerrainTools;
using UnityEngine;

public class SteamGauge : MonoBehaviour
{
    public Transform spinner; // Reference to the object to rotate
    public Transform indicator; //Reference to the skill check indicator
    public float rotationSpeed = 90f; // Speed of rotation in degrees per second
    public float targetAngleRange = 45f; // Range of degrees within which the spinner stops
    public int targetAngleDamage = 20; // The damage done if the spinner stops in the target angle
    public float targetCriticalRange = 10f; // Range of degrees within which the spinner stops to get a critical strike
    public int targetCriticalDamage = 50; // The damage done if the spinner stops in the target angle
    public int targetFailDamage = 5; // The damage done if the spinner stops outside of the target angle
    public bool isRotating = true; // Flag to control rotation
    private float randomTargetAngle; // Randomly chosen target angle
    public GameObject smallDamage; // A reference to the small damage prefab (which will be instantiated when you click)
    public Transform canvas; // A reference to the canvas to create the small damage object on
    public Bounds bounds; // The bounding box that the small damage objects can be create in

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
        if (isRotating && Input.GetMouseButtonDown(0))
        {
            isRotating = false; // Stop the rotation

            // Check if the spinner stopped within the target range
            
            float currentAngle = spinner.localEulerAngles.z;
            if (IsWithinTargetRange(currentAngle, randomTargetAngle, targetCriticalRange))
            {
                Debug.Log("Success! Spinner stopped within the CRITICAL target range.");
                SmallDamage damage = Instantiate(smallDamage, RandomPosInBounds(), Quaternion.LookRotation(transform.position - Camera.main.transform.position), canvas).GetComponent<SmallDamage>();
                damage.damage = targetCriticalDamage;
            }
            else if (IsWithinTargetRange(currentAngle, randomTargetAngle, targetAngleRange))
            {
                Debug.Log("Success! Spinner stopped within the target range.");
                SmallDamage damage = Instantiate(smallDamage, RandomPosInBounds(), Quaternion.LookRotation(transform.position - Camera.main.transform.position), canvas).GetComponent<SmallDamage>();
                damage.damage = targetAngleDamage;
            }
            else
            {
                Debug.Log($"Missed! Current angle: {currentAngle}°");
                SmallDamage damage = Instantiate(smallDamage, RandomPosInBounds(), Quaternion.LookRotation(transform.position - Camera.main.transform.position), canvas).GetComponent<SmallDamage>();
                damage.damage = targetFailDamage;
            }
        }

        // Rotate the object if isRotating is true
        if (isRotating && spinner != null)
        {
            spinner.Rotate(0, 0, -rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void Spin()
    {
        isRotating = true;
    }

    private bool IsWithinTargetRange(float currentAngle, float targetAngle, float range)
    {
        // Account for circular angles (0-360 degrees)
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));
        return angleDifference <= range;
    }

    private Vector3 RandomPosInBounds()
    {
        return new Vector3(Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x),
                           Random.Range(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y),
                           Random.Range(bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z));
    }
}
