using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class BenchForm : BaseForm
    {
        [SerializeField] protected List<CardItem> mCards = new List<CardItem>();
        [SerializeField] private Button okBtn;
        [SerializeField] private Button exitBtn;
        private List<DRBench> dRBenches = new List<DRBench>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            foreach (DRBench bench in GameEntry.DataTable.GetDataTable<DRBench>().GetAllDataRows())
            {
                if (Resources.Load<CharSO>("CharData/witch").favor < bench.Favor)
                    continue;
                dRBenches.Add(bench);
            }
            for (int i = 0; i < mCards.Count; i++)
            {
                mCards[i].SetData(dRBenches[Random.Range(0, dRBenches.Count)],OnClick);
                mCards[i].GetComponent<Button>().interactable = true;
            }
            okBtn.gameObject.SetActive(false);
            okBtn.onClick.AddListener(OnExit);
            exitBtn.onClick.AddListener(OnExit);
            exitBtn.interactable = true;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            okBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
        }

        private void OnClick(DRBench dRBench)
        {
            for (int i = 0; i < mCards.Count; i++)
            {
                mCards[i].GetComponent<Button>().interactable = false;
            }
            GameEntry.Utils.Energy += dRBench.Energy;
            GameEntry.Utils.MaxEnergy += dRBench.EnergyMax;
            GameEntry.Utils.Money+= dRBench.Money;
            foreach (string eventData in dRBench.Buff)
            {
                GameEntry.Utils.RunEvent(eventData);
            }
            okBtn.gameObject.SetActive(true);
            exitBtn.interactable = false;
        }

        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
