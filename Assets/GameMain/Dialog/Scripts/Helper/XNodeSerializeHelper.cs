using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameMain;
using XNode;
using OfficeOpenXml;
using System.IO;
using System;

namespace Dialog
{
    public class XNodeSerializeHelper : IDialogSerializeHelper
    {
        private DialogueGraph dialogueGraph;
        public DialogData Serialize(object data)
        {
            DialogData dialogData = new DialogData();
            dialogueGraph = data as DialogueGraph;
            StartNode startNode = dialogueGraph.GetStartNode() as StartNode;
            Next(dialogData, startNode);
            return dialogData;
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
                case "BackgroundNode":
                    Next(dialogData, fore, (BackgroundNode)node);
                    break;
            }
        }
        private void Next(DialogData dialogData, StartNode startNode, BaseData fore = null)
        {
            dialogData.DialogDatas.Add(startNode.startData);
            dialogData.DialogName = string.IsNullOrEmpty(startNode.startData.dialogName) ? dialogueGraph.name : startNode.startData.dialogName;
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
                {
                    dialogData.DialogDatas.Add(chatData);
                    newFore = chatData;

                    Node node = NextNode(chatNode, $"chatDatas {mIndex}");
                    if (node != null)
                        Next(dialogData, newFore, node);
                }
                mIndex++;
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
                {
                    dialogData.DialogDatas.Add(optionData);

                    Node node = NextNode(optionNode, $"optionDatas {mIndex}");
                    if (node != null)
                        Next(dialogData, optionData, node);
                }
                mIndex++;
            }
        }
        private void Next(DialogData dialogData, BaseData fore, BackgroundNode backgroundNode)
        {
            BackgroundData backgroundData = backgroundNode.backgroundData;
            if (!backgroundData.Fore.Contains(fore))
                backgroundData.Fore.Add(fore);
            if (!fore.After.Contains(backgroundData))
                fore.After.Add(backgroundData);
            if (!dialogData.DialogDatas.Contains(backgroundData))
                dialogData.DialogDatas.Add(backgroundData);

            Node node = NextNode(backgroundNode, "output");
            if (node != null)
                Next(dialogData, backgroundData, node);
        }
        private Node NextNode(Node node, string nodeName)
        {
            //如果没有中途跳转
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
