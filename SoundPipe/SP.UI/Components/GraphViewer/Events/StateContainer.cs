namespace SP.UI.Components.GraphViewer.Events
{
    public class StateContainer
    {
        public StateType State { get; set; }
        public StateContext Context { get; set; }

        public StateContainer()
        {
            State = StateType.Empty;
        }
    }
}
