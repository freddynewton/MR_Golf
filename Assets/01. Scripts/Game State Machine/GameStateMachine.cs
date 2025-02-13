using Cysharp.Threading.Tasks;
using System.Threading;
using UniState;

namespace StateMachine
{
    public class GameStateMachine : IStateMachine
    {
        public void Initialize(ITypeResolver resolver)
        {
            throw new System.NotImplementedException();
        }

        UniTask IExecutableStateMachine.Execute<TState>(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        UniTask IExecutableStateMachine.Execute<TState, TPayload>(TPayload payload, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}