using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderItemData : EntityData
{
    public OrderData OrderData
    {
        get;
        set;
    }

    public OrderItemData(int entityId, int typeId, OrderData orderData)
        :base(entityId,typeId)
    { 
        OrderData= orderData;
    }
}
