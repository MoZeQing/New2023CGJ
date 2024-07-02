using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ManagerForm : BaseForm
    {
        [Header("结算面板")]
        [SerializeField] private Transform canvas;
        //[SerializeField] private Text
        [Header("主界面")]
        [SerializeField] private Text moneyText;
        [SerializeField] private Text clientText;
        [SerializeField] private BubbleItem[] bubbles;
        [Header("控制数值")]
        //[SerializeField,Range(0f,2f)] private float speed=1;
        [SerializeField] private float rate=5;
        [SerializeField] private bool isHideItem;
        [SerializeField,Range(0,5)] private float itemLifeTime;
        private float time;
        private float totalTime;
        private int money;
        private int client;
        private ManagerData managerData;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            managerData = (ManagerData)BaseFormData.UserData;
            time = 1/rate;
            for (int i = 0; i < bubbles.Length; i++)
            {
                bubbles[i].gameObject.SetActive(false);
            }
            time = rate;
            totalTime = 10;
            DOTween.To(value => { moneyText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: managerData.Client, duration: 10);
            DOTween.To(value => { clientText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: managerData.Money, duration: 10);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            totalTime -= Time.deltaTime;
            if (totalTime < 0)
                return;
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = rate;
                int block = 50;
                while (true)
                {
                    block--;
                    int index = Random.Range(0, bubbles.Length);
                    if (!bubbles[index].gameObject.activeSelf)
                    {
                        bubbles[index].SetData(Random.Range(1, 6), "测试", itemLifeTime, isHideItem);
                        break;
                    }
                    if (block < 0)
                        break;
                }
            }
        }

        public void CloseForm()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettleForm,managerData);
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
