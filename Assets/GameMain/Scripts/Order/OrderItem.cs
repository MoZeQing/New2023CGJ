using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using UnityEngine.SocialPlatforms;
using XNode.Examples.RuntimeMathNodes;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace GameMain
{
    public class OrderItem : Entity,IPointerClickHandler
    {
        [SerializeField] private Image coffeeItem;
        [SerializeField] private Text coffeeName;
        [SerializeField] private Text grindText;
        [SerializeField] private Image ice;
        [SerializeField] private Image hot;
        [SerializeField] private Image grind;
        [SerializeField] private Image coarse;
        [SerializeField] private Image friendImg;
        [SerializeField] private Image timeLine;
        [SerializeField] private Image badImg;
        [SerializeField] private Text badText;

        private OrderItemData mOrderItemData = null;
        private OrderData mOrderData = null;
        private float nowTime = 0f;
        private int badCount;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Transform orderCanvas = this.transform.Find("OrderForm");
            coffeeItem = orderCanvas.Find("Item").GetComponent<Image>();
            //sugar = orderCanvas.Find("Sugar").GetComponent<Text>();
            //condensedMilk = orderCanvas.Find("CondensedMilk").GetComponent<Text>();
            //salt = orderCanvas.Find("Salt").GetComponent<Text>();
            ice = orderCanvas.Find("Ice").GetComponent<Image>();
            grind = orderCanvas.Find("Grind").GetComponent<Image>();
            hot=orderCanvas.Find("Hot").GetComponent<Image>();
            coarse = orderCanvas.Find("Coarse").GetComponent<Image>();
            friendImg = orderCanvas.Find("FriendImg").GetComponent<Image>();
            //exitBtn = orderCanvas.Find("Exit").GetComponent<Button>();
            coffeeName = orderCanvas.Find("ItemText").GetComponent<Text>();
            timeLine = orderCanvas.Find("TimeLine").GetComponent<Image>();
            badImg = orderCanvas.Find("BadImg").GetComponent<Image>();
            badText= badImg.transform.Find("BadText").GetComponent<Text>();

            //exitBtn.onClick.AddListener(OnExit);
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            hasBad= false;
            mOrderItemData = (OrderItemData)userData;
            mOrderData = mOrderItemData.OrderData;
            DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mOrderData.NodeTag);
            coffeeItem.sprite = Resources.Load<Sprite>(dRNode.IconPath);
            coffeeName.text = dRNode.Description;
            coarse.gameObject.SetActive(mOrderData.Grind);
            grind.gameObject.SetActive(!mOrderData.Grind);
            hot.gameObject.SetActive(!dRNode.Ice);
            ice.gameObject.SetActive(dRNode.Ice);
            timeLine.gameObject.SetActive(false);
            nowTime = mOrderData.OrderTime;
            badImg.gameObject.SetActive(false);
            badCount = 0;
            if (mOrderData.Urgent)
            {
                timeLine.gameObject.SetActive(mOrderData.Urgent);
            }
            else
            {
                timeLine.gameObject.SetActive(mOrderData.Urgent);
            }
            Debug.Log(nowTime);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            Bad();
            if (mOrderData.LevelTag != LevelTag.Bad && mOrderData.LevelTag != LevelTag.Urgent)
                return;
            if (!hasBad)
                return;
            nowTime -= Time.deltaTime;
            timeLine.fillAmount = nowTime / mOrderData.OrderTime;       
            if (nowTime < mOrderData.OrderTime)
            {
                timeLine.color = Color.green;
            }
            if (nowTime < mOrderData.OrderTime * 2f / 3f) 
            {
                timeLine.color = Color.yellow;
            }
            if (nowTime < mOrderData.OrderTime * 1f / 3f) 
            {
                timeLine.color = Color.red;
            }
            if (nowTime <= 0f && nowTime > -1f)
            {
                nowTime = -1;
                OnExit();
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            nowTime = 9999f;
        }

        public void OnExit()
        {
            GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData, 0));
            GameEntry.Entity.HideEntity(this.Entity);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hasBad&&timeLine.gameObject.activeSelf)
                return;
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag == mOrderData.NodeTag)
                {
                    if (mOrderData.Grind != baseCompenent.Grind)
                        return;
                    int income = 0;
                    IDataTable<DRNode> dtNode = GameEntry.DataTable.GetDataTable<DRNode>();
                    income = dtNode.GetDataRow((int)mOrderData.NodeTag).Price;
                    GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData, income));
                    GameEntry.Entity.HideEntity(baseCompenent.transform.parent.GetComponent<BaseNode>().Entity);
                    GameEntry.Entity.HideEntity(this.Entity);
                }
            }
        }

        private bool hasBad=false;

        public void Bad()
        {
            if (timeLine.gameObject.activeSelf)
                return;
            if (UnityEngine.Random.Range(0, 301) != 300)
                return;
            if (hasBad)
                return;
            if (badCount > 0)
                return;
            if (mOrderData.LevelTag==LevelTag.Bad)
            {
                hasBad = true;
                badImg.gameObject.SetActive(true);
                timeLine.gameObject.SetActive(true);
                nowTime = 5f;
                mOrderData.OrderTime = nowTime;
                badCount = 10;
                badText.text=badCount.ToString();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (badCount == 0)
                return;
            badCount--;
            badText.text= badCount.ToString(); 
            if (badCount == 0)
            { 
                badImg.gameObject.SetActive(false);
                timeLine.gameObject.SetActive(false);
                nowTime = -1;
            }
        }
    }

}
