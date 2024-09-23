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
using DG.Tweening;

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
            ice = orderCanvas.Find("Ice").GetComponent<Image>();
            grind = orderCanvas.Find("Grind").GetComponent<Image>();
            hot=orderCanvas.Find("Hot").GetComponent<Image>();
            coarse = orderCanvas.Find("Coarse").GetComponent<Image>();
            friendImg = orderCanvas.Find("FriendImg").GetComponent<Image>();
            coffeeName = orderCanvas.Find("ItemText").GetComponent<Text>();
            timeLine = orderCanvas.Find("TimeLine").GetComponent<Image>();
            badImg = orderCanvas.Find("BadImg").GetComponent<Image>();
            badText= badImg.transform.Find("BadText").GetComponent<Text>();
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
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
            timeLine.gameObject.SetActive(mOrderData.OrderTime > 0);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (mOrderData.OrderTag != OrderTag.Urgent)
                return;
            nowTime -= Time.deltaTime;
            timeLine.fillAmount = Mathf.Max(nowTime / mOrderData.OrderTime, 0f);       
            if (nowTime <= 0f && nowTime > -1f)
            {
                nowTime = -9999;
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
