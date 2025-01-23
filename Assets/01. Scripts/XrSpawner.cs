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
    [SerializeField] private GameObject m_gameSpawnContainer;

    [Tooltip("Material to use when spawning the golf track")]
    [SerializeField] private Material m_spawnMaterial;

    [Header("Aim Settings")]
    [Tooltip("The Object to Shoot the Beam From")]
    [SerializeField] private Transform m_aimer;

    [Tooltip("Smoothing speed for the aimer")]
    [SerializeField] private float m_aimerSmoothingSpeed = 5f;

    [Tooltip("Layers You Can Spawn On")]
    [SerializeField] private LayerMask m_layer;

    [Tooltip("The Maximum Slope You Can Spawn On")]
    [SerializeField] private float m_maxSurfaceAngle = 45;

    [Min(0)]
    [Tooltip("Distance multiplier for the spawn line")]
    [SerializeField] private float m_distanceMultiplyer = 1;

    [Min(0)]
    [Tooltip("Curve strength for the spawn line")]
    [SerializeField] private float m_curveStrength = 1;

    [Tooltip("Use Worldspace Must be True")]
    [SerializeField] private LineRenderer m_line;

    [Tooltip("Maximum Length of The Spawn Line")]
    [SerializeField] private int m_lineSegments = 50;

    [Header("Line Settings")]
    [Tooltip("Color gradient when spawning is possible")]
    [SerializeField] private Gradient m_canSpawnColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.green, time = 0 } } };
    [Tooltip("Color gradient when spawning is not possible")]
    [SerializeField] private Gradient m_cantSpawnColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.red, time = 0 } } };

    [Header("Unity Events")]
    [Tooltip("Event triggered when spawning starts")]
    [SerializeField] private UnityEvent m_OnStartSpawn;
    [Tooltip("Event triggered when spawning stops")]
    [SerializeField] private UnityEvent m_OnStopSpawn;
    [Tooltip("Event triggered when spawning is completed")]
    [SerializeField] private UnityEvent m_OnSpawn;

    #endregion

    #region Private Fields

    private Vector3[] m_lineArr;
    private bool m_aiming;
    private bool m_hitting;
    private RaycastHit m_aimHit;

    private Vector3 m_currentSpawnSmoothForward;
    private Vector3 m_currentSpawnForward;
    private Vector3 m_currentSpawnPosition;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        m_line.enabled = false;
    }

    /// <summary>
    /// Called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    private void Start()
    {
        m_lineArr = new Vector3[m_lineSegments];
    }

    /// <summary>
    /// Called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void LateUpdate()
    {
        SmoothTargetValues();

        if (m_aiming)
        {
            DrawIndicator();
            CalculateSpawn();
        }
        else
        {
            m_line.positionCount = 0;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the spawning process.
    /// </summary>
    public void StartSpawn()
    {
        m_aiming = true;
        m_OnStartSpawn?.Invoke();
    }

    /// <summary>
    /// Cancels the spawning process.
    /// </summary>
    public void CancelSpawn()
    {
        m_line.positionCount = 0;
        m_line.enabled = false;
        m_hitting = false;
        m_aiming = false;
        m_OnStopSpawn?.Invoke();

        // m_spawnGolfTrack.Despawn();
    }

    /// <summary>
    /// Completes the spawning process.
    /// </summary>
    public void Spawn()
    {
        if (m_hitting)
        {
            if (m_gameSpawnContainer != null)
            {
                var diff = m_aimHit.point - m_gameSpawnContainer.transform.position;
                m_gameSpawnContainer.transform.position = m_aimHit.point;
            }
            m_gameSpawnContainer.transform.position = m_aimHit.point;

            m_OnSpawn?.Invoke();
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
        m_currentSpawnForward = m_aimer.forward;
        m_currentSpawnPosition = m_aimer.position;
        m_currentSpawnSmoothForward = Vector3.Lerp(m_currentSpawnSmoothForward, m_currentSpawnForward, Time.deltaTime * m_aimerSmoothingSpeed);
    }

    /// <summary>
    /// Calculates the spawn position and updates the line renderer.
    /// </summary>
    private void CalculateSpawn()
    {
        m_line.colorGradient = m_cantSpawnColor;
        var lineList = new List<Vector3>();
        int i;
        m_hitting = false;
        for (i = 0; i < m_lineSegments; i++)
        {
            var time = i / 60f;
            m_lineArr[i] = m_currentSpawnPosition;
            m_lineArr[i] += m_currentSpawnSmoothForward * time * m_distanceMultiplyer * 15;
            m_lineArr[i].y += m_curveStrength * (time - Mathf.Pow(9.8f * 0.5f * time, 2));
            lineList.Add(m_lineArr[i]);
            if (i != 0)
            {
                if (Physics.Raycast(m_lineArr[i - 1], m_lineArr[i] - m_lineArr[i - 1], out m_aimHit, Vector3.Distance(m_lineArr[i], m_lineArr[i - 1]), ~Hand.GetHandsLayerMask(), QueryTriggerInteraction.Ignore))
                {
                    // Makes sure the angle isn't too steep
                    if (Vector3.Angle(m_aimHit.normal, Vector3.up) <= m_maxSurfaceAngle && m_layer == (m_layer | (1 << m_aimHit.collider.gameObject.layer)))
                    {
                        m_line.colorGradient = m_canSpawnColor;
                        lineList.Add(m_aimHit.point);
                        m_hitting = true;
                        break;
                    }
                    break;
                }
            }
        }
        m_line.enabled = true;
        m_line.positionCount = i;
        m_line.SetPositions(m_lineArr);
    }

    /// <summary>
    /// Draws the indicator for the spawn position.
    /// </summary>
    private void DrawIndicator()
    {
        if (m_hitting)
        {
            m_gameSpawnContainer.gameObject.SetActive(true);
            m_gameSpawnContainer.transform.position = m_aimHit.point;
            m_gameSpawnContainer.transform.up = m_aimHit.normal;
        }
        else
        {
            m_gameSpawnContainer.gameObject.SetActive(false);
        }
    }

    #endregion
}
