using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using DG.Tweening;

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
        private ActionNode mActionNode = null;
        public CharSO mCharSO = null;
        public Image mImage = null;

        private List<Sprite> mDiffs = new List<Sprite>();//差分
        private ActionState mActionState;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCharacterData = (CharacterData)userData;
            DialogPos= mCharacterData.DialogPos;
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

        }
        public void SetAction(ActionData actionData)
        {
            SetDiff(actionData.diffTag);
            switch (actionData.actionTag)
            {
                case ActionTag.Jump:
                    JumpAction();
                    break;
                case ActionTag.Shake:
                    ShakeAction();
                    break;
            }
        }
        //左右抖动动画
        protected virtual void ShakeAction()
        {
            mImage.gameObject.transform.DOPause();
            mImage.gameObject.transform.localPosition = Vector3.zero;
            mImage.gameObject.transform.DOPunchPosition(Vector3.right*100,0.4f,10);
        }
        //上下跳动动画
        protected virtual void JumpAction()
        {
            mImage.gameObject.transform.DOPause();
            mImage.gameObject.transform.localPosition = Vector3.zero;
            mImage.gameObject.transform.DOLocalJump(Vector3.zero,200,1,0.3f,false);
        }

        public void SetDiff(DiffTag diffTag)
        {
            if (mCharSO == null)
                return;
            if (mCharSO.isMain)
                mImage.sprite = mDiffs[(GameEntry.Utils.closet - 1001) * 18 + (int)diffTag];
            else
                mImage.sprite = mDiffs[(int)diffTag];
        }
        public void SetData(CharSO charSO)
        {
            mCharSO = charSO;
            mDiffs = charSO.diffs;

            mImage.color = Color.gray;
            mImage.DOColor(Color.white, 0.3f);
            mImage.gameObject.transform.localPosition += Vector3.right * 100f;
            mImage.gameObject.transform.DOLocalMoveX(0f, 0.3f);
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
