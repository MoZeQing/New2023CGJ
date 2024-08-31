using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSO))]
public class LevelSOInspector : Editor
{
    private SerializedProperty m_Trigger;
    private SerializedProperty m_Text;
    private string text;

    private void OnEnable()
    {
        m_Trigger = serializedObject.FindProperty("trigger");
        m_Text= serializedObject.FindProperty("text");
    }
    public override void OnInspectorGUI()
    {
        LevelSO levelSO = target as LevelSO;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.wordWrap = true;
        style.fontSize = 16;
        EditorGUILayout.SelectableLabel($"×Ö·û´®½âÎö£º\n{levelSO.trigger.TriggerToString()}", style, GUILayout.MaxHeight(80));
        EditorGUILayout.Space(20);
        if (GUILayout.Button(EditorGUIUtility.TrTextContent("×ª»»²¥·Å", string.Empty, "PlayButton"), GUILayout.Height(20)))
        {
            Debug.Log(m_Text.stringValue);
            levelSO.trigger = new ParentTrigger(m_Text.stringValue);
        }
        base.OnInspectorGUI();
    }
}
