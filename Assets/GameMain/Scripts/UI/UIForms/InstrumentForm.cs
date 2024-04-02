using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System;
using DG.Tweening;

namespace GameMain
{
    public class InstrumentForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Image heaterImage;
        [SerializeField] private Image burnisherImage;
        [SerializeField] private Image stirrerImage;
        [SerializeField] private Image pressImage;
        [SerializeField] private Text heaterText;
        [SerializeField] private Text burnisherText;
        [SerializeField] private Text stirrerText;
        [SerializeField] private Text pressText;
        [SerializeField] private Transform canvas;

        private InstrumentTag instrumentTag;
        private DRItem dRItem = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exitBtn.onClick.AddListener(()=>GameEntry.UI.CloseUIForm(this.UIForm));

            heaterImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).ImagePath);
            burnisherImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).ImagePath);
            stirrerImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).ImagePath);
            pressImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).ImagePath);

            heaterText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Price.ToString() + GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Info;
            burnisherText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Price.ToString() + GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Info;
            stirrerText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Price.ToString() + GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Info;
            pressText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Price.ToString() + GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Info;
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

    public enum InstrumentTag
    {
        Heater,
        Burnisher,
        Stirrer,
        Press
    }
}