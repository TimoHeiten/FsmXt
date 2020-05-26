namespace heitech.FsmXt
{
    ///<summary>
    /// Use to Register different states with their respective Name.
    ///</summary>
    public class StateRegister
    {
        public StateRegister(string key, IState state)
        {
            Key = key;
            State = state;
        }
        ///<summary>
        /// The associated States Key
        ///</summary>
        public string Key { get; }
        ///<summary>
        /// The actual State Representation
        ///</summary>
        public IState State { get; }
    }
}