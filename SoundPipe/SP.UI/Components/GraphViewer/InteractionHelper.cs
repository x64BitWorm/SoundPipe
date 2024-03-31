using SP.UI.Components.GraphViewer.Interaction;
using SP.UI.Utils;
using System;
using System.Drawing;
using System.Linq;

namespace SP.UI.Components.GraphViewer
{
    public partial class GraphViewer
    {
        public Collision GetCollision(Point point)
        {
            foreach (var element in _nodes)
            {
                var container = element.Value;
                Rectangle inputIntersection = container.InputsArea.FirstOrDefault(r => GeomUtils.RectIntersects(r, point));
                if (inputIntersection != default)
                {
                    return new JointCollision(point)
                    {
                        Container = container,
                        IsInput = true,
                        LinkNumber = Array.IndexOf(container.InputsArea, inputIntersection)
                    };
                }
                Rectangle outputIntersection = container.OutputsArea.FirstOrDefault(r => GeomUtils.RectIntersects(r, point));
                if (outputIntersection != default)
                {
                    return new JointCollision(point)
                    {
                        Container = container,
                        IsInput = false,
                        LinkNumber = Array.IndexOf(container.OutputsArea, outputIntersection)
                    };
                }
                if (GeomUtils.RectIntersects(container.Area, point))
                {
                    return new NodeCollision(point)
                    {
                        Container = container
                    };
                }
                foreach (var link in container.InputsLinks)
                {
                    if (link.Intersects(point))
                    {
                        return new LinkCollision(point)
                        {
                            Container = container,
                            LinkNumber = Array.IndexOf(container.InputsLinks, link)
                        };
                    }
                }
            }
            return null;
        }
    }
}
