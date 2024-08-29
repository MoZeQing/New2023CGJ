using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class Trigger 
{
    public TriggerTag key;//什么标签
    public bool not;//是否取反
    public bool equals;
    public string value;//至少的数值
    public virtual List<Trigger> GetTriggers()
    {
        return null;
    }

    public Trigger() { }
    public Trigger(string triggerText)
    {
        string[] triggers = triggerText.Split('[', ']' );
        foreach (string trigger in triggers)
        { 
            
        }
    }

    public string TriggerToString()
    {
        return TriggerToString(this, new StringBuilder()).ToString();
    }

    public static StringBuilder TriggerToString(Trigger trigger, StringBuilder sb)
    {
        if (trigger == null)
            return sb;
        if (trigger.key == TriggerTag.Or)
        {
            if (trigger.GetTriggers().Count != 0)
            {
                sb.Append("(|");
                List<Trigger> orTriggers = trigger.GetTriggers();
                for (int i = 0; i < orTriggers.Count; i++)
                {
                    TriggerToString(orTriggers[i], sb);
                }
                sb.Append(")");
            }
        }
        else
        {
            if (trigger.GetTriggers().Count != 0)
                sb.Append("(&");
            if (trigger.not)
            {
                if (trigger.equals)
                    sb.Append($"[{trigger.key}!={trigger.value}]");
                else
                    sb.Append($"[{trigger.key}<{trigger.value}]");
            }
            else
            {
                if (trigger.equals)
                    sb.Append($"[{trigger.key}={trigger.value}]");
                else
                    sb.Append($"[{trigger.key}>{trigger.value}]");
            }
            if (trigger.GetTriggers().Count != 0)
            {
                List<Trigger> andTriggers = trigger.GetTriggers();
                for (int i = 0; i < andTriggers.Count; i++)
                {
                    TriggerToString(andTriggers[i], sb);
                }
                sb.Append(")");
            }
        }
        return sb;
    }
}
[System.Serializable]
public class ParentTrigger : Trigger
{
    public List<FirstTrigger> and = new List<FirstTrigger>();

    public override List<Trigger> GetTriggers()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FirstTrigger firstTrigger in and)
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
    public override List<Trigger> GetTriggers()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (SceondTrigger firstTrigger in and)
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
    public override List<Trigger> GetTriggers()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (ThirdTrigger firstTrigger in and)
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
    public override List<Trigger> GetTriggers()
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
    public string[] values;

    public EventData() { }
    public EventData(EventTag eventTag)
    { 
        this.eventTag= eventTag;
    }
    public override string ToString()
    { 
        string ans=eventTag.ToString();
        for(int i=0;i<values.Length;i++)
            ans += $" {values[i]}";
        return ans;
    }
}
[System.Serializable]
public enum TriggerTag
{
    None,
    Or,
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
    //友情
    FDog,//狗狗
    FRegular,//露希尔
    FWitch,//老登
    FFiction,//小说家
    FMoney,//催债人
    FCourier,//外卖员
    FlagCount,

    Wisdom,//智慧
    Stamina,//体能
    Charm//魅力
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
    AddRecipe,//增加配方
    AddFriend,
    AddScene,//增加场景
    Test,
    Jump,
    ShowFlag,//展示所有的Flag数据
    AddBuff,//增加buff
    RemoveBuff,//卸载buff
    Weather,//天气
    SetClothing,//设置服装
    ShowForm,//显示窗体
    AddCharm,
    AddStamina,
}