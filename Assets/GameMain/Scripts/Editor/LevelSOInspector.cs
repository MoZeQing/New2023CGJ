using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSO))]
public class LevelSOInspector : Editor
{
    private SerializedProperty m_Trigger;

    private void OnEnable()
    {
        m_Trigger = serializedObject.FindProperty("trigger");
    }
    public override void OnInspectorGUI()
    {
        LevelSO levelSO = target as LevelSO;
        GUIStyle style1 = new GUIStyle();
        style1.normal.textColor = Color.red;
        style1.fontSize = 20;
        GUIStyle style2 = new GUIStyle();
        style2.normal.textColor = Color.white;
        style2.wordWrap = true;
        style2.fontSize = 16;
        EditorGUILayout.LabelField("在同一级内，和（&）的优先级比或（|）大", style1);
        EditorGUILayout.SelectableLabel($"字符串解析：\n{levelSO.trigger.TriggerToString()}", style2, GUILayout.MaxHeight(80));
        EditorGUILayout.Space(20);
        base.OnInspectorGUI();
    }
}
