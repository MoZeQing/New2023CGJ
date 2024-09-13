using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System;

namespace GameMain
{
    public class BaseForm : UIFormLogic
    {
        protected BaseFormData BaseFormData { get; set; }
        protected virtual void OnStart(object userData)
        {
            BaseFormData = (BaseFormData)userData;
            DRUIForms dRUIForms = GameEntry.DataTable.GetDataTable<DRUIForms>().GetDataRow((int)BaseFormData.UIFormId);

            if (dRUIForms.OpenSound != 0)
            {
                GameEntry.Sound.PlaySound(dRUIForms.OpenSound);
            }
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnStart(userData);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }

    public class BaseFormData
    { 
        public UIFormId UIFormId
        {
            get;set;
        }

        public object UserData
        {
            get;set;
        }
        public Action Action
        {
            get;set;
        }
        public BaseFormData(UIFormId uIFormId,object userData=null)
        { 
            UIFormId= uIFormId;
            UserData= userData;
        }
        public BaseFormData(UIFormId uIFormId, Action action,object userData = null)
        {
            UIFormId = uIFormId;
            UserData = userData;
            Action = action;
        }
    }
}


