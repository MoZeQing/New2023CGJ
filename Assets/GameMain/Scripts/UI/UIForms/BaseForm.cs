using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class BaseForm : UIFormLogic
    {
        protected BaseFormData BaseFormData { get; set; }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            BaseFormData = (BaseFormData)userData;
            DRUIForms dRUIForms=GameEntry.DataTable.GetDataTable<DRUIForms>().GetDataRow((int)BaseFormData.UIFormId);

            if (dRUIForms.OpenSound != 0)
            {
                GameEntry.Sound.PlaySound(dRUIForms.OpenSound);
            }
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            DRUIForms dRUIForms = GameEntry.DataTable.GetDataTable<DRUIForms>().GetDataRow((int)BaseFormData.UIFormId);
            
            if (dRUIForms.CloseSound != 0)
            {
                GameEntry.Sound.PlaySound(dRUIForms.CloseSound);
            }
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

        public BaseFormData(UIFormId uIFormId,object userData=null)
        { 
            UIFormId= uIFormId;
            UserData= userData;
        }
    }
}


