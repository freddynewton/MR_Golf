using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    public XrSpawner XrSpawner { get { return m_XrSpawner; } private set { m_XrSpawner = value; } }
    
    [SerializeField] private XrSpawner m_XrSpawner;

    public override void Awake()
    {
        base.Awake();

        XrInputManager.Instance.Initialize();
        XrSpawner.Initiliaze();
    }
}
