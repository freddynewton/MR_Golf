using Autohand;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the spawning of a golf track in the scene.
/// </summary>
public class XrSpawner : MonoBehaviour
{
    #region Serialized Fields

    [Header("Spawn Settings")]
    [Tooltip("The object to spawn")]
    [SerializeField] private GolfTrackController spawnGolfTrack;

    [Tooltip("Material to use when spawning the golf track")]
    [SerializeField] private Material m_spawnMaterial;

    [Header("Aim Settings")]
    [Tooltip("The Object to Shoot the Beam From")]
    [SerializeField] private Transform aimer;
    [Tooltip("Smoothing speed for the aimer")]
    [SerializeField] private float aimerSmoothingSpeed = 5f;
    [Tooltip("Layers You Can Spawn On")]
    [SerializeField] private LayerMask layer;
    [Tooltip("The Maximum Slope You Can Spawn On")]
    [SerializeField] private float maxSurfaceAngle = 45;
    [Min(0)]
    [Tooltip("Distance multiplier for the spawn line")]
    [SerializeField] private float distanceMultiplyer = 1;
    [Min(0)]
    [Tooltip("Curve strength for the spawn line")]
    [SerializeField] private float curveStrength = 1;
    [Tooltip("Use Worldspace Must be True")]
    [SerializeField] private LineRenderer line;
    [Tooltip("Maximum Length of The Spawn Line")]
    [SerializeField] private int lineSegments = 50;

    [Header("Line Settings")]
    [Tooltip("Color gradient when spawning is possible")]
    [SerializeField] private Gradient canSpawnColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.green, time = 0 } } };
    [Tooltip("Color gradient when spawning is not possible")]
    [SerializeField] private Gradient cantSpawnColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.red, time = 0 } } };

    [Header("Unity Events")]
    [Tooltip("Event triggered when spawning starts")]
    [SerializeField] private UnityEvent OnStartSpawn;
    [Tooltip("Event triggered when spawning stops")]
    [SerializeField] private UnityEvent OnStopSpawn;
    [Tooltip("Event triggered when spawning is completed")]
    [SerializeField] private UnityEvent OnSpawn;

    #endregion

    #region Private Fields

    private Vector3[] lineArr;
    private bool aiming;
    private bool hitting;
    private RaycastHit aimHit;

    private Vector3 currentSpawnSmoothForward;
    private Vector3 currentSpawnForward;
    private Vector3 currentSpawnPosition;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        line.enabled = false;
    }

    /// <summary>
    /// Called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    private void Start()
    {
        lineArr = new Vector3[lineSegments];
    }

    /// <summary>
    /// Called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void LateUpdate()
    {
        SmoothTargetValues();

        if (aiming)
        {
            DrawIndicator();
            CalculateSpawn();
        }
        else
        {
            line.positionCount = 0;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the spawning process.
    /// </summary>
    public void StartSpawn()
    {
        aiming = true;
        spawnGolfTrack.PrepareForSpawn(m_spawnMaterial);
        OnStartSpawn?.Invoke();
    }

    /// <summary>
    /// Cancels the spawning process.
    /// </summary>
    public void CancelSpawn()
    {
        line.positionCount = 0;
        line.enabled = false;
        hitting = false;
        aiming = false;
        OnStopSpawn?.Invoke();

        // spawnGolfTrack.Despawn();
    }

    /// <summary>
    /// Completes the spawning process.
    /// </summary>
    public void Spawn()
    {
        if (hitting)
        {
            if (spawnGolfTrack != null)
            {
                var diff = aimHit.point - spawnGolfTrack.transform.position;
                spawnGolfTrack.transform.position = aimHit.point;
            }
            spawnGolfTrack.transform.position = aimHit.point;
            spawnGolfTrack.Spawn();

            OnSpawn?.Invoke();
        }

        CancelSpawn();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Smooths the target values for the aimer.
    /// </summary>
    private void SmoothTargetValues()
    {
        currentSpawnForward = aimer.forward;
        currentSpawnPosition = aimer.position;
        currentSpawnSmoothForward = Vector3.Lerp(currentSpawnSmoothForward, currentSpawnForward, Time.deltaTime * aimerSmoothingSpeed);
    }

    /// <summary>
    /// Calculates the spawn position and updates the line renderer.
    /// </summary>
    private void CalculateSpawn() 
    {
        line.colorGradient = cantSpawnColor;
        var lineList = new List<Vector3>();
        int i;
        hitting = false;
        for (i = 0; i < lineSegments; i++)
        {
            var time = i / 60f;
            lineArr[i] = currentSpawnPosition;
            lineArr[i] += currentSpawnSmoothForward * time * distanceMultiplyer * 15;
            lineArr[i].y += curveStrength * (time - Mathf.Pow(9.8f * 0.5f * time, 2));
            lineList.Add(lineArr[i]);
            if (i != 0)
            {
                if (Physics.Raycast(lineArr[i - 1], lineArr[i] - lineArr[i - 1], out aimHit, Vector3.Distance(lineArr[i], lineArr[i - 1]), ~Hand.GetHandsLayerMask(), QueryTriggerInteraction.Ignore))
                {
                    // Makes sure the angle isn't too steep
                    if (Vector3.Angle(aimHit.normal, Vector3.up) <= maxSurfaceAngle && layer == (layer | (1 << aimHit.collider.gameObject.layer)))
                    {
                        line.colorGradient = canSpawnColor;
                        lineList.Add(aimHit.point);
                        hitting = true;
                        break;
                    }
                    break;
                }
            }
        }
        line.enabled = true;
        line.positionCount = i;
        line.SetPositions(lineArr);
    }

    /// <summary>
    /// Draws the indicator for the spawn position.
    /// </summary>
    private void DrawIndicator()
    {
        if (hitting)
        {
            spawnGolfTrack.gameObject.SetActive(true);
            spawnGolfTrack.transform.position = aimHit.point;
            spawnGolfTrack.transform.up = aimHit.normal;
        }
        else
        {
            spawnGolfTrack.gameObject.SetActive(false);
        }
    }

    #endregion
}
