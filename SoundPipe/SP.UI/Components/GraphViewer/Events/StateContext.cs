namespace SP.UI.Components.GraphViewer.Events
{
    public class StateContext
    {
        public System.Drawing.Point Position { get; set; }
        public int MouseButton { get; set; }
        public int MouseState { get; set; }
        public object Argument { get; set; }

        public StateContext(System.Drawing.Point position, object argument)
        {
            Position = position;
            Argument = argument;
        }
    }
}
