using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DialogStage : Entity
    {
        [SerializeField] private SpriteRenderer mBG;
        [SerializeField] private Transform mLeft;
        [SerializeField] private Transform mRight;
        [SerializeField] private DialogForm mDialogForm = null;

        private BaseCharacter mLeftChar = null;
        private BaseCharacter mRightChar = null;
        private BaseCharacter mMiddleChar = null;

        private Dictionary<CharSO, BaseCharacter> mCharChace = new Dictionary<CharSO, BaseCharacter>();
        private DialogStageData mDialogStageData = null;

        //缓存区
        private Dictionary<int,ChatData> mCharIdChace =new Dictionary<int,ChatData>();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mDialogStageData=(DialogStageData)userData;
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, LoadCharacterSuccess);

            this.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            mBG = this.transform.Find("BGImage").GetComponent<SpriteRenderer>();
            mLeft = this.transform.Find("Left").GetComponent<Transform>();
            mRight = this.transform.Find("Right").GetComponent<Transform>();
            mDialogForm = this.transform.Find("DialogForm").GetComponent<DialogForm>();

            mDialogForm.SetDialog(mDialogStageData.DialogueGraph, this);
        }

        //private void OnEnable()
        //{
        //    GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, LoadCharacterSuccess);

        //    this.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        //    mBG = this.transform.Find("BGImage").GetComponent<SpriteRenderer>();
        //    mLeft = this.transform.Find("Left").GetComponent<Transform>();
        //    mRight = this.transform.Find("Right").GetComponent<Transform>();
        //}

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, LoadCharacterSuccess);

            foreach (KeyValuePair<CharSO, BaseCharacter> pair in mCharChace)
            {
                GameEntry.Entity.HideEntity(pair.Value.Entity);
            }
            mCharChace.Clear();
        }

        //private void OnDisable()
        //{
        //    GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, LoadCharacterSuccess);
        //}
        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="baseCharacter"></param>
        /// <param name="dialogPos"></param>
        private void SetDialogPos(BaseCharacter baseCharacter, DialogPos dialogPos)
        {
            baseCharacter.DialogPos = dialogPos;
            switch (dialogPos)
            {
                case DialogPos.Left:
                    if (mLeftChar == baseCharacter)
                        return;
                    if (mRightChar == baseCharacter)
                        mRightChar = null;
                    if (mMiddleChar == baseCharacter)
                        mMiddleChar = null;
                    if (mLeftChar != null)
                        mLeftChar.gameObject.SetActive(false);
                    mLeftChar = baseCharacter;
                    mLeftChar.gameObject.SetActive(true);
                    mLeftChar.transform.position = mLeft.position;
                    break;
                case DialogPos.Right:
                    if (mLeftChar == baseCharacter)
                        mLeftChar = null; 
                    if (mRightChar == baseCharacter)
                        return;
                    if (mMiddleChar == baseCharacter)
                        mMiddleChar = null;
                    mRightChar = baseCharacter;
                    mRightChar.gameObject.SetActive(true);
                    mRightChar.transform.position = mRight.position;
                    break;
                case DialogPos.Middle:
                    if (mLeftChar == baseCharacter)
                        mLeftChar = null;
                    if (mRightChar == baseCharacter)
                        mRightChar = null;
                    if (mMiddleChar == baseCharacter)
                        return;
                    mMiddleChar = baseCharacter;
                    mMiddleChar.gameObject.SetActive(true);
                    mMiddleChar.transform.position = mRight.position;
                    break;
            }
        }
        /// <summary>
        /// 设置动画
        /// </summary>
        /// <param name="baseCharacter"></param>
        /// <param name="actionData"></param>
        private void SetDialogAction(BaseCharacter baseCharacter, ActionData actionData)
        { 
            
        }

        public void ShowCharacter(ChatData chatData)
        {
            CharSO charSO = chatData.charSO;
            if (charSO == null) return;
            if (mCharChace.ContainsKey(charSO))
            {
                SetDialogPos(mCharChace[charSO],chatData.dialogPos);
                SetDialogAction(mCharChace[charSO], chatData.actionData);
            }
            else
            {
                int entityId = GameEntry.Entity.GenerateSerialId();
                GameEntry.Entity.ShowCharacter(new CharacterData(entityId, 10009, charSO)
                {
                    Position = chatData.dialogPos == DialogPos.Left ? mLeft.position : mRight.position,
                    DialogPos = chatData.dialogPos
                });
                mCharIdChace.Add(entityId, chatData);
            }
            //生成
        }

        private void LoadCharacterSuccess(object sender, GameEventArgs args)
        {
            ShowEntitySuccessEventArgs showEntity = (ShowEntitySuccessEventArgs)args;
            if (mCharIdChace.ContainsKey(showEntity.Entity.Id))
            {
                BaseCharacter baseCharacter= showEntity.Entity.GetComponent<BaseCharacter>();
                if (baseCharacter.DialogPos == DialogPos.Left)
                {
                    mLeftChar = baseCharacter;
                    mLeftChar.transform.position = mLeft.position;
                }
                else
                {
                    mRightChar = baseCharacter;
                    mRightChar.transform.position = mRight.position;
                }
                mCharChace[mCharIdChace[showEntity.Entity.Id].charSO] = baseCharacter;

                SetDialogPos(baseCharacter, mCharIdChace[showEntity.Entity.Id].dialogPos);
                SetDialogAction(baseCharacter, mCharIdChace[showEntity.Entity.Id].actionData);
                mCharIdChace.Remove(showEntity.Entity.Id);
            }
        }
    }

    public enum DialogPos
    { 
        Left,//左侧
        Right,//两边
        Middle//中间
    }
}