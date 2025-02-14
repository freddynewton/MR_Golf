using UnityEngine;

public class GolfballController : MonoBehaviour
{
    public Rigidbody Rigidbody { get { return m_rigidbody; } private set { m_rigidbody = value; } }

    [Header("Settings")]
    [SerializeField] private float m_stoppingSpeed = 0.2f;
    [SerializeField] private float m_forceMultiplier = 2.0f;

    [Header("References")]
    [SerializeField] private TrailRenderer m_trailRenderer;
    [SerializeField] private Rigidbody m_rigidbody;

    private void Awake()
    {
        // Get References
        m_trailRenderer ??= GetComponent<TrailRenderer>();
        m_rigidbody ??= GetComponent<Rigidbody>();

        // Set Default Values
        m_trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (m_rigidbody.linearVelocity.magnitude < m_stoppingSpeed)
        {
            m_trailRenderer.emitting = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Golfclub"))
        {
            m_trailRenderer.emitting = true;

            m_rigidbody.AddForce(collision.relativeVelocity * m_forceMultiplier, ForceMode.Impulse);
        }
    }
}
