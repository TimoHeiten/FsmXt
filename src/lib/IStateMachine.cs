using System;

namespace heitech.FsmXt
{
    public interface IStateMachine
    {
        void TansformState<T>(string type, Message<T> msg, Action onError = null);
    }
}