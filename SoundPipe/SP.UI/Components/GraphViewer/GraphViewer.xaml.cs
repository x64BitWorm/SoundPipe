using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SP.UI.Components.GraphViewer.Events;
using SP.UI.Utils;

namespace SP.UI.Components.GraphViewer
{
    public partial class GraphViewer : UserControl
    {
        public static readonly DependencyProperty NodesProperty =
            DependencyProperty.Register(nameof(Nodes), typeof(GraphNode[]), typeof(GraphViewer),
                new PropertyMetadata(null, OnNodesChanged));

        public static readonly RoutedEvent NodeClickEvent =
            EventManager.RegisterRoutedEvent(nameof(NodeClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                typeof(GraphViewer));

        public static readonly RoutedEvent ContextOpeningEvent =
            EventManager.RegisterRoutedEvent(nameof(ContextOpening), RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                typeof(GraphViewer));

        private Bitmap _canvaBitmap;
        private Graphics _canva;

        private SynchronizationContext _uiContext;
        private Debounce _measureDebounce;
        private Debounce _redrawDebounce;
        private Debounce _tooltipDebounce;
        private int _redrawCount = 0;
        private Dictionary<string, NodeContainer> _nodes;
        private System.Drawing.Point _viewOrigin;

        public GraphNode[] Nodes
        {
            get => (GraphNode[]) GetValue(NodesProperty);
            set => SetValue(NodesProperty, value);
        }

        public event RoutedEventHandler NodeClick
        {
            add => AddHandler(NodeClickEvent, value);
            remove => RemoveHandler(NodeClickEvent, value);
        }

        public event RoutedEventHandler ContextOpening
        {
            add => AddHandler(NodeClickEvent, value);
            remove => RemoveHandler(NodeClickEvent, value);
        }

        public GraphViewer()
        {
            _uiContext = SynchronizationContext.Current;
            _measureDebounce = new Debounce(100, () => MeasureContainerAsync());
            _redrawDebounce = new Debounce(16, () => RedrawAsync());
            _tooltipDebounce = new Debounce(2000, () => DrawTooltipsAsync());
            _nodes = new Dictionary<string, NodeContainer>();
            _viewOrigin = new System.Drawing.Point(0, 0);
            _state = new StateContainer();
            InitializeComponent();
        }

        private static void OnNodesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (GraphViewer)d;
            var value = e.NewValue as GraphNode[];
            source.ReloadNodes();
        }

        private void CanvaSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _measureDebounce.Call();
        }

        private async Task MeasureContainerAsync()
        {
            await _uiContext;
            if (_canvaBitmap != null)
            {
                _canvaBitmap.Dispose();
            }
            var sizeInPixels = WpfUtils.GetElementSizeInPixels(MainPanel);
            _canvaBitmap = new Bitmap((int)sizeInPixels.Width, (int)sizeInPixels.Height);
            _canva = Graphics.FromImage(_canvaBitmap);
            _redrawDebounce.Call();
        }

        private void ReloadNodes()
        {
            var nodes = Nodes;
            _nodes.Clear();
            foreach (var node in nodes)
            {
                _nodes.Add(node.Id, new NodeContainer(node));
            }
            RedrawAsync();
        }

        private async Task RedrawAsync()
        {
            await _uiContext;
            if (_canva == null)
            {
                return;
            }
            _redrawCount++;
            _canva.Clear(Background);
            DrawGrid();
            MeasureNodes(_nodes.Values);
            foreach (var node in _nodes)
            {
                DrawGraphNode(node.Value);
                DrawGraphLinks(node.Value);
            }
            DrawStateAdornments();
            //_canva.DrawString(_redrawCount.ToString(), new Font("Consolas", 10), Brushes.Red, new PointF(5, 5));
            UpdateGraphImage();
        }

        private void UpdateGraphImage()
        {
            Canva.Source = WpfUtils.ImageSourceFromBitmap(_canvaBitmap);
        }
    }
}
