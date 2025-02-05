using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Controls the golf track, including the golf ball dispenser and golf club.
/// Implements the ISpawnable interface.
/// </summary>
public class GolfTrackController : MonoBehaviour, ISpawnable
{
    [Header("References")]
    [SerializeField] private GameObject m_golfBallDispencer; // Reference to the golf ball dispenser
    [SerializeField] private GameObject m_golfClub; // Reference to the golf club
    [SerializeField] private GameObject m_golfTrack; // Reference to the golf track

    
    private Material m_originalMaterial; // Original material of the golf track
    private Renderer m_renderer;
    private Collider m_collider;

    /// <summary>
    /// Gets the renderer of the golf track.
    /// </summary>
    public Renderer Renderer => m_renderer;

    /// <summary>
    /// Gets or sets the original material of the golf track.
    /// </summary>
    public Material OriginalMaterial
    {
        get => m_originalMaterial;
    }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        m_renderer ??= m_golfTrack.GetComponent<Renderer>();
        m_collider ??= m_golfTrack.GetComponent<Collider>();

        m_golfBallDispencer.SetActive(false);
        m_golfClub.SetActive(false);
        m_collider.enabled = false;
    }

    /// <summary>
    /// Prepares the golf track for spawning by changing its material.
    /// </summary>
    public void PrepareForSpawn(Material spawnMaterial)
    {
        m_collider ??= m_golfTrack.GetComponent<Collider>();
        m_renderer ??= m_golfTrack.GetComponent<Renderer>();

        if (m_originalMaterial == null)
        {
            m_originalMaterial = Renderer.material;
        }

        m_collider.enabled = false;
        Renderer.material = spawnMaterial;
        m_golfTrack.SetActive(true);
    }

    /// <summary>
    /// Spawns the golf track and restores its original material.
    /// </summary>
    public void Spawn()
    {
        m_collider ??= m_golfTrack.GetComponent<Collider>();
        m_renderer ??= m_golfTrack.GetComponent<Renderer>();

        m_collider.enabled = true;
        Renderer.material = m_originalMaterial;
        m_golfTrack.SetActive(true);
        m_golfClub.SetActive(true);
        m_golfBallDispencer.SetActive(true);
    }

    /// <summary>
    /// Despawns the golf track.
    /// </summary>
    public void Despawn()
    {
        m_collider ??= m_golfTrack.GetComponent<Collider>();
        m_renderer ??= m_golfTrack.GetComponent<Renderer>();

        m_golfBallDispencer.SetActive(false);
        m_golfClub.SetActive(false);
        m_golfTrack.SetActive(false);
        m_collider.enabled = false;
    }
}
