using UnityEngine;
using DG.Tweening;

/// <summary>
/// Helper class to manage the visibility of an area plane using shader properties.
/// </summary>
public class AreaPlaneShaderHelper : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Duration of the fade animation.")]
    public float FadeDuration = 1f;

    [Tooltip("Easing function for the fade animation.")]
    [SerializeField] private Ease m_fadeEase = Ease.InBack;

    [Header("References")]
    [Tooltip("MeshRenderer component of the area plane.")]
    [SerializeField] private MeshRenderer m_meshRenderer;

    /// <summary>
    /// Initializes the component.
    /// </summary>
    private void Awake()
    {
        // Ensure the MeshRenderer component is assigned
        m_meshRenderer ??= GetComponent<MeshRenderer>();

        m_meshRenderer.material.SetFloat("_Alpha", 0);
    }



    /// <summary>
    /// Sets the visibility of the area plane.
    /// </summary>
    /// <param name="active">If true, the area plane will be visible; otherwise, it will be hidden.</param>
    public void SetVisibility(bool active)
    {
        // Fade the material's alpha to 1 (visible) or 0 (hidden) over the specified duration with the specified easing
        m_meshRenderer.material.DOKill();
        m_meshRenderer.material.DOFloat(active ? 1 : 0, "_Alpha", FadeDuration).SetEase(m_fadeEase);
    }
}
