using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;

[CreateAssetMenu]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    public bool unLoad;
    [SerializeField]
    public bool isRandom;
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public LevelData levelData;

    [MenuItem("Data/LevelToCSV")]
    public static void StoryToCSV()
    {
        try
        {
            StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");
            StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "/Config/story.csv", FileMode.OpenOrCreate), Encoding.GetEncoding("UTF-8"));
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
                        sb.Append(eventData.eventTag.ToString() + " = " + eventData.value.ToString());
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

    [MenuItem("Data/LevelCheck")]
    public static void Check()
    {
        LevelSO[] levelSOs = Resources.LoadAll<LevelSO>("LevelData");
        foreach (LevelSO level in levelSOs)
        {
            if (level.levelData.foreWork != null)
            {
                if (!level.levelData.foreWork.Check())
                {
                    Debug.LogErrorFormat("错误的level配置剧情文件，文件名：{0}_foreWork", level.name);
                    level.levelData.foreWork.Init();
                }
            }
            else
            {
                Debug.LogErrorFormat("不存在的level配置剧情文件，文件名：{0}_foreWork", level.name);
                DialogueGraph foreWork = ScriptableObject.CreateInstance<DialogueGraph>();
                string path = string.Format("{0}/GameMain/Resources/DialogData/Level/{1}", Application.dataPath , level.name);
                if(!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(foreWork, "Assets/GameMain/Resources/DialogData/Level/" + level.name + "/" + level.name + "_foreWork.asset");
                level.levelData.foreWork = foreWork;
                foreWork.Init();
            }
            if (level.levelData.afterWork != null)
            {
                if (!level.levelData.afterWork.Check())
                {
                    Debug.LogErrorFormat("错误的level配置剧情文件，文件名：{0}_afterWork", level.name);
                    level.levelData.afterWork.Init();
                }
            }
            else
            {
                Debug.LogErrorFormat("不存在的level配置剧情文件，文件名：{0}_afterWork", level.name);
                DialogueGraph afterWork = ScriptableObject.CreateInstance<DialogueGraph>();
                string path = string.Format("{0}/GameMain/Resources/DialogData/Level/{1}", Application.dataPath, level.name);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(afterWork, "Assets/GameMain/Resources/DialogData/Level/" + level.name + "/" + level.name + "_afterWork.asset");
                level.levelData.afterWork = afterWork;
                afterWork.Init();
            }
            if (level.levelData.failWork != null)
            {
                if (!level.levelData.failWork.Check())
                {
                    Debug.LogErrorFormat("错误的level配置剧情文件，文件名：{0}_failWork", level.name);
                    level.levelData.failWork.Init();
                }
            }
            else
            {
                Debug.LogErrorFormat("不存在的level配置剧情文件，文件名：{0}_failWork", level.name);
                DialogueGraph failWork = ScriptableObject.CreateInstance<DialogueGraph>();
                string path = string.Format("{0}/GameMain/Resources/DialogData/Level/{1}", Application.dataPath, level.name);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(failWork, "Assets/GameMain/Resources/DialogData/Level/" + level.name + "/" + level.name + "_failWork.asset");
                level.levelData.failWork = failWork;
                failWork.Init();
            }
            EditorUtility.SetDirty(level);
            // 如果SetDirty弃用或者未自动保存
            // 请尝试使用AssetDataBase对此level对象保存
        }
    }
}
