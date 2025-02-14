using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles the rotation of the golf track container based on XR input.
/// </summary>
public class GolfSpawnHandler : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Duration of the animation.")]
    [SerializeField] private float m_animationDuration = 1;

    [Header("Rotation Settings")]
    [Tooltip("Maximum rotation speed.")]
    [SerializeField] private float m_maxRotationSpeed = 1f;

    [Tooltip("Time to reach maximum rotation speed.")]
    [SerializeField] private float m_accelerationTime = 1f;

    [Header("References")]
    [Tooltip("The container for the golf track.")]
    [SerializeField] private Transform m_golfTrackContainer;

    [Tooltip("The container for the golf tools.")]
    [SerializeField] private Transform m_golfToolsContainer;

    [Tooltip("Helper for managing the visibility of the area plane.")]
    [SerializeField] private AreaPlaneShaderHelper m_areaPlane;

    [Tooltip("Controller for the spawn rotation indicator.")]
    [SerializeField] private SpawnRotationIndicatorController m_spawnRotationIndicatorController;

    private bool m_isTurning;
    private float m_currentRotationSpeed;
    private bool m_isInitialized = false;

    /// <summary>
    /// Sets the active state of the golf track container.
    /// </summary>
    /// <param name="isVisible">If true, the golf track container will be active; otherwise, it will be inactive.</param>
    public void SetGolfTrackContainerVisibility(bool isVisible)
    {
        // TODO: Animate
        m_golfTrackContainer.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// Sets the active state of the area plane.
    /// </summary>
    /// <param name="isVisible">If true, the area plane will be visible; otherwise, it will be hidden.</param>
    public void SetAreaPlaneVisibility(bool isVisible)
    {
        m_areaPlane.SetVisibility(isVisible);
    }

    /// <summary>
    /// Sets the active state of the rotation indicator.
    /// </summary>
    /// <param name="isVisible">If true, the rotation indicator will be active; otherwise, it will be inactive.</param>
    public void SetRotationIndicatorVisibility(bool isVisible)
    {
        // TODO: Animate
        m_spawnRotationIndicatorController.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// Sets the active state of the golf tools container.
    /// </summary>
    /// <param name="isVisible">If true, the golf tools container will be active; otherwise, it will be inactive.</param>
    public void SetGolfToolsContainerVisibility(bool isVisible)
    {
        // TODO: Animate
        m_golfToolsContainer.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// Initializes the golf spawn handler.
    /// </summary>
    public void Initialize()
    {
        m_areaPlane.FadeDuration = m_animationDuration;

        SetAreaPlaneVisibility(false);
        SetRotationIndicatorVisibility(false);
        SetGolfTrackContainerVisibility(false);
        SetGolfToolsContainerVisibility(false);

        m_isInitialized = true;
    }

    /// <summary>
    /// Initializes the component.
    /// </summary>
    private void Awake()
    {
        m_spawnRotationIndicatorController ??= GetComponentInChildren<SpawnRotationIndicatorController>();
    }

    /// <summary>
    /// Updates the rotation based on input.
    /// </summary>
    private void Update()
    {
        if (!m_isInitialized)
        {
            return;
        }

        // TODO: Only update rotation if we are in the spawn state

        Vector2 turnInput = XrInputManager.Instance.RightTurnInputValue;

        if (turnInput.x != 0)
        {
            if (!m_isTurning)
            {
                m_isTurning = true;
                m_currentRotationSpeed = 0f;
            }

            m_currentRotationSpeed = Mathf.MoveTowards(m_currentRotationSpeed, m_maxRotationSpeed, m_maxRotationSpeed / m_accelerationTime * Time.deltaTime);
            float rotationAmount = turnInput.x * m_currentRotationSpeed * Time.deltaTime;
            m_golfTrackContainer.Rotate(Vector3.up, rotationAmount);

            // Set highlight based on the direction of the rotation
            if (turnInput.x > 0)
            {
                m_spawnRotationIndicatorController.SetHighlight(true); // Highlight right indicator
            }
            else if (turnInput.x < 0)
            {
                m_spawnRotationIndicatorController.SetHighlight(false); // Highlight left indicator
            }
        }
        else
        {
            if (!m_isTurning)
            {
                return;
            }

            m_isTurning = false;
            m_currentRotationSpeed = 0f;

            // Reset highlights when not turning
            m_spawnRotationIndicatorController.StopHighlight();
        }
    }
}
