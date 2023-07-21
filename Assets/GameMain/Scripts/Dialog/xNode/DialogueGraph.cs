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
}
