using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class FlopForm : BaseForm
    {
        /* 1.将脚本改动到框架内
         * 2.卡牌的数字改为图片——》图片使用表格管理，List
         * 3.注意一下音效
         * 4.计算结果，并把结果传给结算界面
         * 5.注意注销Button的监听
         */
        [SerializeField] int pairCount = 8;
        private int[] m_pariGenCount;
        [SerializeField] private GameObject flopCard;
        [SerializeField] private Transform genNode;
        private List<FlopCard> m_cardList = new List<FlopCard>();
        [SerializeField] private FlopCard card01 = null;
        [SerializeField] private FlopCard card02 = null;
        [SerializeField] private float timeLimit;
        [SerializeField] private Text showTime;

        [SerializeField] private CharData charData;
        [SerializeField] private PlayerData playerData;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_pariGenCount = new int[pairCount];
            for (int i = 0; i < pairCount; i++)
                m_pariGenCount[i] = 2;

            InitFlop();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (timeLimit <= 0)
            {
                GameOver();
            }

            timeLimit -= Time.deltaTime;
            showTime.text = FormatTime((int)timeLimit);

            foreach (var card in m_cardList)
            {
                if (card.isFlip && !card.isDone)
                {
                    if (card01 == null)
                    {
                        card01 = card;
                    }
                    else if (card02 == null && card01 != card)
                    {
                        card02 = card;
                    }
                }
            }

            if (card02 != null && card01 != null)
            {
                if (card01.ID == card02.ID)
                {
                    card01.canClick = false;
                    card01.isDone = true;
                    card02.canClick = false;
                    card02.isDone = true;

                    card01 = null;
                    card02 = null;
                }
                else
                {
                    foreach (var card in m_cardList)
                    {
                        card.CoolDown();
                    }
                    card01.TrunBack();
                    card02.TrunBack();

                    card01 = null;
                    card02 = null;
                }
            }
        }

        private void InitFlop()
        {
            do
            {
                var index = Random.Range(0, 8);
                if (m_pariGenCount[index] > 0)
                {
                    var go = Instantiate(flopCard, genNode).GetComponent<FlopCard>();
                    go.ID = index;
                    m_cardList.Add(go);
                    m_pariGenCount[index]--;
                }

            } while (m_cardList.Count < pairCount * 2);
        }

        public static string FormatTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60; // 计算分钟数
            int seconds = totalSeconds % 60; // 计算剩余的秒数

            // 格式化字符串，确保秒数至少有两位数字（如果需要的话）
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void GameOver()
        {

        }
    }
}

