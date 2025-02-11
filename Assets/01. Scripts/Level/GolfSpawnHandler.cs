using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.HID;

/// <summary>
/// Handles the rotation of the golf track container based on XR input.
/// </summary>
public class GolfSpawnHandler : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float m_animationDuration = 1;

    [Header("Rotation Settings")]
    [Tooltip("Maximum rotation speed.")]
    [SerializeField] private float maxRotationSpeed = 1f;

    [Tooltip("Time to reach maximum rotation speed.")]
    [SerializeField] private float accelerationTime = 1f;

    [Header("References")]
    [Tooltip("The container for the golf track.")]
    [SerializeField] private Transform golfTrackContainer;

    [SerializeField] private AreaPlaneShaderHelper areaPlane;

    private SpawnRotationIndicatorController spawnRotationIndicatorController;
    private bool isTurning;
    private float currentRotationSpeed;
    private bool isInitialized = false;

    public void SetGolfTrackActive(bool active)
    {
        golfTrackContainer.gameObject.SetActive(active);
        return;

        golfTrackContainer.DOKill();

        golfTrackContainer.localScale = active ? Vector3.zero : Vector3.one;
        golfTrackContainer.DOScale(active ? Vector3.one : Vector3.zero, m_animationDuration).SetEase(Ease.InOutSine);
    }

    public void SetPlaneActive(bool active)
    {

        areaPlane.SetVisibility(active);
    }

    public void SetRotationIndicatorActive(bool active)
    {
        // TODO ANIMATE
        spawnRotationIndicatorController.gameObject.SetActive(active);

        //spawnRotationIndicatorController.transform.DOScale(active ? Vector3.one : Vector3.zero, 0.33f).SetEase(Ease.OutBack);
    }

    public void initialize()
    {
        areaPlane.FadeDuration = m_animationDuration;

        SetPlaneActive(false);
        SetRotationIndicatorActive(false);
        SetGolfTrackActive(false);

        isInitialized = true;
    }

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
        if (!isInitialized)
        {
            return;
        }

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
                spawnRotationIndicatorController.SetHighlight(true); // Highlight right indicator
            }
            else if (turnInput.x < 0)
            {
                spawnRotationIndicatorController.SetHighlight(false); // Highlight left indicator
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
