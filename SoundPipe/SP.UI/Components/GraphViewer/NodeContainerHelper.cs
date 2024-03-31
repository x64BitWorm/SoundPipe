using SP.UI.Components.GraphViewer.Interaction;
using SP.UI.Utils;
using System.Drawing;
using System.Xml.Linq;

namespace SP.UI.Components.GraphViewer
{
    public partial class GraphViewer
    {
        private NodeJoint GetOutputSide(GraphNode node, int inputIndex)
        {
            var outputNodeName = node.Inputs[inputIndex];
            if (outputNodeName.Destination == null)
            {
                return null;
            }
            var outputNode = _nodes[outputNodeName.Destination];
            var outputJointIndex = outputNode.Node.Outputs.FirstIndex(o => o.Destination == node.Id);
            return new NodeJoint()
            {
                IsInput = false,
                Node = outputNode.Node,
                Index = outputJointIndex
            };
        }

        private NodeJoint GetInputSide(GraphNode node, int outputIndex)
        {
            var inputNodeName = node.Outputs[outputIndex];
            if (inputNodeName.Destination == null)
            {
                return null;
            }
            var inputNode = _nodes[inputNodeName.Destination];
            var inputJointIndex = inputNode.Node.Inputs.FirstIndex(o => o.Destination == node.Id);
            return new NodeJoint()
            {
                IsInput = true,
                Node = inputNode.Node,
                Index = inputJointIndex
            };
        }

        private void RemoveJoint(NodeJoint joint)
        {
            if (joint.IsInput)
            {
                var reverseJoint = GetOutputSide(joint.Node, joint.Index);
                reverseJoint.Node.Outputs[reverseJoint.Index].Destination = null;
                joint.Node.Inputs[joint.Index].Destination = null;
            }
            else
            {
                var reverseJoint = GetInputSide(joint.Node, joint.Index);
                reverseJoint.Node.Inputs[reverseJoint.Index].Destination = null;
                joint.Node.Outputs[joint.Index].Destination = null;
            }
        }

        private bool AddJoint(NodeJoint from, NodeJoint to)
        {
            GraphNodeLink fromLink;
            GraphNodeLink toLink;
            if (from.IsInput && !to.IsInput)
            {
                fromLink = from.Node.Inputs[from.Index];
                toLink = to.Node.Outputs[to.Index];
            }
            else if (!from.IsInput && to.IsInput)
            {
                fromLink = from.Node.Outputs[from.Index];
                toLink = to.Node.Inputs[to.Index];
            }
            else
            {
                return false;
            }
            if (fromLink.Destination != null || toLink.Destination != null)
            {
                return false;
            }
            fromLink.Destination = to.Node.Id;
            fromLink.Index = to.Index;
            toLink.Destination = from.Node.Id;
            toLink.Index = from.Index;
            return true;
        }

        private Point GetJointPosition(NodeJoint joint)
        {
            var container = _nodes[joint.Node.Id];
            var joints = joint.IsInput ? container.InputsArea : container.OutputsArea;
            return GeomUtils.RectCenter(joints[joint.Index]);
        }

        private GraphNodeLink GetNodeJoint(NodeJoint colllision)
        {
            if (colllision.IsInput)
            {
                return colllision.Node.Inputs[colllision.Index];
            }
            return colllision.Node.Outputs[colllision.Index];
        }
    }
}
