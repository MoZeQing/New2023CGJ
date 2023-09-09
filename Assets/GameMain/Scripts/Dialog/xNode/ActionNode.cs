using GameMain;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XNode;

[NodeWidth(400)]
public class ActionNode : Node {
    public PlayerData ClickData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Click;

    public PlayerData CleanData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Clean;

    public PlayerData PlayData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Play;

    public PlayerData TalkData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Talk;

    public PlayerData BathData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Bath;

    public PlayerData TVData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> TV;

    public PlayerData StoryData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Story;

    public PlayerData TouchData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Touch;

    public PlayerData RestData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Rest;

    public PlayerData SleepData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Sleep;

    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Morning;

    public PlayerData ComfortData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Comfort;
    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}