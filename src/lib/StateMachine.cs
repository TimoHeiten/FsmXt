using System;
using System.Linq;
using System.Collections.Generic;

namespace FsmXt
{
    public class StateMachine
    {
        private readonly Dictionary<string, IState> _transactions;
        ///<summary>
        /// 
        ///</summary>
        public StateMachine(IEnumerable<StateRegister> stateRegisters)
        {
            if (stateRegisters.Any() == false || stateRegisters == null)
                throw new ArgumentException("Empty or null registers not allowed!");

            _transactions = stateRegisters.ToDictionary(x => x.Key, y => y.State);
        }

        ///<summary>
        /// Invoke the Statemachine by eventName and Message. OnError overrides any error behavior.
        ///</summary>
        public void TansformState<T>(string type, Message<T> msg, Action onError = null)
        {
            // does the state exist
            if (_transactions.TryGetValue(type, out IState state))
            {
                // state matches expected state and message
                bool hasEvent = state.HasEvent(msg.EventName);
                bool genericsMatch = IsSameGenericType<T>(state);
                if (hasEvent && genericsMatch)
                {
                    var stateOfT = state as IState<T>;
                    stateOfT.SetCurrentState(msg.StateValue);
                }
                else
                {
                    var err = onError ?? state.OnError;
                    err();
                }
            }
            // no state found
            else
            {
                string _msg = $"Did not find a State for Type: [{type}] && Message: [{msg}]";
                var error = onError ?? new Action(() => System.Console.WriteLine(_msg));
                error.Invoke();
            }
        }

        private bool IsSameGenericType<T>(IState state)
        {
            var types = state.GetType().GenericTypeArguments;

            return typeof(T) == types.First();
        }
    }
}