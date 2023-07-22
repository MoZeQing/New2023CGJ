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
        private CharacterData mCharacterData = null;
        private SpriteRenderer mSpriteRenderer = null;
        private ActionNode mActionNode = null;

        private List<Sprite> mDiffs = new List<Sprite>();//差分
        private ActionState mActionState;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCharacterData = (CharacterData)userData;

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
            mActionState = ActionState.Click;
            if (mActionNode.click != null)
            {
                List<ChatNode> chatNodes = new List<ChatNode>();
                for (int i = 0; i < mActionNode.click.Count; i++)
                {
                    if (GameEntry.Utils.Check(mActionNode.click[i]))
                    {
                        if (mActionNode.GetPort(string.Format("click {0}", i)) != null)
                        {
                            NodePort nodePort = mActionNode.GetPort(string.Format("click {0}", i));
                            if (nodePort.Connection != null)
                            {
                                ChatNode node = (ChatNode)nodePort.Connection.node;
                                chatNodes.Add(node);
                            }
                        }
                    }
                }
                if (chatNodes.Count > 0)
                {
                    ChatNode chatNode = chatNodes[Random.Range(0, chatNodes.Count)];
                }
                else
                {
                    Debug.LogWarningFormat("错误，不存在有效的对话文件，请检查文件以及条件，错误文件：{0}", mCharacterData.ActionGraph.name);
                }
            }
        }
        public void SetChar(CharData charData)
        {
            mSpriteRenderer.sprite = mDiffs[(int)charData.DiffTag];
            Action(charData.ActionTag);
        }

        private void Action(ActionTag action)
        {
            switch (action)
            {
                case ActionTag.Jump:
                    break;
                case ActionTag.Shake:
                    break;
            }
        }
    }
    public class CharData
    {
        public DiffTag DiffTag
        {
            get;
            set;
        }

        public ActionTag ActionTag
        {
            get;
            set;
        }
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
        Amazed//惊讶
    }
    //动作Tag
    public enum ActionTag
    {
        Jump,//上下跳动
        Shake//左右抖动
    }
    public enum ActionState
    {
        Idle,//静止状态
        Click,//点击状态
        Coffee,//咖啡状态
    }
}
