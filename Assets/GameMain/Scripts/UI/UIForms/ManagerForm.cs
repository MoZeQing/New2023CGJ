using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ManagerForm : UIFormLogic
    {
        [SerializeField] private Text moneyText;
        [SerializeField] private Text clientText;
        [SerializeField] private BubbleItem[] bubbles;
        [SerializeField]
        private float speed;
        private float time;
        private int money;
        private int client;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            time = 1/speed;
            for (int i = 0; i < bubbles.Length; i++)
            {
                bubbles[i].gameObject.SetActive(false);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 1/speed;
                bubbles[Random.Range(0, bubbles.Length)].SetData(3, "²âÊÔ");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void Start()
        {
            time = speed;
            for (int i = 0; i < bubbles.Length; i++)
            {
                bubbles[i].gameObject.SetActive(false);
            }
            DOTween.To(value => { moneyText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: 4396, duration: 10);
            DOTween.To(value => { clientText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: 468, duration: 10);
        }

        private void Update()
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = speed;
                int block = 50;
                while (true)
                {
                    block--;
                    int index = Random.Range(0, bubbles.Length);
                    if (!bubbles[index].gameObject.activeSelf)
                    {
                        bubbles[index].SetData(Random.Range(1,6), "²âÊÔ");
                        break;
                    }
                    if (block < 0)
                        break;
                }
            }
        }
    }
}
