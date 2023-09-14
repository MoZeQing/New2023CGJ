using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GameMain;

[NodeWidth(400)]
public class ChatNode : Node
{

    [Input] public float a;

    public string dialogId;

    [SerializeField,Output(dynamicPortList = true)]
    public List<ChatData> chatDatas= new List<ChatData>();

    protected override void Init()
    {
        base.Init();
    }

    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }
}

[Serializable]
public class ChatData
{
    [SerializeField]
    public string charName;
    [SerializeField]
    public CharData left;
    [SerializeField]
    public CharData middle; 
    [SerializeField]
    public CharData right;
    [SerializeField]
    public Sprite background;
    [SerializeField]
    public List<EventData> eventDatas;
    [TextArea,SerializeField]
    public string text;
}
[Serializable]
public class CharData
{
    [SerializeField]
    public CharSO charSO;
    [SerializeField]
    public ActionData actionData;
}

public enum ChatTag
{
    Start,//Æô¶¯×´Ì¬
    Chat,//±ê×¼×´Ì¬
    Option,//Ñ¡ÔñÖ§×´Ì¬
    Trigger,//ÅÐ¶ÏÖ§×´Ì¬
}

public enum SoundTag
{
    None,
    Doubt_S,
    Doubt_M,
    Doubt_L,
    Reluctantly,
    Hesitate,
    Speechless,
    Happy,
    Approve
}