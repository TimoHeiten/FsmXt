using System;
using System.Linq;

namespace heitech.FsmXt.Tests
{
    public class StateMocks
    {
        public class StateWithoutGenerics : IState
        {
            public bool ErrorWasSet;
            public string Name { get; set; }

            public bool HasEvent(string eventName)
            {
                return true;
            }

            public void OnError()
            {
                ErrorWasSet = true;
            }
        }

        public class StateWithStringParameter : IState<string>
        {
            public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public bool ErrorWasSet;

            public bool _HasEvent;

            public bool HasEvent(string eventName)
            {
                return _HasEvent;
            }

            public void OnError()
            {
                ErrorWasSet = true;
            }

            public string CurrentState;

            public void SetCurrentState(string value)
            {
                CurrentState = value;
            }
        }
    }
}