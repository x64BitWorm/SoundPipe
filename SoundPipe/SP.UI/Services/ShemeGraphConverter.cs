using SP.Domain;
using SP.Domain.Models;
using SP.UI.Components.GraphViewer;
using SP.UI.Models;
using SP.UI.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace SP.UI.Services
{
    public class ShemeGraphConverter
    {
        private readonly FiltersManager _filtersManager;

        public ShemeGraphConverter(FiltersManager filtersManager)
        {
            _filtersManager = filtersManager;
        }

        public GraphNode[] ToUiView(FilterNode<UINodeInfo>[] from)
        {
            var result = new GraphNode[from.Length];
            var freeOutputIndex = new Dictionary<string, int>();
            for (int i = 0; i < from.Length; i++)
            {
                var node = from[i];
                var filterInfo = _filtersManager.GetFilterMetaInfo(node.Type);
                var graphNode = new GraphNode()
                {
                    Id = node.Id,
                    BorderColor = WpfUtils.DrawingColorFromMediaColor((Color)ColorConverter.ConvertFromString(node.MetaInfo.Color)),
                    Label = node.MetaInfo.Label,
                    Popup = node.Type,
                    X = node.MetaInfo.X,
                    Y = node.MetaInfo.Y
                };
                graphNode.Inputs = node.Dependencies.Select(d => new GraphNodeLink()
                {
                    Description = d.Description,
                    Destination = d.Id,
                    Index = d.Id == null ? 0 : freeOutputIndex.ContainsKey(d.Id) 
                    ? (++freeOutputIndex[d.Id])
                    : (freeOutputIndex[d.Id] = 0)
                }).ToArray();
                switch (filterInfo.GetFilterType())
                {
                    case SDK.Primitives.FilterType.Consumer:
                        graphNode.Outputs = new GraphNodeLink[0];
                        break;
                    case SDK.Primitives.FilterType.Provider:
                        graphNode.Outputs = new GraphNodeLink[]
                        {
                            new GraphNodeLink()
                            {
                                Description = "Выходной звуковой поток",
                                Destination = null,
                                Index = 0
                            }
                        };
                        break;
                    case SDK.Primitives.FilterType.SequentialProvider:
                        graphNode.Outputs = new GraphNodeLink[]
                        {
                            new GraphNodeLink()
                            {
                                Description = "Выходной параллельный поток 1",
                                Destination = null,
                                Index = 0
                            },
                            new GraphNodeLink()
                            {
                                Description = "Выходной параллельный поток 2",
                                Destination = null,
                                Index = 0
                            }
                        };
                        break;
                }
                result[i] = graphNode;
            }
            for (int i = 0; i < from.Length; i++)
            {
                var node = from[i];
                foreach (var input in result[i].Inputs)
                {
                    var outputNode = result.FirstOrDefault(n => n.Id == input.Destination);
                    if (outputNode == null)
                    {
                        continue;
                    }
                    var firstEmpty = outputNode.Outputs.FirstIndex(n => n.Destination == null);
                    outputNode.Outputs[firstEmpty].Destination = result[i].Id;
                    outputNode.Outputs[firstEmpty].Index = result[i].Inputs.FirstIndex(n => n.Destination == outputNode.Id);
                }
            }
            return result;
        }

        public FilterNode<UINodeInfo>[] ToConstructorView(GraphNode[] from)
        {
            var result = new FilterNode<UINodeInfo>[from.Length];
            for (int i = 0; i < from.Length; i++)
            {
                var node = from[i];
                result[i] = new FilterNode<UINodeInfo>()
                {
                    Id = node.Id,
                    MetaInfo = new UINodeInfo()
                    {
                        X = node.X,
                        Y = node.Y,
                        Label = node.Label,
                        Color = WpfUtils.ColorToString(node.BorderColor)
                    },
                    Dependencies = node.Inputs.Select(input => new FilterDependency(input?.Destination)).ToArray()
                };
            }
            return result;
        }
    }
}
