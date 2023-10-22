using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger 
{
    public TriggerTag key;//什么标签
    public bool not;//是否取反
    public bool equals;
    public string value;//至少的数值
    public virtual List<Trigger> GetAndTrigger()
    {
        return null;
    }

    public virtual List<Trigger> GetOrTrigger()
    {
        return null;
    }
}
[System.Serializable]
public class ParentTrigger : Trigger
{
    public List<FirstTrigger> and = new List<FirstTrigger>();
    public List<FirstTrigger> or = new List<FirstTrigger>();

    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FirstTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FirstTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class FirstTrigger : Trigger
{
    public List<SceondTrigger> and = new List<SceondTrigger>();
    public List<SceondTrigger> or=new List<SceondTrigger>();
    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (SceondTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (SceondTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class SceondTrigger : Trigger
{
    public List<ThirdTrigger> and = new List<ThirdTrigger>();
    public List<ThirdTrigger> or = new List<ThirdTrigger>();
    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (ThirdTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (ThirdTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class ThirdTrigger : Trigger
{
    public List<FourthTrigger> and = new List<FourthTrigger>();
    public List<FourthTrigger> or = new List<FourthTrigger>();
    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FourthTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FourthTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class FourthTrigger : Trigger
{
    public override List<Trigger> GetAndTrigger()
    {
        return new List<Trigger>();
    }

    public override List<Trigger> GetOrTrigger()
    {
        return new List<Trigger>();
    }
}

[System.Serializable]
public class TriggerData
{
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public List<EventData> events = new List<EventData>();
}

[System.Serializable]
public class EventData
{
    public EventTag eventTag;
    public string value;

    public EventData() { }
    public EventData(EventTag eventTag)
    { 
        this.eventTag= eventTag;
    }
    public EventData(EventTag eventTag, string value)
    {
        this.eventTag= eventTag;   
        this.value= value;
    }
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
    Ability,
    Money,//当前拥有的钱
    Coffee,//是否存在咖啡（一旦有咖啡）
    Day,//目前经过的时间
    Energy,//能量
    MaxEnergy,//最高能量
    Ap,//行动力
    MaxAp,//最大行动力
    Location,//位置
    TimeTag,//时间标签
    Week,//星期
    BehaviorTag,
    Index,
    Rent,//设置租金
}
[System.Serializable]
public enum EventTag
{
    Play,//播放剧情（在有链接时默认播放链接剧情，否则播放参数指定的剧情）
    AddFavor,//增加好感度（参数为角色ID）
    AddLove,
    AddHope,
    AddFamily,
    AddMood,
    AddAbility,
    AddEnergy,
    AddMaxEnergy,
    AddAp,
    AddMoney,//增加金钱（参数为金钱数量）
    AddItem,//增加道具
    AddFlag,//增加旗子
    RemoveFlag,//移除旗子
    NextDay,//跳转至下一天
    PlayBgm,//播放bgm
    EndGame,//结束游戏
    AddDay,//增加时间
    AddAction,//增加Action
    Rent,//设置租金
}