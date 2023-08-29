using System.Reflection;
using UnityEngine;
using XNode;

[CreateAssetMenu(fileName ="DialogueGraph")]
public class DialogueGraph : NodeGraph
{
    public string DialogTag
    {
        get;
        set;
    }

    public Node GetStartNode()
    {
        foreach (Node node in nodes)
        {
            if (node.GetType().ToString() == "StartNode")
            {
                return node;
            }
        }
        return null;
    }
}
