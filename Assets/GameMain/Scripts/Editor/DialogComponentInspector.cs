using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogComponent))]
public class DialogComponentInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize= 18;
            style.normal.textColor = Color.white;

            DialogComponent dialogComponent= (DialogComponent)target;
            GUILayout.Label($"已经加载的Dialog数量:{dialogComponent.GetLoadedDialogCount}", style);
            GUILayout.Label($"已经加载的Story数量:{dialogComponent.GetLoadedStoryCount}", style);
            GUILayout.Label($"全部的Story数量:{dialogComponent.GetAllStoryCount}", style);
        }
    }
}
