using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using UnityEngine.SocialPlatforms;

public class OrderItem : Entity
{
    [SerializeField] private Image coffeeItem;
    [SerializeField] private Text coffeeName;
    [SerializeField] private Text sugar;
    [SerializeField] private Text condensedMilk;
    [SerializeField] private Text salt;
    [SerializeField] private Text orderTime;
    [SerializeField] private Button exitBtn;

    private OrderItemData mOrderItemData = null;
    private OrderData mOrderData = null;
    private float nowTime=0f;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        Transform orderCanvas = this.transform.Find("OrderForm");
        coffeeItem = orderCanvas.Find("ItemText").GetComponent<Image>();
        sugar = orderCanvas.Find("Sugar").GetComponent<Text>();
        condensedMilk = orderCanvas.Find("CondensedMilk").GetComponent<Text>();
        salt = orderCanvas.Find("Salt").GetComponent<Text>();
        orderTime=orderCanvas.Find("TimeText").GetComponent<Text>();
        //exitBtn = orderCanvas.Find("Exit").GetComponent<Button>();
        coffeeName = orderCanvas.Find("ItemText").GetComponent<Text>();

        //exitBtn.onClick.AddListener(OnExit);
    }
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        mOrderItemData = (OrderItemData)userData;
        mOrderData = mOrderItemData.OrderData;

        //coffeeItem.sprite = GameEntry.Utils.nodeImage[(int)mOrderData.NodeTag];
        //coffeeName.text = mOrderData.NodeName;
        sugar.color = new Color(0f, 0f, 0f, mOrderData.Sugar ? 1f : 0.5f);
        condensedMilk.color = new Color(0f, 0f, 0f, mOrderData.CondensedMilk ? 1f : 0.5f);
        salt.color = new Color(0f, 0f, 0f, mOrderData.Salt ? 1f : 0.5f);
        nowTime = mOrderData.OrderTime;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        nowTime-=Time.deltaTime;
        if (nowTime < 0)
            orderTime.text = "∞";
        else
            orderTime.text = Mathf.Floor(nowTime).ToString();
        if (nowTime <= 0f&&nowTime>-1f)
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
                //if (baseCompenent.Sugar != mOrderData.Sugar)
                //    return;
                //if (baseCompenent.CondensedMilk != mOrderData.CondensedMilk)
                //    return;
                //if (baseCompenent.Salt != mOrderData.Salt)
                //    return;
                //计算收入
                int income = 0;
                IDataTable<DRNode> dtNode=GameEntry.DataTable.GetDataTable<DRNode>();
                foreach (NodeTag nodeTag in baseCompenent.Materials)
                {
                    income += dtNode.GetDataRow((int)nodeTag).Price;
                }
                income += mOrderData.Sugar ? 2 : 0;
                income += mOrderData.CondensedMilk ? 5 : 0;
                income += mOrderData.Salt ? 3 : 0;
                Debug.LogFormat("完成订单，订单收入:{0}", income);
                //计算收入
                GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData, income));
                GameEntry.Entity.HideEntity(baseCompenent.transform.parent.GetComponent<BaseNode>().Entity);
                GameEntry.Entity.HideEntity(this.Entity);
            }
        }
    }
}
