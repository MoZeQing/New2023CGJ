using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : Entity
{
    [SerializeField] private Image coffeeItem;
    [SerializeField] private Text coffeeName;
    [SerializeField] private Text sugar;
    [SerializeField] private Text condensedMilk;
    [SerializeField] private Text salt;
    [SerializeField] private Text time;

    private OrderItemData mOrderItemData = null;
    private OrderData mOrderData = null;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
    }
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        //mOrderItemData = (OrderItemData)userData;
        //mOrderData = mOrderItemData.OrderData;

        //coffeeItem.sprite = GameEntry.Utils.nodeImage[(int)mOrderData.NodeTag];
        //coffeeName.text = mOrderData.NodeName;
        //sugar.color = new Color(1f, 1f, 1f, mOrderData.Sugar ? 0.5f : 1f);
        //condensedMilk.color = new Color(1f, 1f, 1f, mOrderData.CondensedMilk ? 0.5f : 1f);
        //salt.color = new Color(1f, 1f, 1f, mOrderData.Salt ? 0.5f : 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseCompenent baseCompenent = null;
        if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
        {
            if (baseCompenent.NodeTag == mOrderData.NodeTag)
            {
                GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData));
                GameEntry.Entity.HideEntity(baseCompenent.transform.parent.GetComponent<BaseNode>().Entity);
            }
        }
    }
}
