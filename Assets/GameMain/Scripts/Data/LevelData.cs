using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string levelName;
    public bool isRemove;
    public ParentTrigger trigger;
    public LevelTag levelTag;
    public string foreWork;
    public string afterWork;
    public List<NewOrderData> orderDatas=new List<NewOrderData>();
    public int levelTime;
    public int levelMoney;
    public bool isCoarse;
    public bool notCoarse;


    public LevelData() { }
    public LevelData(DRLevel level)
    {
        levelName = level.LevelName;
        isRemove = level.IsRemove;
        trigger = new ParentTrigger(level.Trigger);
        levelTag = (LevelTag)level.LevelTag;
        foreWork = level.ForeDialogName;
        afterWork=level.AfterDialogName;
        if(!string.IsNullOrEmpty(level.OrderDatas))
        {
            string[] orderTexts = level.OrderDatas.Split('-');
            foreach (string orderText in orderTexts)
            {
                string[] orders = orderText.Split('=');
                NewOrderData orderData = new NewOrderData()
                {
                    nodeNodeTag = int.Parse(orders[0]),
                    orderTime = int.Parse(orders[1])
                };
                orderDatas.Add(orderData);
            }
        }
        levelTime = level.LevelTime;
        levelMoney = level.LevelMoney;
        isCoarse= level.IsCoarse;
        notCoarse= level.NotCoarse;
    }
    public LevelData(LevelSO level)
    {
        levelName = level.name;
        isRemove = level.isRemove;
        trigger = level.trigger;
        levelTag = level.levelTag;
        foreWork = level.foreWork?.name;
        afterWork = level.afterWork?.name;
        orderDatas = level.orderDatas;
        levelTime = level.levelTime;
        levelMoney = level.levelMoney;
        isCoarse = level.isCoarse;
        notCoarse = level.notCoarse;
    }
    public List<OrderData> GetRandOrderDatas()
    {
        List<OrderData> ans = new List<OrderData>();
        foreach (NewOrderData newOrderData in orderDatas)
            ans.Add(newOrderData.GetOrderData(levelTag, newOrderData.orderTime, isCoarse, notCoarse));
        return ans;
    }
}
