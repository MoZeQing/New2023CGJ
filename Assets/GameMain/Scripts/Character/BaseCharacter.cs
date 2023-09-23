using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;

namespace GameMain
{
    public class BaseCharacter : EntityLogic, IPointerDownHandler
    {
        public DialogPos DialogPos
        {
            get;
            set;
        }

        private CharacterData mCharacterData = null;
        private Image mImage = null;
        private ActionNode mActionNode = null;

        private List<Sprite> mDiffs = new List<Sprite>();//差分
        private ActionState mActionState;

        private void Start()
        {
            mImage = this.GetComponent<Image>();
        }
        private void OnEnable()
        {
            mImage = this.GetComponent<Image>();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCharacterData = (CharacterData)userData;
            DialogPos= mCharacterData.DialogPos;

            mImage = this.GetComponent<Image>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            //mActionState = ActionState.Click;
            //if (mActionNode.click != null)
            //{
            //    List<ChatNode> chatNodes = new List<ChatNode>();
            //    for (int i = 0; i < mActionNode.click.Count; i++)
            //    {
            //        if (GameEntry.Utils.Check(mActionNode.click[i]))
            //        {
            //            if (mActionNode.GetPort(string.Format("click {0}", i)) != null)
            //            {
            //                NodePort nodePort = mActionNode.GetPort(string.Format("click {0}", i));
            //                if (nodePort.Connection != null)
            //                {
            //                    ChatNode node = (ChatNode)nodePort.Connection.node;
            //                    chatNodes.Add(node);
            //                }
            //            }
            //        }
            //    }
            //    if (chatNodes.Count > 0)
            //    {
            //        ChatNode chatNode = chatNodes[Random.Range(0, chatNodes.Count)];
            //    }
            //    else
            //    {
            //        Debug.LogWarningFormat("错误，不存在有效的对话文件，请检查文件以及条件，错误文件：{0}", mCharacterData.ActionGraph.name);
            //    }
            //}
        }
        public void SetAction(ActionData actionData)
        {
            Debug.Log(this.gameObject.name);
            mImage = this.GetComponent<Image>();
            mImage.sprite = mDiffs[(int)actionData.diffTag];
            switch (actionData.actionTag)
            {
                case ActionTag.Jump:
                    break;
                case ActionTag.Shake:
                    break;
            }
        }

        public void SetData(CharSO charSO)
        {
            mDiffs = charSO.diffs;
        }
    }
    [System.Serializable]
    public class ActionData
    {
        public DiffTag diffTag;
        public ActionTag actionTag;
        public SoundTag soundTag;
    }
    //差分Tag
    public enum DiffTag
    {
        AnXiang,//安详
        CuoE,//错欸，O~O
        no,//不！！！
        Yun,//晕
        Chan,//馋
        OO,//哦哦
        HaiXiu,//害羞
        DanXin,//担心
        QieYi,//惬意
        O,//哦
        Ai,//哎
        YinAnDeXiao,//阴暗地笑
        XianQi,//嫌弃
        ShengQi,//生气
        ZhongJingYa,//中惊讶
        WeiXiao,//微笑
        XiaoJingYa,//小惊讶
        MoRen,//默认表情
    }
    //动作Tag
    public enum ActionTag
    {
        None,//无
        Jump,//上下跳动
        Shake,//左右抖动
        Squat
    }
    public enum ActionState
    {
        Idle,//静止状态
        Click,//点击状态
        Coffee,//咖啡状态
    }
}
