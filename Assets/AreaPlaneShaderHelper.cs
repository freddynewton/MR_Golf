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

    private Material areaPlaneMaterial;

    /// <summary>
    /// Initializes the component.
    /// </summary>
    private void Awake()
    {
        // Ensure the MeshRenderer component is assigned
        m_meshRenderer ??= GetComponent<MeshRenderer>();

        // Get the material from the MeshRenderer
        areaPlaneMaterial = m_meshRenderer.material;

        areaPlaneMaterial.SetFloat("_Alpha", 0);

        SetVisibility(false);
    }



    /// <summary>
    /// Sets the visibility of the area plane.
    /// </summary>
    /// <param name="active">If true, the area plane will be visible; otherwise, it will be hidden.</param>
    public void SetVisibility(bool active)
    {
        // Fade the material's alpha to 1 (visible) or 0 (hidden) over the specified duration with the specified easing
        areaPlaneMaterial.DOKill();
        areaPlaneMaterial.DOFloat(active ? 1 : 0, "_Alpha", FadeDuration).SetEase(m_fadeEase);
    }
}
