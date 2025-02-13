using UniState;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace StateMachine
{
    public class InitialState : StateBase
    {
        InitialState()
        {
            // Set the state name
            // StateName = "Initial State";
        }

        public override UniTask<StateTransitionInfo> Execute(CancellationToken token)
        {


            throw new System.NotImplementedException();
        }
    }
}