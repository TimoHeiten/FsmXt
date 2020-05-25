namespace FsmXt
{
    ///<summary>
    /// IState<T> interface to set the currentState and invoke further changes if necessary.
    ///</summary>
    public interface IState<T> : IState
    {
        void SetCurrentState(T value);
    }

    public interface IState
    {
        ///<summary>
        /// Name associated with the State Representation
        ///</summary>
        string Name { get; set; }
        ///<summary>
        /// Check if the asked for event exists for this State
        ///</summary>
        bool HasEvent(string eventName);
        ///<summary>
        /// Handle any Errors
        ///</summary>
        void OnError();
    }
}