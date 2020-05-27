using System.Collections.Generic;

namespace heitech.FsmXt.Tests
{
    public class ErrorState
    {
        public bool WasSet { get; set; } = false;
        public bool WasOverridden { get; set; }
    }
    public class SetupCode
    {
        public static (StateMachine machine, List<StateRegister> states) CreateForTests(ErrorState errorState)
        {
            var reg = new List<StateRegister>
            {
                new StateRegister("no-generic", new StateMocks.StateWithoutGenerics()),
                new StateRegister("string-generic", new StateMocks.StateWithStringParameter())
            };
            return (new StateMachine(reg, () => errorState.WasSet = true), reg);
        }



        public static Message<string> CreateMessage()
        {
            return new Message<string>{ EventName = "event-1", NextState = "abcaffeschnee" };
        }
    }
}