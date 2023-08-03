using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class SettleForm : UIFormLogic
    {
        [SerializeField] private Text mOrder;
        [SerializeField] private Text mMoney;
        [SerializeField] private Button mOKButton;

        private LevelData mLevelData;

        //思考传入什么参数
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mLevelData=(LevelData)userData;

            mMoney.text = mLevelData.OrderData.OrderMoney.ToString();

            mOKButton.onClick.AddListener(Click);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void Click()
        {
            GameEntry.UI.CloseUIForm(this.UIForm);//更改
            if (mLevelData.Index == 3)
            {
                GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, mLevelData.Day+1);
            }
            else
            {
                GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, 0);
            }
        }
    }
}
