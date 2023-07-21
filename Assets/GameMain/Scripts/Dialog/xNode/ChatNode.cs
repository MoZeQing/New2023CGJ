using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(400)]
public class ChatNode : Node
{

    [Input] public float a;
    [Output] public float b;

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
    public Sprite charSprite;
    [TextArea,SerializeField]
    public string text;
}

public enum ChatTag
{
    Start,//Æô¶¯×´Ì¬
    Chat,//±ê×¼×´Ì¬
    Option,//Ñ¡ÔñÖ§×´Ì¬
}