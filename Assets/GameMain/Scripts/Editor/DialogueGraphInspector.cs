using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameMain;
using Dialog;

[CustomEditor(typeof(DialogueGraph))]
public class DialogueGraphInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("转出为文本数据"))
        {
            DialogueGraph dialogueGraph = serializedObject.targetObject as DialogueGraph;
            string path = EditorUtility.OpenFolderPanel("输出目录", "C://", "");
            string fileName = dialogueGraph.name;
            XNodeSerializeHelper helper1 = new XNodeSerializeHelper();
            CSVSerializeHelper helper2 = new CSVSerializeHelper();
            DialogData dialogData = helper1.Serialize(dialogueGraph);
            helper2.Deserialize(dialogData, path, fileName);
        }
    }
}