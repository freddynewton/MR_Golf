using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    public IContainerBuilder Builder
    {
        get => m_builder;
        private set => m_builder = value;
    }

    private IContainerBuilder m_builder;

    protected override void Configure(IContainerBuilder builder)
    {
        m_builder = builder;
    }
}
