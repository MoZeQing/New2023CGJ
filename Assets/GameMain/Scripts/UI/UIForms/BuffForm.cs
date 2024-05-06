using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;
using DG.Tweening;

namespace GameMain
{
    public class BuffForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private Text text;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));

            text.text = string.Empty;
            foreach (int id in GameEntry.Buff.GetData())
            { 
                DRBuff dRBuff=GameEntry.DataTable.GetDataTable<DRBuff>().GetDataRow(id);
                text.text += dRBuff.BuffName+":\n";
                text.text += dRBuff.BuffText + "\n\n";
            }
            text.text = text.text.Replace("\\n", "\n");
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }
    }
}

