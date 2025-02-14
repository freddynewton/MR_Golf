using StateMachine;
using UniState;
using UnityEngine;
using VContainer;
using VContainer.Unity;


/// <summary>
/// Manages the main components of the application, including the XR spawner and golf spawn handler.
/// </summary>
public class MainManager : Singleton<MainManager>
{
    /// <summary>
    /// Gets the XR spawner component.
    /// </summary>
    public XrSpawner XrSpawner => m_XrSpawner;

    /// <summary>
    /// Gets the golf spawn handler component.
    /// </summary>
    public GolfSpawnHandler GolfSpawnHandler => m_GolfSpawnHandler;

    [Header("References")]
    [Tooltip("Reference to the XR spawner component.")]
    [SerializeField] private XrSpawner m_XrSpawner;

    [Tooltip("Reference to the golf spawn handler component.")]
    [SerializeField] private GolfSpawnHandler m_GolfSpawnHandler;

    private GameStateMachine GameStateMachine = new();
    private GameLifetimeScope GameLifetimeScope = new();

    /// <summary>
    /// Initializes the main manager and its components.
    /// </summary>
    public override void Awake()
    {
        base.Awake();

        // Set GameLifetimeScope and GameStateMachine
        GameLifetimeScope ??= new ();
        GameStateMachine ??= new ();

        // VContainer.IObjectResolver _objectResolver;

        // var typeResolver = _objectResolver.ToTypeResolver();

        // var stateMachine = StateMachineHelper.CreateStateMachine(GameStateMachine, GameLifetimeScope);

        // TODO: REDO BELOW
        // Find and assign the XR spawner and golf spawn handler components if not already assigned
        m_XrSpawner ??= FindFirstObjectByType<XrSpawner>();
        m_GolfSpawnHandler ??= FindFirstObjectByType<GolfSpawnHandler>();

        // Initialize the XR input manager, XR spawner, and golf spawn handler
        XrInputManager.Instance.Initialize();
        XrSpawner.Initilize();
        m_GolfSpawnHandler.Initialize();
    }
}
