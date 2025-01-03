using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class XrSceneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager m_ArPlaneManager;
    [SerializeField] private ARCameraManager m_ARCameraManager;
    [SerializeField] private ARCameraBackground m_ARCameraBackground;

    private void Start()
    {
        m_ArPlaneManager ??= FindFirstObjectByType<ARPlaneManager>();
        m_ARCameraManager ??= FindFirstObjectByType<ARCameraManager>();
        m_ARCameraBackground ??= FindFirstObjectByType<ARCameraBackground>();
    }

    private void Awake()
    {
        SetArPlaneActive(true);
        StartCoroutine(FadePassthrough(true));
    }

    public void SetArPlaneActive(bool active)
    {
        m_ArPlaneManager.enabled = active;

        if (m_ARCameraManager != null)
        {
            m_ARCameraManager.enabled = active;
        }

        if (m_ARCameraBackground != null)
        {
            m_ARCameraBackground.enabled = active;
        }
    }

    private IEnumerator FadePassthrough(bool active)
    {
        yield return new WaitForSeconds(2f);
    }
}
