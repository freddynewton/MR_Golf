using UnityEngine;

/// <summary>
/// Checks if a golf ball has entered the goal area and triggers a particle effect.
/// </summary>
public class GolfTrackGoalChecker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem m_goalParticleSystem; // Particle system to play when a goal is detected

    /// <summary>
    /// Called when another collider enters the trigger collider attached to the object where this script is attached.
    /// </summary>
    /// <param name="other">The collider that enters the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "GolfBall"
        if (other.gameObject.CompareTag("GolfBall"))
        {
            // Destroy the golf ball object after 5 seconds
            Destroy(other.gameObject, 5);
            // Play the goal particle system
            m_goalParticleSystem.Play();
        }
    }
}
