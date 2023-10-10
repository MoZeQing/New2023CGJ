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
    public bool isRemove;
    [SerializeField]
    public OutingSceneState outingSceneState;
    [SerializeField]
    public bool outingBefore;
    [SerializeField]
    public TimeTag timeTag;
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public DialogueGraph dialogueGraph;
    [SerializeField]
    public List<EventData> eventDatas= new List<EventData>();

#if UNITY_EDITOR
    [MenuItem("Data/Story/主故事输出为CSV")]
    public static void MainStoryToCSV()
    {
        try
        {
            //StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");
            //StringBuilder storiesTag = new StringBuilder();
            //StringBuilder triggers = new StringBuilder();
            //StringBuilder dialogues = new StringBuilder();
            //StringBuilder events = new StringBuilder();
            //foreach (StorySO story in storySOs)
            //{
            //    storiesTag.Append(story.name);
            //    storiesTag.Append(",");
            //    triggers.Append("测试");
            //    triggers.Append(",");
            //    dialogues.Append(story.dialogueGraph.name);
            //    dialogues.Append(",");
            //    foreach (EventData eventData in story.eventDatas)
            //    {
            //        events.Append(eventData.eventTag.ToString() + " = " + eventData.value.ToString());
            //        events.Append(",");
            //    }
            //    events.Append(",");
            //}
            //storiesTag.Remove(storiesTag.Length - 1, 1);
            //triggers.Remove(triggers.Length - 1, 1);
            //dialogues.Remove(dialogues.Length - 1, 1);
            //events.Remove(events.Length - 1, 1);
            //StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "/mainStory.csv", FileMode.OpenOrCreate), Encoding.GetEncoding("GB2312"));
            //sw.WriteLine(storiesTag.ToString());
            //sw.WriteLine(triggers.ToString());
            //sw.WriteLine(dialogues.ToString());
            //sw.WriteLine(events.ToString());
            //sw.Close();
            //Debug.Log("mainStory输出完毕");

            StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");
            StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "/mainStory.txt", FileMode.OpenOrCreate), Encoding.GetEncoding("UTF-8"));
            foreach (StorySO story in storySOs)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("StoryTag：" + story.name + ",");
                sb.Append("故事索引：" + story.dialogueGraph.name + ",");
                sb.Append("触发事件：");
                foreach (EventData eventData in story.eventDatas)
                {
                    sb.Append(eventData.eventTag.ToString() + " = " + eventData.value.ToString());
                    sb.Append(" ");
                }
                sw.WriteLine(sb.ToString());
                sw.WriteLine("触发器规则：\n" + TriggerToString(story.trigger, new StringBuilder(), 0));
            }
            sw.Close();
            Debug.Log("mainStory输出完毕");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
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
#endif
}
