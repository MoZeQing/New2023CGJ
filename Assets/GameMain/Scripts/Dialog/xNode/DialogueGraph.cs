using System.Reflection;
using UnityEngine;
using UnityEditor;
using XNode;

[CreateAssetMenu(fileName ="DialogueGraph")]
public class DialogueGraph : NodeGraph
{
    public string dialogTag;
    [TextArea(5,10)]
    public string dialogInfo;

    public bool Check()
    {
        foreach (Node node in nodes)
        {
            if (node.GetType().ToString() == "StartNode")
            {
                return true;
            }
        }
        return false;
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

#if UNITY_EDITOR
    [MenuItem("Data/Dialog/检查错误")]
    public static void DialogCheck()
    {
        DialogueGraph[] graphs = Resources.LoadAll<DialogueGraph>("DialogData");
        foreach (DialogueGraph graph in graphs)
        {
            if (!graph.Check())
                Debug.LogErrorFormat("不存在StartNode的对话剧情，请检查{0}", graph.name);
        }
    }
#endif
}
