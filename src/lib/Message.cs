namespace heitech.FsmXt
{
    ///<summary>
    /// Message that indicates a State transformation. The actual transformation is done by the state itself.
    ///</summary>
    public class Message<T>
    {
        ///<summary>
        /// The Value to set for the current state in the Representing state.
        ///</summary>
        public T NextState { get; set; }
        ///<summary>
        /// The Eventname that transforms the state
        ///</summary>
        public string EventName { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Message<T>;
            if (other != null)
            {
                return other.EventName == this.EventName;
            }
            else
                return false;
        }

        public override int GetHashCode() 
        {
            return EventName.GetHashCode();
        }
    }
}