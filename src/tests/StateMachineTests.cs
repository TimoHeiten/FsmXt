using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace heitech.FsmXt.Tests
{
    public class StateMachineTests
    {
        [Fact]
        public void CtorDoesNotAllowEmptyOrNullStates()
        {
            Assert.Throws<ArgumentException>(() => new StateMachine(null, () => {}));
            Assert.Throws<ArgumentException>(() => new StateMachine(Enumerable.Empty<StateRegister>(), () => {}));

            Assert.Throws<ArgumentException>(() => new StateMachine
            (
                new List<StateRegister>
                { 
                    new StateRegister("abc", new StateMocks.StateWithoutGenerics())
                },
                null)
            );
        }

        [Fact]
        public void EventDoesNotExistsInvokesStandardError()
        {
            var errState = new ErrorState();
            var (machine, reg) = SetupCode.CreateForTests(errState);
            var msg = SetupCode.CreateMessage();

            machine.TansformState("not existing", msg);

            Assert.True(errState.WasSet);
        }

        [Fact]
        public void EventDoesNotExistsInvokesOverriddenOnError()
        {
            var errState = new ErrorState();
            var (machine, reg) = SetupCode.CreateForTests(errState);
            var msg = SetupCode.CreateMessage();

            machine.TansformState("not existing", msg, () =>  { errState.WasOverridden = true; /* errState.WasSet = false; */ } );

            Assert.False(errState.WasSet);
            Assert.True(errState.WasOverridden);
        }

        [Fact]
        public void EventFoundButTypeNotMatchingInvokesStateOnError()
        {
            var errState = new ErrorState();
            var (machine, reg) = SetupCode.CreateForTests(errState);
            var msg = SetupCode.CreateMessage();

            machine.TansformState("no-generic", msg);

            var nonGeneric = reg.First().State as StateMocks.StateWithoutGenerics;
            Assert.NotNull(nonGeneric);

            Assert.True(nonGeneric.ErrorWasSet);
        }

        [Fact]
        public void EventFoundButTypeNotMatchingInvokesOnErrorOverridden()
        {
             var errState = new ErrorState();
            var (machine, reg) = SetupCode.CreateForTests(errState);
            var msg = SetupCode.CreateMessage();

            machine.TansformState("no-generic", msg, () => errState.WasSet = true);

            var nonGeneric = reg.First().State as StateMocks.StateWithoutGenerics;
            Assert.NotNull(nonGeneric);

            Assert.False(nonGeneric.ErrorWasSet);
            Assert.True(errState.WasSet);
        }
    }
}