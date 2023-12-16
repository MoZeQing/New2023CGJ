using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace GameMain 
{
    [CreateAssetMenu]
    public class CatStateSO : ScriptableObject
    {
        public CatStateData catStateData;

        [MenuItem("Data/CatStateCheck")]
        public static void Check()
        {
            try
            {
                DialogueGraph dialogueGraph = new DialogueGraph();
                CatStateSO[] catStateSOs = Resources.LoadAll<CatStateSO>("CatStateData");
                foreach (CatStateSO catStateSO in catStateSOs)
                {
                    if (!Directory.Exists(Application.dataPath + "/GameMain/Resources/DialogData/Behavior/" + catStateSO.name))
                    {
                        Directory.CreateDirectory(Application.dataPath + "/GameMain/Resources/DialogData/Behavior/" + catStateSO.name);
                    }
                    foreach (BehaviorData behavior in catStateSO.catStateData.behaviors)
                    {
                        for (int i = 0; i < behavior.dialogues.Count; i++)
                        {
                            DialogueGraph dialogue = behavior.dialogues[i];
                            if (dialogue == null)
                            {
                                string assetPath = string.Format("{0}/{1}/{2}_{3}_{4}.asset", "Assets/GameMain/Resources/DialogData/Behavior", catStateSO.name,catStateSO.name, behavior.behaviorTag, i);
                                DialogueGraph graph = DialogueGraph.CreateInstance("DialogueGraph") as DialogueGraph;
                                graph.Init();
                                behavior.dialogues[i] = graph;
                                AssetDatabase.CreateAsset(graph, assetPath);
                            }
                        }
                    }
                }
                Debug.Log("Êä³öÍê±Ï");
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}