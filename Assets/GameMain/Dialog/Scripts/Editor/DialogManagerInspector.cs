using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(DialogManager))]
public class DialogManagerInspector : Editor
{
    private SerializedProperty m_DialogueGraph;
    private SerializedProperty m_DialogTextIndex;
    private SerializedProperty m_DialogExcelPath;
    private SerializedProperty m_DialogCSVPath;
    public const string DefaultScenePath = "Assets/Dialog/DailogTest.unity";

    private DialogueGraph dialogueGraph=null;

    private void OnEnable()
    {
        m_DialogueGraph = serializedObject.FindProperty("dialogueGraph");
        m_DialogTextIndex = serializedObject.FindProperty("dialogTextIndex");
        m_DialogExcelPath= serializedObject.FindProperty("dialogExcelPath");
        m_DialogCSVPath = serializedObject.FindProperty("dialogCSVPath");
    }
    public override void OnInspectorGUI()
    {
        DialogManager dialogManager= (DialogManager)target;
        serializedObject.Update();
        m_DialogTextIndex.intValue = EditorGUILayout.Popup("测试模式", m_DialogTextIndex.intValue, new[] { "XNode", "Excel","CSV" });
        switch (m_DialogTextIndex.intValue)
        {
            case 0:
                EditorGUILayout.PropertyField(m_DialogueGraph);
                if (m_DialogueGraph == null)
                {
                    EditorGUILayout.HelpBox("错误，请引用一个有效的Node", MessageType.Error);
                }
                else
                {
                    if (GUILayout.Button(EditorGUIUtility.TrTextContent("测试播放", string.Empty, "PlayButton"), GUILayout.Height(20)))
                    {
                        DialogOpenOrPlay("Assets/Dialog/DialogTest.unity");
                    }
                }
                break;
            case 1://Excel模式
                GUILayout.Label(m_DialogExcelPath.stringValue);
                if (GUILayout.Button(EditorGUIUtility.TrTextContent("索引Excel", string.Empty), GUILayout.Height(20)))
                {
                    m_DialogExcelPath.stringValue = EditorUtility.OpenFilePanel("打开对应的文件", "C://", "");//利用一个脚本管理路径
                }
                if (m_DialogExcelPath.stringValue != string.Empty)
                {
                    if (GUILayout.Button(EditorGUIUtility.TrTextContent("测试播放", string.Empty, "PlayButton"), GUILayout.Height(20)))
                    {
                        DialogOpenOrPlay("Assets/Dialog/DialogTest.unity");
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("错误，请索引一个有效的Excel", MessageType.Error);
                }
                break;
            case 2:
                GUILayout.Label(m_DialogCSVPath.stringValue);
                if (GUILayout.Button(EditorGUIUtility.TrTextContent("索引CSV", string.Empty), GUILayout.Height(20)))
                {
                    m_DialogCSVPath.stringValue = EditorUtility.OpenFilePanel("打开对应的文件", "C://", "");//利用一个脚本管理路径
                }
                if (m_DialogCSVPath.stringValue != string.Empty)
                {
                    if (GUILayout.Button(EditorGUIUtility.TrTextContent("测试播放", string.Empty, "PlayButton"), GUILayout.Height(20)))
                    {
                        DialogOpenOrPlay("Assets/Dialog/DialogTest.unity");
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("错误，请索引一个有效的CSV", MessageType.Error);
                }
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }

    public static void DialogOpenOrPlay(string path)
    {
        string sceneName = Path.GetFileNameWithoutExtension(path);
        bool isCurScene = EditorSceneManager.GetActiveScene().name.Equals(sceneName);
        //通过一个中继的So来保存数据
        if (!Application.isPlaying)
        {
            if (isCurScene)
            {
                Debug.Log($"运行场景：{sceneName}");
                EditorApplication.ExecuteMenuItem("Edit/Play");
            }
            else
            {
                Debug.Log($"打开场景：{sceneName}");
                EditorSceneManager.OpenScene(path);
            }
        }
        else
        {
            if (isCurScene)
            {
                Debug.Log($"退出场景：{sceneName}");
                EditorApplication.ExecuteMenuItem("Edit/Play");
            }
        }
    }
}
