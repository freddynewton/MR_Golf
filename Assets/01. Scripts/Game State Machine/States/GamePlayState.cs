using Cysharp.Threading.Tasks;
using System.Threading;
using UniState;

namespace StateMachine
{
    public class GamePlayState : StateBase<GameStateMachine>
    {
        public override UniTask Initialize(CancellationToken token)
        {
            return base.Initialize(token);
        }

        public override UniTask<StateTransitionInfo> Execute(CancellationToken token)
        {
            GameStateMachine stateMachine = Payload;

            throw new System.NotImplementedException();
        }
    }
}