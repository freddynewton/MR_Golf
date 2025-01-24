using UnityEngine;

/// <summary>
/// Handles the rotation of the golf track container based on XR input.
/// </summary>
public class GolfSpawnHandler : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Maximum rotation speed.")]
    [SerializeField] private float maxRotationSpeed = 1f;

    [Tooltip("Time to reach maximum rotation speed.")]
    [SerializeField] private float accelerationTime = 1f;

    [Header("References")]
    [Tooltip("The container for the golf track.")]
    [SerializeField] private Transform golfTrackContainer;

    private SpawnRotationIndicatorController spawnRotationIndicatorController;
    private bool isTurning;
    private float currentRotationSpeed;

    /// <summary>
    /// Initializes the component.
    /// </summary>
    private void Awake()
    {
        spawnRotationIndicatorController ??= GetComponentInChildren<SpawnRotationIndicatorController>();
    }

    /// <summary>
    /// Updates the rotation based on input.
    /// </summary>
    private void Update()
    {
        Vector2 turnInput = XrInputManager.Instance.RightTurnInputValue;

        if (turnInput.x != 0)
        {
            if (!isTurning)
            {
                isTurning = true;
                currentRotationSpeed = 0f;
            }

            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, maxRotationSpeed, maxRotationSpeed / accelerationTime * Time.deltaTime);
            float rotationAmount = turnInput.x * currentRotationSpeed * Time.deltaTime;
            golfTrackContainer.Rotate(Vector3.up, rotationAmount);

            // Set highlight based on the direction of the rotation
            if (turnInput.x > 0)
            {
                spawnRotationIndicatorController.SetHighlight(false); // Highlight right indicator
            }
            else if (turnInput.x < 0)
            {
                spawnRotationIndicatorController.SetHighlight(true); // Highlight left indicator
            }
        }
        else
        {
            if (!isTurning)
            {
                return;
            }

            isTurning = false;
            currentRotationSpeed = 0f;

            // Reset highlights when not turning
            spawnRotationIndicatorController.StopHighlight();
        }
    }
}
