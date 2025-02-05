using DG.Tweening;
using UnityEngine;

public class SpawnRotationIndicatorController : MonoBehaviour
{
    [SerializeField] private Transform _leftIndicator;
    [SerializeField] private Transform _rightIndicator;
    [SerializeField] private float _duration = 1f; // Duration of the rotation
    [SerializeField] private float _highlightScale = 1.3f; // Scale of the highlight
    [SerializeField] private float _highlightDuration = 0.5f; // Duration of the highlight
    [SerializeField] private Ease _ease = Ease.Linear; // Ease of the rotation
    [SerializeField] private float _rotationSpeed = 5f; // Speed of the smooth rotation

    private void Update()
    {
        BillboardToCamera();
    }

    /// <summary>
    /// Stops the highlight animation and resets the scale.
    /// </summary>
    public void StopHighlight()
    {
        _leftIndicator.DOKill();
        _rightIndicator.DOKill();

        _leftIndicator.DOScale(new Vector3(-1, 1, _leftIndicator.localScale.z), _highlightDuration).SetEase(_ease);
        _rightIndicator.DOScale(new Vector3(1, 1, _rightIndicator.localScale.z), _highlightDuration).SetEase(_ease);
    }

    /// <summary>
    /// Sets the highlight for the specified indicator.
    /// </summary>
    /// <param name="isLeft">If true, highlights the left indicator; otherwise, highlights the right indicator.</param>
    public void SetHighlight(bool isLeft)
    {
        if (isLeft)
        {
            _leftIndicator.DOScale(new Vector3(-1 * _highlightScale, 1 * _highlightScale, _leftIndicator.localScale.z), _highlightDuration).SetEase(_ease);
            _rightIndicator.DOScale(new Vector3(1, 1, _rightIndicator.localScale.z), _highlightDuration).SetEase(_ease);
        }
        else
        {
            _leftIndicator.DOScale(new Vector3(-1, 1, _leftIndicator.localScale.z), _highlightDuration).SetEase(_ease);
            _rightIndicator.DOScale(new Vector3(1 * _highlightScale, 1 * _highlightScale, _rightIndicator.localScale.z), _highlightDuration).SetEase(_ease);
        }
    }

    /// <summary>
    /// Rotates the object to face the main camera on the y-axis smoothly.
    /// </summary>
    private void BillboardToCamera()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 direction = cameraPosition - transform.position;
        direction.y = 0; // Ignore changes in the y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
