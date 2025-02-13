using Cysharp.Threading.Tasks;
using System.Threading;
using UniState;

namespace StateMachine
{
    public class PlacementState : StateBase<(GolfSpawnHandler, XrSpawner)>
    {
        private XrSpawner m_XrSpawner;
        private GolfSpawnHandler m_GolfSpawnHandler;

        public override UniTask Initialize(CancellationToken token)
        {
            m_XrSpawner = Payload.Item2;
            m_GolfSpawnHandler = Payload.Item1;

            return UniTask.CompletedTask;
        }

        public override UniTask<StateTransitionInfo> Execute(CancellationToken token)
        {
            m_XrSpawner.StartSpawn();

            m_GolfSpawnHandler.SetGolfTrackActive(true);
            m_GolfSpawnHandler.SetPlaneActive(true);
            m_GolfSpawnHandler.SetRotationIndicatorActive(true);

            StateTransitionInfo transitionInfo = new StateTransitionInfo();
            

            return UniTask.FromResult(new StateTransitionInfo());
        }
    }
}