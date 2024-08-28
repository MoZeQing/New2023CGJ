using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GameMain;
using System.IO;
using UnityEditor;
using ExcelDataReader;
using Dialog;

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

public enum ChatTag
{
    Start,//Æô¶¯×´Ì¬
    Chat,//±ê×¼×´Ì¬
    Option,//Ñ¡ÔñÖ§×´Ì¬
}
