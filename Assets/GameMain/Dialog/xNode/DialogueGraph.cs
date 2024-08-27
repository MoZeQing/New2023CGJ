using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using System;
using System.IO;
using OfficeOpenXml;//Epplus
using CharData1 = CharData;//重复命名了，我真是傻逼

namespace GameMain
{
    //[CreateAssetMenu(fileName = "DialogueGraph")]
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

        //public void Init()
        //{
        //    if (this.nodes.Count == 0)
        //    {
        //        StartNode startNode = AddNode(typeof(StartNode)) as StartNode;
        //        ChatNode chatNode = AddNode(typeof(ChatNode)) as ChatNode;
        //        startNode.name = "Start";
        //        chatNode.name = "Chat";
        //        startNode.position = Vector2.zero;
        //        chatNode.position = Vector2.zero;
        //        AssetDatabase.AddObjectToAsset(startNode, this);
        //        AssetDatabase.AddObjectToAsset(chatNode, this);
        //        List<ChatData> chatDatas = new List<ChatData>();
        //        ChatData chatData = new ChatData();
        //        chatData.charName = "测试";
        //        chatData.text = string.Format("测试，来源：{0}", this.name);
        //        chatDatas.Add(chatData);
        //        chatNode.chatDatas = chatDatas;
        //        startNode.GetOutputPort("start").Connect(chatNode.GetInputPort("a"));
        //        AssetDatabase.SaveAssets();
        //    }
        //}

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

        //[MenuItem("导入导出工具/对话文件导出")]
        public static void SOToExcel()
        {
            Debug.Log(0);
        }

        [MenuItem("导入导出工具/对话文件转入")]
        public static void ExcelToSO()
        {
            try
            {
                string path = EditorUtility.OpenFolderPanel("打开对应的文件", "C://", "");//利用一个脚本管理路径
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] fileInfos = root.GetFiles();
                Dictionary<string, CharSO> charPair = new Dictionary<string, CharSO>();
                Dictionary<string, Node> nodePair = new Dictionary<string, Node>();
                foreach (CharSO charSO in Resources.LoadAll<CharSO>("CharData"))
                {
                    charPair.Add(charSO.name, charSO);
                }
                foreach (FileInfo fileInfo in fileInfos)
                {
                    ExcelPackage package = new ExcelPackage(fileInfo);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//剧情文件默认只使用表1
                    string savePath = "Assets/GameMain/Resources/DialogData/" + Path.GetFileNameWithoutExtension(fileInfo.Name) + ".asset";
                    DialogueGraph dialogue = DialogueGraph.CreateInstance<DialogueGraph>();
                    AssetDatabase.CreateAsset(dialogue, savePath);

                    int index = 0;
                    int rowCount = worksheet.Dimension.Rows;

                    List<ChatData> chatDatas = new List<ChatData>();
                    List<OptionData> optionDatas = new List<OptionData>();

                    StartNode startNode = dialogue.AddNode<StartNode>() as StartNode;
                    startNode.name = "Start";
                    AssetDatabase.AddObjectToAsset(startNode, dialogue);
                    /*格式要求
                     * 从第二列开始
                     *  块类型（其中0为的对话，1为选择，2为判断）1
                     *  块序号（第几个块）2
                     *  该选项在当前块中的序号3
                     *  左角色（ID 差分 动效）仅在对话块中有效4 5 6
                     *  中角色（ID 差分 动效）仅在对话块中有效7 8 9
                     *  右角色（ID 差分 动效）仅在对话块中有效10 11 12
                     *  角色名称（实际对话中的名称）13
                     *  文本（在选项中）在判断块中用于输出判断逻辑（不推荐）14
                     *  背景15
                     *  事件（使用|字符进行分割）16
                     *  跳转（当前块的出块，若为空则默认退出对话）17
                     */
                    for (int row = 3; row <= rowCount; row++)//表格从（1，1）开始
                    {
                        if (worksheet.Cells[row, 2].Value.ToString() != index.ToString())//新建的块语句
                        {
                            index++;
                            if (chatDatas.Count != 0)
                            {
                                ChatNode chatNode = dialogue.AddNode<ChatNode>() as ChatNode;
                                chatDatas =chatNode.chatDatas;
                                chatNode.name = "Chat";
                                AssetDatabase.AddObjectToAsset(chatNode, dialogue);
                            }

                            if (optionDatas.Count != 0)
                            {
                                OptionNode optionNode = dialogue.AddNode<OptionNode>() as OptionNode;
                                optionDatas =optionNode.optionDatas;
                                optionNode.name = "Option";
                                AssetDatabase.AddObjectToAsset(optionNode, dialogue);
                            }
                        }

                        //策略化
                        if (worksheet.Cells[row, 1].Value.ToString() == "0")
                        {
                            ChatData chatData = new ChatData();
                            chatData.charName = worksheet.Cells[row, 13].Value.ToString();
                            chatData.text = worksheet.Cells[row, 14].Value.ToString();
                            if (worksheet.Cells[row, 15].Value.ToString() != "0")
                            {
                                chatData.background = Resources.Load<Sprite>("Image/Background/" + worksheet.Cells[row, 18].Value.ToString());
                            }
                            if (worksheet.Cells[row, 4].Value.ToString() != "0")
                            {
                                chatData.left = new CharData1();
                                chatData.left.charSO = charPair[worksheet.Cells[row, 4].Value.ToString()];
                                chatData.left.actionData = new ActionData();
                                chatData.left.actionData.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 5].Value.ToString());
                                chatData.left.actionData.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 6].Value.ToString());
                            }
                            if (worksheet.Cells[row, 7].Value.ToString() != "0")
                            {
                                chatData.middle = new CharData1();
                                chatData.middle.charSO = charPair[worksheet.Cells[row, 7].Value.ToString()];
                                chatData.middle.actionData = new ActionData();
                                chatData.middle.actionData.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 8].Value.ToString());
                                chatData.middle.actionData.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 9].Value.ToString());
                            }
                            if (worksheet.Cells[row, 10].Value.ToString() != "0")
                            {
                                chatData.right = new CharData1();
                                chatData.right.charSO = charPair[worksheet.Cells[row, 10].Value.ToString()];
                                chatData.right.actionData = new ActionData();
                                chatData.right.actionData.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 11].Value.ToString());
                                chatData.right.actionData.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 12].Value.ToString());
                            }
                            chatDatas.Add(chatData);
                        }

                        if (worksheet.Cells[row, 1].Value.ToString() == "1")
                        {
                            OptionData optionData = new OptionData();
                            optionData.text = worksheet.Cells[row, 17].Value.ToString();
                            optionDatas.Add(optionData);
                        }
                        Debug.Log(worksheet.Cells[row, 1].Value.ToString());
                    }
                    //连线
                    for (int row = 3; row <= rowCount; row++)//表格从（1，1）开始
                    {
                        string[] tags = worksheet.Cells[row, 17].Value.ToString().Split('-');
                        foreach (string tag in tags)
                        {

                        }
                    }
                    EditorUtility.SetDirty(dialogue);
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
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

        //[MenuItem("Data/Dialog/检查错误")]
        public static void DialogCheck()
        {
            DialogueGraph[] graphs = Resources.LoadAll<DialogueGraph>("DialogData");
            foreach (DialogueGraph graph in graphs)
            {
                if (!graph.Check())
                    Debug.LogErrorFormat("不存在StartNode的对话剧情，请检查{0}", graph.name);
            }
        }
    }

}