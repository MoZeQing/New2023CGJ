using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger 
{
    public List<Trigger> OR = new List<Trigger>();
    public List<Trigger> And = new List<Trigger>();

    public TriggerTag key;//什么标签
    public bool not;//是否取反
    public bool equals;
    public string value;//至少的数值
}

[System.Serializable]
public class TriggerData
{
    [SerializeField]
    public Trigger trigger;
    [SerializeField]
    public List<EventData> events = new List<EventData>();
}

[System.Serializable]
public class EventData
{
    public EventTag eventTag;
    public string value;
}
[System.Serializable]
public enum TriggerTag
{
    None,
    Flag,//剧情挂起
    Favor,//当前角色的好感度
    Hope,//希望
    Mood,//心情
    Love,//爱情
    Family,//亲情
    Money,//当前拥有的钱
    Coffee,//是否存在咖啡（一旦有咖啡）
    Day,//目前经过的时间
    Energy,//能量
    MaxEnergy,//最高能量
    Ap,//行动力
    MaxAp,//最大行动力
    Location,
    Index
}
[System.Serializable]
public enum EventTag
{
    Play,//播放剧情（在有链接时默认播放链接剧情，否则播放参数指定的剧情）
    AddFavor,//增加好感度（参数为角色ID）
    AddMoney,//增加金钱（参数为金钱数量）
    AddFlag,//增加旗子
    RemoveFlag//移除旗子
}