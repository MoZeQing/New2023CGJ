using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.UI;
using UnityGameFramework.Runtime;
using GameFramework.DataTable;

namespace GameMain
{
    public static class UIExtension
    {
        public static void CloseUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        { 
            
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, userData);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            IDataTable<DRUIForms> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForms>();
            DRUIForms drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!drUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }
            Debug.Log(assetName);
            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, /*Constant.AssetPriority.UIFormAsset*/50, drUIForm.PauseCoveredUIForm, userData);
        }

        public static void CloseUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        { 

        }
    }

}