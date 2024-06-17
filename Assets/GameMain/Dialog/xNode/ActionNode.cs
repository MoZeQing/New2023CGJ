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
    public GameMain.CharData ClickCharData;

    public PlayerData CleanData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Clean;
    public GameMain.CharData CleanCharData;

    public PlayerData PlayData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Play;
    public GameMain.CharData PlayCharData;

    public PlayerData TalkData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Talk;
    public GameMain.CharData TalkCharData;

    public PlayerData BathData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Bath;
    public GameMain.CharData BathCharData;

    public PlayerData TVData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> TV;
    public GameMain.CharData TVCharData;

    public PlayerData StoryData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Story;
    public GameMain.CharData StoryCharData;

    public PlayerData TouchData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Touch;
    public GameMain.CharData TouchCharData;

    public PlayerData RestData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Rest;
    public GameMain.CharData RestCharData;

    public PlayerData SleepData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Sleep;
    public GameMain.CharData SleepCharData;

    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Morning;
    public GameMain.CharData MorningCharData;

    public PlayerData ComfortData;
    [SerializeField, Output(dynamicPortList = true)]
    public List<ParentTrigger> Comfort;
    public GameMain.CharData ComfortCharData;
    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}