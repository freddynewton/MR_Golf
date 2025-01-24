using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages XR input actions and action maps.
/// </summary>
public class XrInputManager : Singleton<XrInputManager>
{
    /// <summary>
    /// Gets the action map for right hand interactions.
    /// </summary>
    public InputActionMap XriRightInteractionActionMap { get; private set; }

    /// <summary>
    /// Gets the action map for left hand interactions.
    /// </summary>
    public InputActionMap XriLeftInteractionActionMap { get; private set; }

    /// <summary>
    /// Gets the action map for right hand locomotion.
    /// </summary>
    public InputActionMap XriRightLocomotionActionMap { get; private set; }

    /// <summary>
    /// Gets the action map for left hand locomotion.
    /// </summary>
    public InputActionMap XriLeftLocomotionActionMap { get; private set; }

    private XRIDefaultInputActions xrInputActions;
    private InputAction rightTurnAction;

    /// <summary>
    /// Gets the right turn input value.
    /// </summary>
    public Vector2 RightTurnInputValue => rightTurnAction?.ReadValue<Vector2>() ?? Vector2.zero;

    private void OnEnable()
    {
        xrInputActions.Enable();
    }

    private void OnDisable()
    {
        xrInputActions.Disable();
    }

    /// <summary>
    /// Initializes the XR input action maps.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        xrInputActions = new XRIDefaultInputActions();

        XriRightInteractionActionMap = xrInputActions.XRIRightInteraction;
        XriLeftInteractionActionMap = xrInputActions.XRILeftInteraction;

        XriRightLocomotionActionMap = xrInputActions.XRIRightLocomotion;
        XriLeftLocomotionActionMap = xrInputActions.XRILeftLocomotion;

        rightTurnAction = XriRightLocomotionActionMap.FindAction("Turn");

        xrInputActions.Enable();
    }
}
