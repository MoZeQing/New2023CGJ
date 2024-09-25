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
            foreach (int id in GameEntry.Buff.GetSaveData().buffs)
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
    [System.Serializable]
    public class BuffData
    {
        public List<int> buffs = new List<int>();
        public float MoneyMulti { get; set; }
        public float MoneyPlus { get; set; }
        public float EnergyMulti { get; set; }
        public float EnergyPlus { get; set; }
        public float EnergyMaxMulti { get; set; }
        public float EnergyMaxPlus { get; set; }
        public float FavorMulti { get; set; }
        public float FavorPlus { get; set; }
        public float TimeMulti { get; set; }
        public float TimePlus { get; set; }
        public float WisdomPlus { get; set; }
        public float StaminaPlus { get; set; }
        public float CharmPlus { get; set; }

        public void AddBuff(DRBuff dRBuff)
        {
            MoneyMulti += dRBuff.MoneyMulti / 100f;
            MoneyPlus += dRBuff.MoneyPlus / 100f;
            EnergyMulti += dRBuff.EnergyMulti / 100f;
            EnergyPlus += dRBuff.EnergyPlus / 100f;
            EnergyMaxMulti += dRBuff.EnergyMaxMulti / 100f;
            EnergyMaxPlus += dRBuff.EnergyMaxPlus / 100f;
            FavorMulti += dRBuff.FavorMulti / 100f;
            FavorPlus += dRBuff.FavorPlus / 100f;
            TimeMulti += dRBuff.TimeMulti / 100f;
            TimePlus += dRBuff.TimePlus / 100f;
            WisdomPlus += dRBuff.WisdomPlus / 100f;
            StaminaPlus += dRBuff.StaminaPlus / 100f;
            CharmPlus += dRBuff.CharmPlus / 100f;
        }

        public void AddBuff(int buffIndex)
        {
            if (!GameEntry.DataTable.GetDataTable<DRBuff>().HasDataRow(buffIndex))
            {
                Debug.LogErrorFormat("错误，你输入了一个无效的buffID");
                return;
            }
            DRBuff dRBuff = GameEntry.DataTable.GetDataTable<DRBuff>().GetDataRow(buffIndex);
            AddBuff(dRBuff);
        }
        public void RemoveBuff(int buffIndex)
        {
            DRBuff dRBuff = GameEntry.DataTable.GetDataTable<DRBuff>().GetDataRow(buffIndex);
            RemoveBuff(dRBuff);
        }
        public void RemoveBuff(DRBuff dRBuff)
        {
            MoneyMulti -= dRBuff.MoneyMulti / 100f;
            MoneyPlus -= dRBuff.MoneyPlus / 100f;
            EnergyMulti -= dRBuff.EnergyMulti / 100f;
            EnergyPlus -= dRBuff.EnergyPlus / 100f;
            EnergyMaxMulti -= dRBuff.EnergyMaxMulti / 100f;
            EnergyMaxPlus -= dRBuff.EnergyMaxPlus / 100f;
            FavorMulti -= dRBuff.FavorMulti / 100f;
            FavorPlus -= dRBuff.FavorPlus / 100f;
            TimeMulti -= dRBuff.TimeMulti / 100f;
            TimePlus -= dRBuff.TimePlus / 100f;
            WisdomPlus -= dRBuff.WisdomPlus / 100f;
            StaminaPlus -= dRBuff.StaminaPlus / 100f;
            CharmPlus -= dRBuff.CharmPlus / 100f;
        }

        public BuffData()
        {
            MoneyMulti = 1;
            MoneyPlus = 0;
            EnergyMulti = 1;
            EnergyPlus = 0;
            EnergyMaxMulti = 1;
            EnergyMaxPlus = 0;
            FavorMulti = 1;
            FavorPlus = 0;
            TimeMulti = 1;
            TimePlus = 0;
            WisdomPlus = 0;
            StaminaPlus = 0;
            CharmPlus = 0;
        }
        public BuffData(DRBuff dRBuff)
        {
            MoneyMulti = dRBuff.MoneyMulti / 100f;
            MoneyPlus = dRBuff.MoneyPlus / 100f;
            EnergyMulti = dRBuff.EnergyMulti / 100f;
            EnergyPlus = dRBuff.EnergyPlus / 100f;
            EnergyMaxMulti = dRBuff.EnergyMaxMulti / 100f;
            EnergyMaxPlus = dRBuff.EnergyMaxPlus / 100f;
            FavorMulti = dRBuff.FavorMulti / 100f;
            FavorPlus = dRBuff.FavorPlus / 100f;
            TimeMulti = dRBuff.TimeMulti / 100f;
            TimePlus = dRBuff.TimePlus / 100f;
            WisdomPlus = dRBuff.WisdomPlus / 100f;
            StaminaPlus = dRBuff.StaminaPlus / 100f;
            CharmPlus = dRBuff.CharmPlus / 100f;
        }
    }
}

