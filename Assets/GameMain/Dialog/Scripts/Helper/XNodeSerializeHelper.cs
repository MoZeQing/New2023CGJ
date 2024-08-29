using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameMain;
using XNode;
using UnityEditor.SceneManagement;
using OfficeOpenXml;
using System.IO;
using System;

namespace Dialog
{
    public class XNodeSerializeHelper : IDialogSerializeHelper
    {
        public void Serialize(DialogData dialogData, object data)
        {
            DialogueGraph dialogueGraph = data as DialogueGraph;
            StartNode startNode = dialogueGraph.GetStartNode() as StartNode;
            Next(dialogData, startNode);
        }

        public void Next(DialogData dialogData, BaseData fore, Node node)
        {
            if (node == null)
                return;
            string typeTag = node.GetType().Name;
            switch (typeTag)
            {
                case "StartNode":
                    Next(dialogData, fore, (StartNode)node);
                    break;
                case "ChatNode":
                    Next(dialogData, fore, (ChatNode)node);
                    break;
                case "OptionNode":
                    Next(dialogData, fore, (OptionNode)node);
                    break;
            }
        }
        private void Next(DialogData dialogData, StartNode startNode, BaseData fore = null)
        {
            dialogData.DialogDatas.Add(startNode.startData);
            Node node = NextNode(startNode, "start");
            if (node != null)
                Next(dialogData, startNode.startData, node);
        }
        private void Next(DialogData dialogData, BaseData fore, ChatNode chatNode)
        {
            BaseData newFore = fore;
            int mIndex = 0;
            while (mIndex < chatNode.chatDatas.Count)
            {
                ChatData chatData = chatNode.chatDatas[mIndex];
                if (!chatData.Fore.Contains(newFore))
                    chatData.Fore.Add(newFore);
                if (!newFore.After.Contains(chatData))
                    newFore.After.Add(chatData);
                if (!dialogData.DialogDatas.Contains(chatData))
                    dialogData.DialogDatas.Add(chatData);
                newFore = chatData;
                mIndex++;

                Node node = NextNode(chatNode, $"chatDatas {mIndex - 1}");
                if (node != null)
                    Next(dialogData, newFore, node);
            }
        }
        private void Next(DialogData dialogData, BaseData fore, OptionNode optionNode)
        {
            int mIndex = 0;
            while (mIndex < optionNode.optionDatas.Count)
            {
                OptionData optionData = optionNode.optionDatas[mIndex];
                if (!optionData.Fore.Contains(fore))
                    optionData.Fore.Add(fore);
                if (!fore.After.Contains(optionData))
                    fore.After.Add(optionData);
                if (!dialogData.DialogDatas.Contains(optionData))
                    dialogData.DialogDatas.Add(optionData);
                mIndex++;

                Node node = NextNode(optionNode, $"optionDatas {mIndex - 1}");
                if (node != null)
                    Next(dialogData, optionData, node);
            }
        }
        private Node NextNode(Node node, string nodeName)
        {
            //���û����;��ת
            if (node.GetPort(nodeName) != null)
            {
                NodePort nodePort = node.GetPort(nodeName);
                if (nodePort.Connection != null)
                {
                    Node nextNode = nodePort.Connection.node;
                    return nextNode;
                }
            }
            return null;
        }
    }

}