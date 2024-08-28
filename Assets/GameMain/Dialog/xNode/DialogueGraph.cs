using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using System;

namespace GameMain
{
    [CreateAssetMenu(fileName = "DialogueGraph")]
    [System.Serializable]
    public class DialogueGraph : NodeGraph
    {
        [TextArea(5, 10)]
        public string dialogInfo;

        public override Node AddNode(Type type)
        {
            return base.AddNode(type);
        }

        public bool Check()
        {
            foreach (Node node in nodes)
            {
                if (node.GetType().ToString() == "StartNode")
                {
                    return true;
                }
            }
            return false;
        }

        public Node GetStartNode()
        {
            foreach (Node node in nodes)
            {
                if (node.GetType().ToString() == "StartNode")
                {
                    return node;
                }
            }
            return null;
        }

        public static string GetOutPortName(int node, int index)
        {
            switch (node)
            {
                case 0:
                    return string.Format("chatDatas {0}", index);
                case 1:
                    return string.Format("optionDatas {0}", index);
                case 2:
                    return string.Format("triggerDatas {0}", index);
            }
            return string.Empty;
        }
    }

}