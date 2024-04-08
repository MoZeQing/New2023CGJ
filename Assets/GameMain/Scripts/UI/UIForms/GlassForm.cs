using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System;

namespace GameMain
{
    public class GlassForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button heaterBtn;
        [SerializeField] private Button burnisherBtn;
        [SerializeField] private Button stirrerBtn;
        [SerializeField] private Button pressBtn;
        [SerializeField] private Image heaterImage;
        [SerializeField] private Image burnisherImage;
        [SerializeField] private Image stirrerImage;
        [SerializeField] private Image pressImage;
        [SerializeField] private Text heaterText;
        [SerializeField] private Text burnisherText;
        [SerializeField] private Text stirrerText;
        [SerializeField] private Text pressText;

        private InstrumentTag instrumentTag;
        private DRItem dRItem = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(OnExit);

            heaterBtn.onClick.AddListener(()=>UpdateInstrument(InstrumentTag.Heater));
            burnisherBtn.onClick.AddListener(() => UpdateInstrument(InstrumentTag.Burnisher));
            stirrerBtn.onClick.AddListener(() => UpdateInstrument(InstrumentTag.Stirrer));
            pressBtn.onClick.AddListener(() => UpdateInstrument(InstrumentTag.Press));

            heaterBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Preposition != 0;
            burnisherBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Preposition != 0;
            stirrerBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Preposition != 0;
            pressBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Preposition != 0;

            heaterBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Price <=GameEntry.Utils.Money;
            burnisherBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Price <= GameEntry.Utils.Money;
            stirrerBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Price <= GameEntry.Utils.Money;
            pressBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Price <= GameEntry.Utils.Money;

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
            heaterBtn.onClick.RemoveAllListeners();
            stirrerBtn.onClick.RemoveAllListeners();
            burnisherBtn.onClick.RemoveAllListeners();
            pressBtn.onClick.RemoveAllListeners();
        }

        private void UpdateInstrument(InstrumentTag instrumentTag)
        {
            switch (instrumentTag)
            {
                case InstrumentTag.Press:
                    dRItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID);
                    this.instrumentTag = InstrumentTag.Press;
                    break;
                case InstrumentTag.Stirrer:
                    dRItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID);
                    this.instrumentTag = InstrumentTag.Stirrer;
                    break;
                case InstrumentTag.Heater:
                    dRItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID);
                    this.instrumentTag = InstrumentTag.Heater;
                    break;
                case InstrumentTag.Burnisher:
                    dRItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID);
                    this.instrumentTag = InstrumentTag.Burnisher;
                    break;
            }

            DRItem updateItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(dRItem.Preposition);
            GameEntry.UI.OpenUIForm(UIFormId.OkTips, UpdateItem, "��ȷ��Ҫ������");
        }

        private void UpdateItem()
        {
            GameEntry.Utils.Money -= dRItem.Price;
            switch (this.instrumentTag)
            {
                case InstrumentTag.Press:
                    GameEntry.Utils.PlayerData.pressID = dRItem.Preposition;
                    break;
                case InstrumentTag.Stirrer:
                    GameEntry.Utils.PlayerData.stirrerID = dRItem.Preposition;
                    break;
                case InstrumentTag.Heater:
                    GameEntry.Utils.PlayerData.heaterID = dRItem.Preposition;
                    break;
                case InstrumentTag.Burnisher:
                    GameEntry.Utils.PlayerData.burnisherID = dRItem.Preposition;
                    break;
            }

            heaterBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Preposition != 0;
            burnisherBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Preposition != 0;
            stirrerBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Preposition != 0;
            pressBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Preposition != 0;

            heaterBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Price <= GameEntry.Utils.Money;
            burnisherBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Price <= GameEntry.Utils.Money;
            stirrerBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Price <= GameEntry.Utils.Money;
            pressBtn.interactable = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Price <= GameEntry.Utils.Money;

            heaterImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).ImagePath);
            burnisherImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).ImagePath);
            stirrerImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).ImagePath);
            pressImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).ImagePath);

            heaterText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Price.ToString()+GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.heaterID).Info;
            burnisherText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Price.ToString() + GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.burnisherID).Info;
            stirrerText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Price.ToString() + GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.stirrerID).Info;
            pressText.text = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Price.ToString() + GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(GameEntry.Utils.PlayerData.pressID).Info;
        }

        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
