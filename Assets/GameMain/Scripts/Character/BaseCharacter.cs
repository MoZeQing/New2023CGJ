using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private SpriteRenderer mSpriteRenderer = null;
        private ActionNode mActionNode = null;

        private List<Sprite> mDiffs = new List<Sprite>();//差分
        private ActionState mActionState;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCharacterData = (CharacterData)userData;
            DialogPos= mCharacterData.DialogPos;

            mSpriteRenderer = this.GetComponent<SpriteRenderer>();
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
            //mSpriteRenderer.sprite = mDiffs[(int)actionData.diffTag];
            switch (actionData.actionTag)
            {
                case ActionTag.Jump:
                    break;
                case ActionTag.Shake:
                    break;
            }
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
        Idle,//静止
        Laugh,//高兴
        Anger,//愤怒
        Lose,//失落
        Cry,//哭泣
        Shy,//害羞
        Amazed,//惊讶
        Scare,//恐惧
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
