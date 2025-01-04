using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRailSystem : MonoBehaviour
{
    //this script moves the object holding the camera to positions based on the state of the battle (choosing attacks, doing checks, etc)
    public BattleStateManager battleStateManager;

    [System.Serializable]
    public class RailPoint
    {
        public Vector3 position; // Target position for the camera
        public Vector3 rotation; // Target rotation (Euler angles) for the camera
    }

    [Header("Rail Points")]
    public List<RailPoint> railPoints = new List<RailPoint>();

    [Header("Lerp Settings")]
    public float positionLerpSpeed = 5f;
    public float rotationLerpSpeed = 5f;

    private void Update()
    {
        if (battleStateManager == null)
        {
            Debug.LogWarning("BattleStateManager is not assigned.");
            return;
        }

        // Get the current battle state
        int currentState = battleStateManager.battleState;

        if (currentState >= 0 && currentState < railPoints.Count)
        {
            // Get the target position and rotation
            Vector3 targetPosition = railPoints[currentState].position;
            Quaternion targetRotation = Quaternion.Euler(railPoints[currentState].rotation);

            // Smoothly interpolate position and rotation
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionLerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationLerpSpeed);
        }
        else
        {
            Debug.LogWarning($"Invalid battleState: {currentState}. RailPoints count: {railPoints.Count}");
        }
    }
}