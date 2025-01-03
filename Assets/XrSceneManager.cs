using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class XrSceneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager m_ArPlaneManager;

    private void Start()
    {
        m_ArPlaneManager ??= FindFirstObjectByType<ARPlaneManager>();
    }

    private void Awake()
    {
        SetArPlaneActive(true);
    }

    public void SetArPlaneActive(bool active)
    {
        m_ArPlaneManager.enabled = active;
    }
}
