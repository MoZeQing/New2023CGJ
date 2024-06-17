using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(400)]
public class TriggerNode : Node
{
    [Input] public float a;
    [Output] public float b;

    [SerializeField, Output(dynamicPortList = true)]
    public List<TriggerData> triggerDatas = new List<TriggerData>();
    protected override void Init()
    {
        base.Init();

    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        return null; // Replace this
    }
}


