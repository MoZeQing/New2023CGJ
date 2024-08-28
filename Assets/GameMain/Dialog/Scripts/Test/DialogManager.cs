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
                dialogData = new DialogData(helper, dialogueGraph);
                break;
            case 1:
                helper = new ExcelSerializeHelper();
                FileInfo fileInfo = new FileInfo(dialogExcelPath);
                ExcelPackage package = new ExcelPackage(fileInfo);
                dialogData = new DialogData(helper, package);
                break;
            case 2:
                helper = new CSVSerializeHelper();
                string csvPath = dialogCSVPath.Replace($"{Application.dataPath}/Dialog/Resources/", string.Empty).Replace(".csv", string.Empty);
                TextAsset textAsset = Resources.Load<TextAsset>(csvPath);
                dialogData = new DialogData(helper, textAsset.text);
                break;
        }
        dialogBox.SetDialog(dialogData);
    }
}
