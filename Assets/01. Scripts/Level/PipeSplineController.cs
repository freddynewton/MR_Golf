using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Controls the behavior of the pipe spline when a golf ball enters it.
/// </summary>
public class PipeSplineController : MonoBehaviour
{
    [Header("Settings")]
    [Min(0)][SerializeField] private float m_baseSpeed = 1; // Base speed for the spline animation
    [Min(0)][SerializeField] private float m_lerpToStartDuration = 0.3f; // Duration to lerp to the start of the spline

    [Header("References")]
    [SerializeField] private SplineContainer m_splineContainer; // Reference to the spline container

    /// <summary>
    /// Triggered when another collider enters the trigger collider attached to the object where this script is attached.
    /// </summary>
    /// <param name="other">The collider that enters the trigger.</param>
    private async void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is tagged as "Golfball"
        if (other.gameObject.CompareTag("Golfball"))
        {
            // Get the GolfballController component from the entering object
            GolfballController golfballController = other.GetComponent<GolfballController>();

            // Lerp to the start point of the spline
            await UniTask.WhenAll(LerpToStartPoint(golfballController, this.GetCancellationTokenOnDestroy()));

            // Add a SplineAnimate component to the golf ball
            AddSplineAnimator(golfballController);
        }
    }

    /// <summary>
    /// Adds a SplineAnimate component to the golf ball.
    /// </summary>
    /// <param name="golfballController"></param>
    private void AddSplineAnimator(GolfballController golfballController)
    {
        // Add a SplineAnimate component to the golf ball
        SplineAnimate splineAnimate = golfballController.gameObject.AddComponent<SplineAnimate>();
        splineAnimate.AnimationMethod = SplineAnimate.Method.Speed; // Set animation method to speed
        splineAnimate.MaxSpeed = Mathf.Clamp(golfballController.Rigidbody.linearVelocity.magnitude, m_baseSpeed, float.MaxValue); // Clamp the speed
        splineAnimate.Easing = SplineAnimate.EasingMode.EaseIn; // Set easing mode
        splineAnimate.Container = m_splineContainer; // Set the spline container
        splineAnimate.Loop = SplineAnimate.LoopMode.Once; // Set loop mode to once

        // Destroy the SplineAnimate component once the animation is completed
        splineAnimate.Completed += () => Destroy(splineAnimate);

        // Start the spline animation
        splineAnimate.Play();
    }

    /// <summary>
    /// Lerps the golf ball to the start point of the spline.
    /// </summary>
    private UniTask LerpToStartPoint(GolfballController golfballController, CancellationToken ct)
    {
        // Get the start point of the spline
        Vector3 startPoint = m_splineContainer.EvaluatePosition(0);

        // Lerp to the start point and return the task
        return golfballController.transform.DOMove(startPoint, m_lerpToStartDuration).SetEase(Ease.InOutSine).Play().ToUniTask(cancellationToken: ct);
    }
}
