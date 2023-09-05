using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(400)]
public class ActionNode : Node {
    public PlayerData ClickData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Click;

    public PlayerData TalkData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Talk;

    public PlayerData TouchData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Touch;

    public PlayerData PlayData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Play;

    public PlayerData SleepData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Sleep;

    public PlayerData MorningData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Morning;
    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}