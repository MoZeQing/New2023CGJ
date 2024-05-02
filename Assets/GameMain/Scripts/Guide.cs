using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Guide : MonoBehaviour
    {
        //Lv1
        //点击咖啡豆之后才会生成研磨器（显示提示）

        //研磨三次之后后才会显示水和加热器（显示提示）
        [SerializeField] private List<GameObject> materials = new List<GameObject>();
        //加热水之后才会增加过滤器（提示）

        //过滤后要求交单

        //Lv2
        //要求制作热拿铁

        //生成牛奶和打泡器

        //要求打发牛奶

        //打发后生成咖啡杯

        //提示交单

        //Lv3
        //要求制作冰拿铁（加糖）

        //要求制作冰拿铁

        //显示加糖，要求加糖

        // Start is called before the first frame update
        void Start()
        {
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide1_1);
        }

        // Update is called once per frame

        public void Guide1_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                //初始化
                materials.Add(GameObject.Find("Materials").transform.Find("CoffeeBean").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Water").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Milk").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Cup").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Sugar").gameObject);

                materials[(int)NodeTag.CoffeeBean].SetActive(true);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "点击咖啡豆槽位，会生成对应的材料卡");
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide1_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_2);
            }
        }

        public void Guide1_2(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.CoffeeBean)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ManualGrinder)
                {
                    Position = Vector3.down * 6
                });
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "使用研磨器将咖啡粉研磨为粗咖啡粉");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_3);
            }
        }

        public void Guide1_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.CoarseGroundCoffee)
                    return;
                materials[1].SetActive(true);//水
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "点击水槽位，会生成对应的材料卡");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_4);
            }
        }

        public void Guide1_4(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Water)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Heater)
                {
                    Position = Vector3.down * 6
                }) ;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "使用加热器将水加热");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_4);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_5);
            }
        }

        public void Guide1_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.HotWater)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Kettle)
                {
                    Position = Vector3.down * 6
                });
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "将热水和细咖啡粉放置到过滤壶中，生成浓缩咖啡");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_5);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
            }
        }

        public void Guide1_6(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Espresso)
                    return;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "拖动生成的咖啡卡牌到左上角的对应订单处");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
                GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide2_1);
            }
        }
        //制作粗咖啡版本的拿铁
        public void Guide2_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "现在，制作一个新的浓缩咖啡吧");
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide2_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
            }
        }

        public void Guide2_2(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Espresso)
                    return;
                materials[2].SetActive(true);
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "点击牛奶槽位，会生成对应的材料卡");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
            }
        }

        public void Guide2_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Milk)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Stirrer)
                {
                    Position = Vector3.down * 6
                });
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "使用搅拌器将牛奶打发");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
            }
        }

        public void Guide2_4(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.LowFoamingMilk)
                    return;
                materials[3].SetActive(true);
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "点击杯槽位，会生成对应的材料卡");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
            }
        }

        public void Guide2_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Cup)
                    return;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "使用杯子将浓缩咖啡和低泡牛奶合成为拿铁");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
            }
        }

        public void Guide2_6(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.HotLatte)
                    return;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "使用拿铁交单");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
                GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide3_1);
            }
        }

        public void Guide3_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "现在制作一杯细浓缩咖啡，因此我们先制作细咖啡粉");
                GameEntry.Player.AddRecipe(2);
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide3_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                Invoke(nameof(Guide3_2), 2f);
            }
        }
        public void Guide3_2()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "所谓细咖啡粉，就是再研磨一次的粗咖啡粉");
        }
        //现在，继续完成细拿铁吧
        public void Guide3_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.FineGroundCoffee)
                    return;
                materials[7].SetActive(true);
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "继续完成细版本的冰拿铁吧！");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
            }
        }

        //public void Guide3_3(object sender, GameEventArgs e)
        //{
        //    ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
        //    BaseCompenent baseCompenent = null;
        //    if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
        //    {
        //        if (baseCompenent.NodeTag != NodeTag.Sugar)
        //            return;
        //        GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
        //        GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "将糖直接放置在拿铁上，那么咖啡就会合成为带糖的咖啡");
        //        Invoke(nameof(Guide3_4), 2f);
        //        GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
        //    }
        //}

        //public void Guide3_4()
        //{
        //    GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
        //    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "带配件的咖啡将会增加咖啡的价格，但是注意：只有订单中具有的配件才会提升咖啡的价格");
        //    Invoke(nameof(Guide3_5), 2f);
        //}

        public void Guide3_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.IceLatte)
                    return;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "那么将咖啡拖动到对应的订单上");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
            }
        }

        //需要在点击交互时发送信息
        public void Guide4_1()
        {
            GameEntry.UI.CloseUIForm(UIFormId.MainForm);
        }

        public void Guide4_2()
        {

        }
    }
}