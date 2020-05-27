using System;
using System.Linq;
using System.Collections.Generic;

namespace heitech.FsmXt
{
    public class StateMachine : IStateMachine
    {
        private readonly Dictionary<string, IState> _transactions;
        private readonly Action _defaultError;
        ///<summary>
        /// Pass all registers you need. Must not be empty or null
        ///</summary>
        public StateMachine(IEnumerable<StateRegister> stateRegisters, Action defaultError)
        {
            if (stateRegisters == null || stateRegisters.Any() == false)
                throw new ArgumentException("Empty or null registers not allowed!");

            if (defaultError == null)
                throw new ArgumentException("Default error must not be null!");

            _defaultError = defaultError;
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
                    stateOfT.SetCurrentState(msg.NextState);
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
                var error = onError ?? _defaultError;
                error.Invoke();
            }
        }

        private bool IsSameGenericType<T>(IState state)
        {
            var firstOfStateOf_T_ = state.GetType()
                                         .GetInterfaces()
                                         .FirstOrDefault(x => x == typeof(IState<T>));

            if (firstOfStateOf_T_ == null)
            {
                return false;
            }

            var types = firstOfStateOf_T_.GenericTypeArguments;

            return typeof(T) == types.FirstOrDefault();
        }
    }
}