using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelComponent))]
public class LevelComponentInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 18;
            style.normal.textColor = Color.white;

            LevelComponent levelComponent = (LevelComponent)target;
            GUILayout.Label($"已经加载的Level数量:{levelComponent.GetLoadedLevelCount}", style);
            GUILayout.Label($"全部的Level数量:{levelComponent.GetAllLevelCount}", style);
        }
    }
}