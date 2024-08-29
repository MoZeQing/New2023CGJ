using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StorySO))]
public class StorySOInspector : Editor
{
    private SerializedProperty m_Trigger;

    private void OnEnable()
    {
        m_Trigger = serializedObject.FindProperty("trigger");
    }
    public override void OnInspectorGUI()
    {
        StorySO storySO = target as StorySO;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.wordWrap = true;
        style.fontSize=18;

        EditorGUILayout.SelectableLabel($"×Ö·û´®½âÎö£º\n{storySO.trigger.TriggerToString()}", style, GUILayout.MaxHeight(80));
        base.OnInspectorGUI();
    }
}
