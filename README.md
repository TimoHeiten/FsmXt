# heitech.FsmXt
Very simple way of implementing a Finite state machine.

## Use this Finite state machine implementation like so:

1. Create some classes (at least one) that implements the IState<T> interface.
    Make sure to use the generic interface, the non generic is just for internal use...

2. Create a new Instance of the StateMachine class providing an IEnumerable<IState> to the ctor and a default error handler callback.
    This is needed, when an event cannot be found.
    
3. Send a Message<T> to the StateMachine by invoking the Method TransformState. If a State of the eventName in the Message will be found
    the Statemachine instance changes set state by invoking the SetCurrentState method.
    
## How is this useful?
With this somewhat decoupled state machine you are able to implement different types of IState<T> interfaces for different Events.
Those can then perform some Actions or what not on the SetCurrentState Methods. It decouples your logic from state change and can be very
usefult to keep complex business logic in check.

```csharp
class StateOne : IState<string>
{
   // other members
   public bool HasEvent(string eventName)
      => eventName == "any event";
   
   public void SetCurrentState(string value)
   {
       // some business logic to be invoked here.
   }
}

class Program
{
  static void Main(string args[])
  {
      var stateRegistars = new List<StateRegister>
      {
          new StateRegister("stateOneKey", new StateOne())
      };
      Action onError = () => Console.WriteLine("Error occured");
      
      var machine = new StateMachine(stateRegistars, onError);

      // Invoke with a new instance of the message
      var message = new Message<string> { EventName = "any event", CurrentState = "currentState" };

      machine.TransformState(type: "stateOneKey", message: message, onError: null);
      // will invoke the StateOne.HasEvent, which returns true and then call the StateOne.SetCurrentState("currentState");
   }
}
```
This will allow for multiple scenarios:
- Use the Message to invoke Visitor like Actions on a IState encapsulated State
- Hold on to the References of the State and ask for its State in an Actor like model
- Simple State Machine State
- any other you can come up with ;-D


