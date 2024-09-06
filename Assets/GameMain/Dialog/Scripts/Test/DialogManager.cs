using Dialog;
using GameMain;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DialogManager : MonoBehaviour
{
    [SerializeField] private string dialogExcelPath;
    [SerializeField] private int dialogTextIndex;
    [SerializeField] private DialogueGraph dialogueGraph;
    [SerializeField] private string excelDataPath;
    [SerializeField] private string dialogCSVPath;
    private void Start()
    {
        DialogBox dialogBox = this.GetComponent<DialogBox>();
        DialogData dialogData = null;
        IDialogSerializeHelper helper = null;
        switch (dialogTextIndex)
        {
            case 0:
                helper = new XNodeSerializeHelper();
                dialogData = helper.Serialize(dialogueGraph);
                break;
            case 1:
                helper = new ExcelSerializeHelper();
                FileInfo fileInfo = new FileInfo(dialogExcelPath);
                ExcelPackage package = new ExcelPackage(fileInfo);
                dialogData = helper.Serialize(package);
                break;
            case 2:
                helper = new CSVSerializeHelper();
                // 将 CSV 文件路径中的 “Assets/GameMain/Dialog/Resources/” 和 “.csv” 去除
                string csvPath = dialogCSVPath.Replace($"{Application.dataPath}/GameMain/Dialog/Resources/", string.Empty).Replace(".csv", string.Empty);
                TextAsset textAsset = Resources.Load<TextAsset>(csvPath);

                if (textAsset == null)
                {
                    Debug.LogError($"Failed to load CSV file at path: {csvPath}. Please ensure the file is located in the Resources folder.");
                }
                else
                {
                    Debug.Log(textAsset.text);
                    dialogData = helper.Serialize(textAsset.text);
                }
                break;
        }
        dialogBox.SetDialog(dialogData);
    }
}
