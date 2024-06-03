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

        private List<Sprite> mDiffs = new List<Sprite>();//²î·Ö
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
                    break;
                case ActionTag.Shake:
                    break;
            }
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
    //²î·ÖTag
    public enum DiffTag
    {
        AnXiang,//°²Ïê
        CuoE,//´íšG£¬O~O
        no,//²»£¡£¡£¡
        Yun,//ÔÎ
        Chan,//²ö
        OO,//Å¶Å¶
        HaiXiu,//º¦Ðß
        DanXin,//µ£ÐÄ
        QieYi,//ã«Òâ
        O,//Å¶
        Ai,//°¥
        YinAnDeXiao,//Òõ°µµØÐ¦
        XianQi,//ÏÓÆú
        ShengQi,//ÉúÆø
        ZhongJingYa,//ÖÐ¾ªÑÈ
        WeiXiao,//Î¢Ð¦
        XiaoJingYa,//Ð¡¾ªÑÈ
        MoRen,//Ä¬ÈÏ±íÇé
    }
    //¶¯×÷Tag
    public enum ActionTag
    {
        None,//ÎÞ
        Jump,//ÉÏÏÂÌø¶¯
        Shake,//×óÓÒ¶¶¶¯
        Squat
    }
    public enum ActionState
    {
        Idle,//¾²Ö¹×´Ì¬
        Click,//µã»÷×´Ì¬
        Coffee,//¿§·È×´Ì¬
    }
}
