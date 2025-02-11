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
    private InputAction rightStickClick;

    /// <summary>
    /// Gets the right turn input value.
    /// </summary>
    public Vector2 RightTurnInputValue => rightTurnAction?.ReadValue<Vector2>() ?? Vector2.zero;

    /// <summary>
    /// Gets the right stick click action.
    /// </summary>
    public InputAction RightStickClick => rightStickClick;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Initializes the XR input action maps.
    /// </summary>
    public void Initialize()
    {
        xrInputActions = new XRIDefaultInputActions();

        XriRightInteractionActionMap = xrInputActions.XRIRightInteraction;
        XriLeftInteractionActionMap = xrInputActions.XRILeftInteraction;

        XriRightLocomotionActionMap = xrInputActions.XRIRightLocomotion;
        XriLeftLocomotionActionMap = xrInputActions.XRILeftLocomotion;

        // Find the right turn action in the right locomotion action map
        rightTurnAction = XriRightLocomotionActionMap.FindAction("Turn");

        // Find the right stick click action in the right interaction action map
        rightStickClick = XriRightInteractionActionMap.FindAction("Right Stick Click");

        // Enable the input actions
        xrInputActions.Enable();
    }
}
