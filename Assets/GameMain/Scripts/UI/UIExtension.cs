using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.UI;
using UnityGameFramework.Runtime;
using GameFramework.DataTable;
using System;

namespace GameMain
{
    public static class UIExtension
    {
        public static bool HasUIForm(this UIComponent uiComponent, UIFormId uiFormId)
        {
            IDataTable<DRUIForms> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForms>();
            DRUIForms drUIForm = dtUIForm.GetDataRow((int)uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return false;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            return uiComponent.HasUIForm(assetName);
        }
        public static void CloseUIGroup(this UIComponent uiComponent,string groupName)
        {
            IUIGroup uIGroup = uiComponent.GetUIGroup(groupName);
            IUIForm[] forms = uIGroup.GetAllUIForms();
            foreach (IUIForm form in forms)
            {
                UIForm ui = GameEntry.UI.GetUIForm(form.UIFormAssetName);
                GameEntry.UI.CloseUIForm(ui);
            }
        }
        public static void CloseUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        {
            IDataTable<DRUIForms> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForms>();
            DRUIForms drUIForm = dtUIForm.GetDataRow((int)uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("错误，表中不存在编号为{0}的UI", uiFormId.ToString());
                return;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if(!uiComponent.HasUIForm(assetName))
            {
                return;
            }

            UIForm uiForm = uiComponent.GetUIForm(assetName);
            GameEntry.UI.CloseUIForm(uiForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, Action mAction, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, new BaseFormData(uiFormId, mAction, userData));
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, new BaseFormData(uiFormId,userData));
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, BaseFormData userData = null)
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
            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, 50, drUIForm.PauseCoveredUIForm, userData);
        }

        public static UIForm GetUIForm(this UIComponent uiComponent, int uiFormId)
        {
            IDataTable<DRUIForms> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForms>();
            DRUIForms drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!uiComponent.IsLoadingUIForm(assetName))
                return null;

            if (!uiComponent.HasUIForm(assetName))
                return null;
            return uiComponent.GetUIForm(assetName);
        }

        public static UIForm GetUIForm(this UIComponent uiComponent, UIFormId uiFormId)
        {
            IDataTable<DRUIForms> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForms>();
            DRUIForms drUIForm = dtUIForm.GetDataRow((int)uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);

            if (!uiComponent.HasUIForm(assetName))
                return null;
            return uiComponent.GetUIForm(assetName);
        }
    }

}