using UnityEngine;

public class SpawnRotationIndicatorController : MonoBehaviour
{
    [SerializeField] private Transform _leftIndicator;
    [SerializeField] private Transform _rightIndicator;
    [SerializeField] private float _duration = 1f; // Duration of the rotation

    private Quaternion _targetRotation;
    private float _timeElapsed;

    private void Start()
    {
        _targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (_timeElapsed < _duration)
        {
            _timeElapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, _timeElapsed / _duration);
        }
    }

    // Rotate transform to face forward to the camera
    public void RotateToCamera()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 direction = cameraPosition - transform.position;
        direction.y = 0; // Ignore changes in the y-axis
        _targetRotation = Quaternion.LookRotation(direction);
        _timeElapsed = 0f; // Reset the time elapsed to start the lerp
    }

    // Draw Gizmo to forward direction
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
