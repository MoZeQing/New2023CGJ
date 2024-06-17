using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GameMain;
using System.IO;
using UnityEditor;
using ExcelDataReader;

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

#if UNITY_EDITOR
    [MenuItem("Data/Plot/Excel转换为剧情")]
    public static void ExcelToPlot()
    {
        //string path = EditorUtility.OpenFilePanel("选择配置文件目录", Application.dataPath, " ");
        //string info = new FileInfo(path).Name;//获取当前剧本名字
        //FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        //try
        //{
        //    var excelData = ExcelReaderFactory.CreateOpenXmlReader(stream);
            
        //    System.Data.DataSet sheets = excelData.AsDataSet();

        //    foreach (System.Data.DataTable sheet in sheets.Tables)//对所有的表进行
        //    {
        //        string plotPath = "Assets/Resources/Data/PlotData/" + info;
        //        //寻找当前存在的文件夹，如果不存在则创建
        //        if (!Directory.Exists(plotPath))
        //        {
        //            Directory.CreateDirectory(plotPath);
        //        }

        //        var cols = sheet.Columns.Count;//ExcelFile的表Sheet的列数
        //        var count = sheet.Rows.Count;//Sheet中的行数

        //        for (int i = 0; i < cols; i += 3)
        //        {
        //            if (sheet.Rows[0][i].ToString() == "")//如果内容为空，跳出
        //                break;
        //            PlotScriptableObject plot = ScriptableObject.CreateInstance("PlotScriptableObject") as PlotScriptableObject;
        //            PlotData plotData = new PlotData();
        //            plot.plot = plotData;
        //            plot.plot.plotTag = sheet.Rows[0][i].ToString();
        //            string assetPath = string.Format("{0}/{1}.asset", plotPath, sheet.Rows[0][i].ToString());
        //            for (int j = 2; j < count; j++)
        //            {
        //                if (sheet.Rows[j][i + 1].ToString() == "")//如果内容为空，跳出
        //                    break;

        //                DialogueData dialogue = new DialogueData();
        //                if (sheet.Rows[j][i].ToString() == "")
        //                    dialogue.charData = "char_null";
        //                else
        //                    dialogue.charData = sheet.Rows[j][i].ToString();//通过字符指定角色数据
        //                dialogue.text = sheet.Rows[j][i + 1].ToString();
        //                if (sheet.Rows[j][i + 2].ToString() != "")
        //                {
        //                    string[] events = sheet.Rows[j][i + 2].ToString().Split(' ', '=');//以'和=切分字符串
        //                    for (int k = 0; k < events.Length; k += 2)
        //                    {
        //                        EventData data = new EventData
        //                        {
        //                            eventData = (EventEnum)Enum.Parse(typeof(EventEnum), events[k]),
        //                            eventValue = events[k + 1]
        //                        };
        //                        dialogue.eventDatas.Add(data);
        //                    }
        //                }
        //                plot.plot.dialogues.Add(dialogue);
        //            }
        //            AssetDatabase.CreateAsset(plot, assetPath);
        //        }
        //    }
        //    stream.Close();
        //    Debug.Log("转换完毕");
        //}
        //catch (Exception e)
        //{
        //    stream.Close();
        //}
    }

    [MenuItem("Data/Plot/剧情转换为CSV")]
    public static void PlotToCSV()
    {
        StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");

        //string path = EditorUtility.OpenFilePanel("选择配置文件目录", Application.dataPath, " ");
        //string info = new FileInfo(path).Name;//获取当前剧本名字
        //FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        //try
        //{
        //    var excelData = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //    System.Data.DataSet sheets = excelData.AsDataSet();

        //    foreach (System.Data.DataTable sheet in sheets.Tables)//对所有的表进行
        //    {
        //        string plotPath = "Assets/Resources/Data/PlotData/" + info;
        //        //寻找当前存在的文件夹，如果不存在则创建
        //        if (!Directory.Exists(plotPath))
        //        {
        //            Directory.CreateDirectory(plotPath);
        //        }

        //        var cols = sheet.Columns.Count;//ExcelFile的表Sheet的列数
        //        var count = sheet.Rows.Count;//Sheet中的行数

        //        for (int i = 0; i < cols; i += 3)
        //        {
        //            if (sheet.Rows[0][i].ToString() == "")//如果内容为空，跳出
        //                break;
        //            PlotScriptableObject plot = ScriptableObject.CreateInstance("PlotScriptableObject") as PlotScriptableObject;
        //            PlotData plotData = new PlotData();
        //            plot.plot = plotData;
        //            plot.plot.plotTag = sheet.Rows[0][i].ToString();
        //            string assetPath = string.Format("{0}/{1}.asset", plotPath, sheet.Rows[0][i].ToString());
        //            for (int j = 2; j < count; j++)
        //            {
        //                if (sheet.Rows[j][i + 1].ToString() == "")//如果内容为空，跳出
        //                    break;

        //                DialogueData dialogue = new DialogueData();
        //                if (sheet.Rows[j][i].ToString() == "")
        //                    dialogue.charData = "char_null";
        //                else
        //                    dialogue.charData = sheet.Rows[j][i].ToString();//通过字符指定角色数据
        //                dialogue.text = sheet.Rows[j][i + 1].ToString();
        //                if (sheet.Rows[j][i + 2].ToString() != "")
        //                {
        //                    string[] events = sheet.Rows[j][i + 2].ToString().Split(' ', '=');//以'和=切分字符串
        //                    for (int k = 0; k < events.Length; k += 2)
        //                    {
        //                        EventData data = new EventData
        //                        {
        //                            eventData = (EventEnum)Enum.Parse(typeof(EventEnum), events[k]),
        //                            eventValue = events[k + 1]
        //                        };
        //                        dialogue.eventDatas.Add(data);
        //                    }
        //                }
        //                plot.plot.dialogues.Add(dialogue);
        //            }
        //            AssetDatabase.CreateAsset(plot, assetPath);
        //        }
        //    }
        //    stream.Close();
        //    Debug.Log("转换完毕");
        //}
        //catch (Exception e)
        //{
        //    stream.Close();
        //}
    }
#endif
}

public class StoryCSVData
{
    public string storyName = "storyName";
    public int foreDay=-1;
    public int afterDay=-1;
    public string plotTag="NULL";
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
    Start,//启动状态
    Chat,//标准状态
    Option,//选择支状态
    Trigger,//判断支状态
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