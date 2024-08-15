using GameFramework.DataTable;
using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class StorySO : ScriptableObject
{
    [SerializeField]
    public bool unLoad;
    [SerializeField]
    public bool isRemove;
    [SerializeField]
    public OutingSceneState outingSceneState;
    [SerializeField]
    public GameState gameState;
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public DialogueGraph dialogueGraph;
    [SerializeField]
    public List<EventData> eventDatas= new List<EventData>();
    [SerializeField,TextArea]
    public string content;

    //[MenuItem("Data/StoryToCSV")]
    public static void StoryToCSV()
    {
        try
        {
            StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");
            StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "/Config/mainStory.csv", FileMode.OpenOrCreate), Encoding.GetEncoding("UTF-8"));
            sw.WriteLine("故事标签,故事索引,故事时间,触发事件,触发器规则");
            foreach (StorySO story in storySOs)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(story.name + ",");
                sb.Append(story.dialogueGraph.name + ",");
                sb.Append(story.gameState + ",");
                if (story.eventDatas.Count != 0)
                {
                    foreach (EventData eventData in story.eventDatas)
                    {
                        sb.Append(eventData.eventTag.ToString());
                        sb.Append(" ");
                    }
                }
                else
                {
                    sb.Append("NULL");
                }
                sb.Append("," + story.trigger.TriggerToString());
                sw.WriteLine(sb.ToString());
            }
            sw.Close();
            Debug.Log("mainStory输出完毕");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    //[MenuItem("Data/StoryCheck")]
    public static void Check()
    { 
        
    }

    public static StringBuilder TriggerToString(Trigger trigger,StringBuilder sb,int sort)
    {
        if (trigger == null)
            return sb;
        sb.Append('\t',sort);
        if (trigger.not)
        {
            if (trigger.equals)
                sb.Append(trigger.key.ToString() + "不等于" + trigger.value.ToString());
            else
                sb.Append(trigger.key.ToString() + "小于" + trigger.value.ToString());
        }
        else
        {
            if (trigger.equals)
                sb.Append(trigger.key.ToString() + "等于" + trigger.value.ToString());
            else
                sb.Append(trigger.key.ToString() + "大于" + trigger.value.ToString());
        }
        sb.Append('\n');
        if (trigger.GetAndTrigger().Count != 0)
        {
            sb.Append("以下的要求全部满足:\n");
            foreach (Trigger tr in trigger.GetAndTrigger())
            {
                TriggerToString(tr, sb,sort+1);
            }
        }
        if (trigger.GetOrTrigger().Count != 0)
        {
            sb.Append("满足一下任意要求:\n");
            foreach (Trigger tr in trigger.GetAndTrigger())
            {
                TriggerToString(tr, sb, sort + 1);
            }
        }
        return sb;
    }
}
