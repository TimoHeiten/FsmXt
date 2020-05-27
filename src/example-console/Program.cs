using System;
using heitech.FsmXt;
using Microsoft.Extensions.DependencyInjection;
namespace example_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddStateMachine(() => System.Console.WriteLine("an error occured!"));

            var provider = services.BuildServiceProvider();
            _machine = provider.GetRequiredService<IStateMachine>();

            _message = new Message<string>() { EventName = "any event", NextState = "latest Value for state one" };
            RunNext(state: "State One", notice: "for the second state press any key");

            _message = new Message<string>() { EventName = "another event", NextState = "latest Value for State two" };
            RunNext(state: "State Two", notice: "for error scenario press any key");

            _message = new Message<string> { EventName = "not found", NextState = "does not matter" };
            RunNext(state: "State One", notice:"thats it, finish with pressing a key");
        }

        private static IStateMachine _machine;
        private static Message<string> _message;

        private static void RunNext(string state, string notice)
        {
            _machine.TansformState(state, _message);
            
            System.Console.WriteLine();
            System.Console.WriteLine(notice);
            Console.ReadKey();
        }
    }

    public class StateOne : IState<string>
    {
        public string Name { get; set; } = "State One";
        public bool HasEvent(string eventName)
        {
            System.Console.WriteLine("incoming: " + eventName);
            return  eventName == "any event";
        }

        public void OnError()
        {
            // nothing to do in this simple case.
            System.Console.WriteLine("error occured inside the State");
        }

        public string CurrentState;

        public void SetCurrentState(string value)
        {
            System.Console.WriteLine("state for StateOne was: [" + CurrentState + "]");
            CurrentState = value;
            System.Console.WriteLine("state for StateOne is now: [" + CurrentState + "]");

            // no further action necessary
        }
    }

     public class StateTwo : IState<string>
    {
        public string Name { get; set; } = "State Two";
        public bool HasEvent(string eventName)
        {
            System.Console.WriteLine("incoming: " + eventName);
            return  eventName == "another event";
        }

        public void OnError()
        {
            // nothing to do in this simple case.
            System.Console.WriteLine("error occured inside the State");
        }

        public string CurrentState;

        public void SetCurrentState(string value)
        {
            System.Console.WriteLine("state for StateTwo was: [" + CurrentState + "]");
            CurrentState = value;
            System.Console.WriteLine("state for StateTwo is now: [" + CurrentState + "]");

            // no further action necessary
        }
    }
}
