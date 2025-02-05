using UnityEngine;

namespace Assets._01._Scripts.SuctionSystem
{
    public class TubeSuctionController : MonoBehaviour
    {
        [SerializeField] private float m_power;
        [SerializeField] private Transform m_targetPoint;

        private Rigidbody m_rigidbody;

        private void Update()
        {
            if (m_rigidbody)
            {
                m_rigidbody.AddForce((m_targetPoint.transform.position - m_rigidbody.transform.position) * m_power, ForceMode.VelocityChange);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Golfball"))
            {
                m_rigidbody = other.GetComponent<Rigidbody>();
                m_rigidbody.isKinematic = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Golfball"))
            {
                m_rigidbody = null;
            }
        }

        private void OnDrawGizmos()
        {

        }
    }
}
