using Autohand;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XrSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The object to spawn")]
    public GolfTrackController spawnGolfTrack;

    [SerializeField] private Material m_spawnMaterial; // Material to use when spawning the golf track

    [Header("Aim Settings")]
    [Tooltip("The Object to Shoot the Beam From")]
    public Transform aimer;
    public float aimerSmoothingSpeed = 5f;
    [Tooltip("Layers You Can Spawn On")]
    public LayerMask layer;
    [Tooltip("The Maximum Slope You Can Spawn On")]
    public float maxSurfaceAngle = 45;
    [Min(0)]
    public float distanceMultiplyer = 1;
    [Min(0)]
    public float curveStrength = 1;
    [Tooltip("Use Worldspace Must be True")]
    public LineRenderer line;
    [Tooltip("Maximum Length of The Spawn Line")]
    public int lineSegments = 50;

    [Header("Line Settings")]
    public Gradient canSpawnColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.green, time = 0 } } };
    public Gradient cantSpawnColor = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey() { color = Color.red, time = 0 } } };


    [Header("Unity Events")]
    public UnityEvent OnStartSpawn;
    public UnityEvent OnStopSpawn;
    public UnityEvent OnSpawn;

    Vector3[] lineArr;
    bool aiming;
    bool hitting;
    RaycastHit aimHit;

    Vector3 currentSpawnSmoothForward;
    Vector3 currentSpawnForward;
    Vector3 currentSpawnPosition;

    private void Awake()
    {
        line.enabled = false;
    }

    private void Start()
    {
        lineArr = new Vector3[lineSegments];
    }

    void LateUpdate()
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

    void SmoothTargetValues()
    {
        currentSpawnForward = aimer.forward;
        currentSpawnPosition = aimer.position;
        currentSpawnSmoothForward = Vector3.Lerp(currentSpawnSmoothForward, currentSpawnForward, Time.deltaTime * aimerSmoothingSpeed);
    }

    void CalculateSpawn()
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
                    //Makes sure the angle isnt too steep
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

    void DrawIndicator()
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

    public void StartSpawn()
    {
        aiming = true;
        spawnGolfTrack.PrepareForSpawn(m_spawnMaterial);
        OnStartSpawn?.Invoke();
    }

    public void CancelSpawn()
    {
        line.positionCount = 0;
        line.enabled = false;
        hitting = false;
        aiming = false;
        OnStopSpawn?.Invoke();

        spawnGolfTrack.Despawn();
    }

    public void Spawn()
    {
        Queue<Vector3> fromPos = new Queue<Vector3>();

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

            return;
        }

        CancelSpawn();
    }
}
