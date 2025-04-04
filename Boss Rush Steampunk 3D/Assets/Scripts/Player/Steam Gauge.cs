//using UnityEditor.IMGUI.Controls;
//using UnityEditor.TerrainTools;
using UnityEngine;

public class SteamGauge : MonoBehaviour
{
    public Transform spinner; // Reference to the object to rotate
    public Transform indicator; //Reference to the skill check indicator
    public float rotationSpeed = 90f; // Speed of rotation in degrees per second
    public float targetAngleRange = 45f; // Range of degrees within which the spinner stops
    public float targetCriticalRange = 10f; // Range of degrees within which the spinner stops to get a critical strike
    public float tolerance = 3f; // How much the stick can be bumped by if it's close enough to a range
    private bool isRotating = false; // Flag to control rotation
    public int result = -1; // -1 while undefined, 0 when missed, 1 when hit target, 2 when hit crit
    private float randomTargetAngle; // Randomly chosen target angle
    public GameObject smallDamage; // A reference to the small damage prefab (which will be instantiated when you click)
    public Transform canvas; // A reference to the canvas to create the small damage object on
    public Bounds bounds; // The bounding box that the small damage objects can be create in

	public TextCutscene cutscene;
	public bool tutorial;

    void Start()
    {
        // Choose a random target angle at the start (0 to 360 degrees)
        Randomize();
		cutscene = TextCutscene.me;
    }

    void Update()
    {
        // Check if the left mouse button is clicked or space bar
        if (isRotating && (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")))
        {
			if(tutorial){
				cutscene.next();
			}
            isRotating = false; // Stop the rotation

            // Check if the spinner stopped within the target range
            
            float currentAngle = spinner.localEulerAngles.z;
            if (IsWithinTargetRange(currentAngle, randomTargetAngle, targetCriticalRange, out float difference1, tolerance))
            {
                Debug.Log("Success! Spinner stopped within the CRITICAL target range.");
                result = 2;
                spinner.Rotate(0, 0, difference1);
            }
            else if (IsWithinTargetRange(currentAngle, randomTargetAngle, targetAngleRange, out float difference2, tolerance))
            {
                Debug.Log("Success! Spinner stopped within the target range.");
                result = 1;
                spinner.Rotate(0, 0, difference2);
            }
            else
            {
                Debug.Log($"Missed! Current angle: {currentAngle}�");
                result = 0;
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
        result = -1;
		//set the battle state to the one with the wheel
    }

    public void Randomize()
    {
        // Choose a random target angle at the start (0 to 360 degrees)
        randomTargetAngle = Random.Range(0f, 360f);
        indicator.Rotate(0, 0, randomTargetAngle);
        Debug.Log($"Target Angle: {randomTargetAngle}�");
    }

    private bool IsWithinTargetRange(float currentAngle, float targetAngle, float range)
    {
        // Account for circular angles (0-360 degrees)
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));
        return angleDifference <= range;
    }

    private bool IsWithinTargetRange(float currentAngle, float targetAngle, float range, out float difference, float tolerance)
    {
        // Account for circular angles (0-360 degrees)
        difference = Mathf.DeltaAngle(currentAngle, targetAngle);
        float angleDifference = Mathf.Abs(difference);
        difference = angleDifference <= range ? 0 : Mathf.Sin(difference) * (angleDifference - range);
        return angleDifference <= range + tolerance;
    }
}
