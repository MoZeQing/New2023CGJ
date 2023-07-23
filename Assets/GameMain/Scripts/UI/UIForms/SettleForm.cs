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

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OrderData orderData=(OrderData)userData;
            mMoney.text = orderData.OrderMoney.ToString();

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
            
        }
    }
}
