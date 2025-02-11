using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    public XrSpawner XrSpawner => m_XrSpawner;
    public GolfSpawnHandler GolfSpawnHandler => m_GolfSpawnHandler;
    
    [SerializeField] private XrSpawner m_XrSpawner;
    [SerializeField] private GolfSpawnHandler m_GolfSpawnHandler;



    public override void Awake()
    {
        base.Awake();

        m_XrSpawner ??= FindFirstObjectByType<XrSpawner>();
        m_GolfSpawnHandler ??= FindFirstObjectByType<GolfSpawnHandler>();

        XrInputManager.Instance.initialize();
        XrSpawner.Initilize();
        m_GolfSpawnHandler.initialize();
    }
}
