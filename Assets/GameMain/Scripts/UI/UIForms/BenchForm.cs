using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class BenchForm : UIFormLogic
    {
        [SerializeField] protected List<CardItem> mCards = new List<CardItem>();

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
                mCards[i].SetData(dRBenches[Random.Range(0,dRBenches.Count)]);
            }
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
}
