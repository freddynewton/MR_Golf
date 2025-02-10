using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Splines;

public class PipeAccellerator : MonoBehaviour
{
    [SerializeField] private SplineContainer m_splineContainer;

    [SerializeField] private float m_speed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Golfball"))
        {
            Debug.Log("Start");

            StartCoroutine(StartAccellerator(other.GetComponent<Rigidbody>()));
        }
    }

    private IEnumerator StartAccellerator(Rigidbody rigidbody)
    {
        float time = 1f;
        float currentTime = 0f;

        while (currentTime <= time)
        {
            currentTime += Time.deltaTime * m_speed;

            float t = Mathf.Clamp01(currentTime / time);

            // rigidbody.MovePosition(m_splineContainer.EvaluatePosition(t));
            rigidbody.AddForce(m_splineContainer.EvaluatePosition(t) * m_speed, ForceMode.Acceleration);

            yield return null;
        }
    }
}
