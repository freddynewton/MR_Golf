using Autohand.Demo;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpawnRotationIndicatorController : MonoBehaviour
{
    [SerializeField] private Transform _leftIndicator;
    [SerializeField] private Transform _rightIndicator;
    [SerializeField] private float _duration = 1f; // Duration of the rotation
    [SerializeField] private float _highlightScale = 1.3f; // Scale of the highlight
    [SerializeField] private float _highlightDuration = 0.5f; // Duration of the highlight
    [SerializeField] private Ease _ease = Ease.Linear; // Ease of the rotation

    public void SetHighlight(bool isLeft)
    {
        _leftIndicator.DOKill();
        _rightIndicator.DOKill();

        if (isLeft)
        {
            _leftIndicator.DOScale(Vector3.one * _highlightScale, _highlightDuration).SetEase(_ease);
            _rightIndicator.DOScale(Vector3.one, _highlightDuration).SetEase(_ease);
        }
        else
        {
            _leftIndicator.DOScale(Vector3.one, _highlightDuration).SetEase(_ease);
            _rightIndicator.DOScale(Vector3.one * _highlightScale, _highlightDuration).SetEase(_ease);
        }
    }
}
