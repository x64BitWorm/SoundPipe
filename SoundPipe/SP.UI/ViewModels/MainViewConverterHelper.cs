namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        public void SyncUiGraphToGraphConstructor()
        {
            _shemeConstructor.SetLinks(_shemeGraphConverter.ToConstructorView(ShemeNodes));
        }

        public void SyncGraphConstructorToUiGraph()
        {
            ShemeNodes = _shemeGraphConverter.ToUiView(_shemeConstructor.GetLinks());
        }
    }
}
