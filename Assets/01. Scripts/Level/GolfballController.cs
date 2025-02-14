using UnityEngine;

/// <summary>
/// Controls the behavior of the golf ball, including its movement and interactions.
/// </summary>
public class GolfballController : MonoBehaviour
{
    /// <summary>
    /// Gets the Rigidbody component attached to the golf ball.
    /// </summary>
    public Rigidbody Rigidbody { get { return m_rigidbody; } private set { m_rigidbody = value; } }

    [Header("Settings")]
    [SerializeField] private float m_stoppingSpeed = 0.2f; // Speed below which the trail stops emitting
    [SerializeField] private float m_forceMultiplier = 2.0f; // Multiplier for the force applied on collision

    [Header("References")]
    [SerializeField] private TrailRenderer m_trailRenderer; // Reference to the TrailRenderer component
    [SerializeField] private Rigidbody m_rigidbody; // Reference to the Rigidbody component

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Get References
        m_trailRenderer ??= GetComponent<TrailRenderer>();
        m_rigidbody ??= GetComponent<Rigidbody>();

        // Set Default Values
        m_trailRenderer.emitting = false;
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update()
    {
        // Stop emitting the trail if the ball's speed is below the stopping speed
        if (m_rigidbody.linearVelocity.magnitude < m_stoppingSpeed)
        {
            m_trailRenderer.emitting = false;
        }
    }

    /// <summary>
    /// Called when the collider attached to this object collides with another collider.
    /// </summary>
    /// <param name="collision">Collision data associated with this collision event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a golf club
        if (collision.gameObject.CompareTag("Golfclub"))
        {
            // Start emitting the trail
            m_trailRenderer.emitting = true;

            // Apply force to the ball based on the collision's relative velocity
            m_rigidbody.AddForce(collision.relativeVelocity * m_forceMultiplier, ForceMode.Impulse);
        }
    }
}
