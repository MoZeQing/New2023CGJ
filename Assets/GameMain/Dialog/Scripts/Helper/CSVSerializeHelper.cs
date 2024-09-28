using Dialog;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSerializeHelper : IDialogSerializeHelper
{
    public void Deserialize(DialogData dialogData,string path,string fileName)
    {
        try
        {
            string savePath = $"{path}/{fileName}.csv";
            StreamWriter sw = new StreamWriter(savePath);
            foreach (BaseData baseData in dialogData.DialogDatas)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(baseData.Id).Append(',');
                switch (baseData.GetType().ToString())
                {
                    case "Dialog.ChatData":
                        ChatData chatData = baseData as ChatData;
                        sb.Append("0").Append(',');
                        sb.Append(chatData.charName).Append(',');
                        sb.Append(chatData.text).Append(',');
                        break;
                    case "Dialog.OptionData":
                        OptionData optionData = baseData as OptionData;
                        sb.Append("1").Append(',').Append(',');
                        sb.Append(optionData.text).Append(',');
                        break;
                }
                sw.WriteLine(sb.ToString());
            }
            sw.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    public DialogData Serialize( object data)
    {
        DialogData dialogData=new DialogData();
        Dictionary<string, BaseData> mapsDialogData = new Dictionary<string, BaseData>();
        string dialogText = data.ToString();
        string[] dialogRows = dialogText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] startDialogs = dialogRows[2].Split(',');
        StartData startData = new StartData()
        { 
            dialogName= startDialogs[4]
        };
        dialogData.DialogName = startData.dialogName;
        dialogData.DialogDatas.Add(startData);

        for (int i = 3; i < dialogRows.Length ; i++) // 表格从（1，1）开始
        {
            string[] dialogs = dialogRows[i].Split(',');
            if (dialogs[0] == "#")
                continue;

            try
            {
                BaseData baseData = dialogs[1] switch
                {
                    "0" => ChatSerialize(dialogs),
                    "1" => OptionSerialize(dialogs),
                    "2" => BackgroundSerialize(dialogs),
                    "3" => BlackSerialize(dialogs),
                    "4" => VoiceSerialize(dialogs),
                    "5" => CGSerialize(dialogs),
                    _ => throw new CSVParseException(i, $"{dialogData.DialogName} Unknown type '{dialogs[1]}'")
                };

                string identifier = dialogs[2];
                baseData.Id = int.Parse(identifier); // 设置标识符到 BaseData
                mapsDialogData.Add(identifier, baseData); // 使用标识符作为键
                dialogData.DialogDatas.Add(baseData);
            }
            catch (Exception ex) when (ex is FormatException || ex is ArgumentException)
            {
                // 这里使用 CSVParseException 并将当前行号传递进去
                throw new CSVParseException(i + 1, $"{dialogData.DialogName} Error processing row {i + 1}: {ex.Message}");
            }
        }

        // 连接数据的逻辑保持不变
        BaseData fore = startData;
        for (int i = 3; i < dialogRows.Length ; i++)
        {
            string[] dialogs = dialogRows[i].Split(',');
            Debug.Log($"Processing row {i}: {string.Join(",", dialogs)}");
            if (dialogs[0] == "#")
                continue;

            string chatTag = dialogs[2];
            if (!mapsDialogData.TryGetValue(chatTag, out var baseData))
            {
                continue; // 处理未找到的情况
            }
            // 如果当前节点没有 Fore 并且前一个节点是普通对话，则连接
            if (baseData.Fore.Count == 0)
            {
                // 将自己与上一个普通对话节点连接
                if (!fore.After.Contains(baseData))
                    fore.After.Add(baseData);
                if (!baseData.Fore.Contains(fore))
                    baseData.Fore.Add(fore);
            }

            string[] tags = dialogs[15].Split('-');
            foreach (string tag in tags)
            {
                Debug.Log($"Current tag processing: {tag}"); // 输出当前处理的标签

                if (tag == "0") // 忽略 '0' 标签，但也记录它
                {
                    Debug.Log("Skipping tag '0'");
                    continue;
                }

                if (mapsDialogData.TryGetValue(tag, out var nextBaseData))
                {
                    Debug.Log($"Found next base data for tag {tag} with ID {nextBaseData.Id}"); // 找到标签对应的基础数据

                    if (!nextBaseData.Fore.Contains(baseData))
                    {
                        nextBaseData.Fore.Add(baseData); // 添加到前置节点
                        Debug.Log($"Added to fore of {tag}");
                    }
                    if (!baseData.After.Contains(nextBaseData))
                    {
                        baseData.After.Add(nextBaseData); // 添加到后置节点
                        Debug.Log($"Added to after of {tag}");
                    }
                }
                else
                {
                    Debug.Log($"No base data found for tag {tag}"); // 未找到对应的基础数据
                }
            }

            fore = baseData;
        }
        return dialogData;
    }

    private BaseData CGSerialize(string[] csvString)
    {
        return new CGData
        {
            cgTag = (CGTag)int.Parse(csvString[10]),
            parmOne = int.Parse(csvString[11]),
            parmTwo = int.Parse(csvString[12]),
            parmThree = csvString[13],
            cgSpr = Resources.Load<Sprite>("Dialog/CG/" + csvString[14])
        };
    }

    private BaseData VoiceSerialize(string[] csvString)
    {
        return new VoiceData
        {
            voice = csvString[14]
        };
    }

    private BaseData BackgroundSerialize(string[] csvString)
    {
        return new BackgroundData
        {
            backgroundTag = (BackgroundTag)int.Parse(csvString[10]),
            parmOne = int.Parse(csvString[11]),
            parmTwo = int.Parse(csvString[12]),
            parmThree = csvString[13],
            backgroundSpr = Resources.Load<Sprite>("Dialog/Background/" + csvString[14])
        };
    }

    public BaseData ChatSerialize(string[] csvString)
    {
        ChatData chatData = new ChatData();
        chatData.chatPos = (DialogPos)int.Parse(csvString[12]);
        chatData.charName = csvString[13];
        chatData.text = csvString[14];
        //chatData.eventData = string.IsNullOrEmpty(csvString[15]) ? null : csvString[15].Split('|');
        chatData.leftAction = new ActionData();
        if (csvString[3] != string.Empty)
        {
            chatData.leftAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[3]}");
            chatData.leftAction.diffTag = (DiffTag)int.Parse(csvString[4]);
            chatData.leftAction.actionTag = (ActionTag)int.Parse(csvString[5]);
        }
        chatData.middleAction = new ActionData();
        if (csvString[6] != string.Empty)
        {
            chatData.middleAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[6]}");
            chatData.middleAction.diffTag = (DiffTag)int.Parse(csvString[7]);
            chatData.middleAction.actionTag = (ActionTag)int.Parse(csvString[8]);
        }

        chatData.rightAction = new ActionData();
        if (csvString[9] != string.Empty)
        {
            chatData.rightAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[9]}");
            chatData.rightAction.diffTag = (DiffTag)int.Parse(csvString[10]);
            chatData.rightAction.actionTag = (ActionTag)int.Parse(csvString[11]);
        }
        return chatData;
    }
    public BaseData BlackSerialize(string[] csvString)
    {
        BlackData blackData = new BlackData();
        blackData.text = csvString[14];
        return blackData;
    }
    public BaseData OptionSerialize(string[] csvString)
    {
        return new OptionData
        {
            trigger = new ParentTrigger(csvString[13]),
            eventData = string.IsNullOrEmpty(csvString[9]) ? null : csvString[15].Split('|'),
            text = csvString[14] 
        };
    }
}

/// <summary>
/// 自定义异常类，用于 CSV 解析中的错误定位
/// </summary>
/// <summary>
/// 自定义异常类，用于 CSV 解析错误
/// </summary>
public class CSVParseException : Exception
{
    public int Row { get; }

    // 确保只存在一个构造函数
    public CSVParseException(int row, string message) : base(message)
    {
        Row = row;
    }
}

