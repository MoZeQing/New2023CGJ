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
    public class OrderItem : Entity/*, IPointerClickHandler*/
    {
        [SerializeField] private Image coffeeItem;
        [SerializeField] private Text coffeeName;
        //[SerializeField] private Text sugar;
        //[SerializeField] private Text condensedMilk;
        //[SerializeField] private Text salt;
        [SerializeField] private Text grindText;
        [SerializeField] private Image ice;
        [SerializeField] private Image hot;
        [SerializeField] private Image grind;
        [SerializeField] private Image coarse;
        [SerializeField] private Image friendImg;
        [SerializeField] private Image timeLine;
        [SerializeField] private Button exitBtn;

        private OrderItemData mOrderItemData = null;
        private OrderData mOrderData = null;
        private float nowTime = 0f;

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

            //exitBtn.onClick.AddListener(OnExit);
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            mOrderItemData = (OrderItemData)userData;
            mOrderData = mOrderItemData.OrderData;
            DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mOrderData.NodeTag);
            coffeeItem.sprite = Resources.Load<Sprite>(dRNode.ImagePath);
            coffeeName.text = dRNode.Description;
            coarse.gameObject.SetActive(mOrderData.Grind);
            grind.gameObject.SetActive(!mOrderData.Grind);
            hot.gameObject.SetActive(!dRNode.Ice);
            ice.gameObject.SetActive(dRNode.Ice);
            if (mOrderItemData.OrderData.friendName != null&& mOrderItemData.OrderData.friendName != string.Empty)
            {
                friendImg.sprite = GameEntry.Utils.chars[mOrderItemData.OrderData.friendName].orderSprite;
            }
            else
            {
                friendImg.sprite = GameEntry.Utils.orderSprite;
            }
            nowTime = mOrderData.OrderTime * 3 * GameEntry.Utils.OrderPower;
            Debug.Log(nowTime);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            nowTime -= Time.deltaTime;
            timeLine.fillAmount = nowTime / (mOrderData.OrderTime * 3 * GameEntry.Utils.OrderPower);
            
            if (nowTime < mOrderData.OrderTime * 3 * GameEntry.Utils.OrderPower)
            {
                timeLine.color = Color.green;
            }
            if (nowTime < mOrderData.OrderTime * 2 * GameEntry.Utils.OrderPower)
            {
                timeLine.color = Color.yellow;
            }
            if (nowTime < mOrderData.OrderTime * GameEntry.Utils.OrderPower)
            {
                timeLine.color = Color.red;
            }
            //if (nowTime < 0)
            //    timeLine.fillAmount = 1;
            //else
            //    timeLine.fillAmount = nowTime / mOrderData.OrderTime;
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

        private void OnExit()
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
                    if (mOrderData.Urgent)
                        income = (int)(income * 1.5f);
                    income += mOrderData.Sugar ? 2 : 0;
                    income += mOrderData.CondensedMilk ? 5 : 0;
                    income += mOrderData.Salt ? 3 : 0;
                    float p = 1f;
                    if (nowTime > mOrderData.OrderTime * 2 * GameEntry.Utils.OrderPower)
                    {
                        p = 1.5f;
                        GameEntry.Utils.PlayerData.acoffee++;
                        GameEntry.Utils.PlayerData.bcoffee++;
                        GameEntry.Utils.PlayerData.ccoffee++;
                    }
                    else if (nowTime > mOrderData.OrderTime * 1 * GameEntry.Utils.OrderPower)
                    {
                        p = 1f;
                        GameEntry.Utils.PlayerData.bcoffee++;
                        GameEntry.Utils.PlayerData.ccoffee++;
                    }
                    else
                    {
                        p = 0.8f;
                        GameEntry.Utils.PlayerData.ccoffee++;
                    }
                    income = (int)(income * p*GameEntry.Utils.PricePower);
                    if (mOrderItemData.OrderData.friendName != null)
                    {
                        GameEntry.Utils.AddFriendFavor(mOrderItemData.OrderData.friendName, mOrderItemData.OrderData.friendFavor);
                    }
                    GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData, income));
                    GameEntry.Entity.HideEntity(baseCompenent.transform.parent.GetComponent<BaseNode>().Entity);
                    GameEntry.Entity.HideEntity(this.Entity);
                }
            }
        }

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData, 0));
        //    GameEntry.Entity.HideEntity(this.Entity);
        //}
    }

}
