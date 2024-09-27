using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Guide : MonoBehaviour
    {

        [SerializeField] private List<GameObject> materials = new List<GameObject>();

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
                materials[(int)NodeTag.CoffeeBean].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("点击咖啡豆槽位，会生成对应的材料卡", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-370f, -250f, 0f), Vector3.zero));
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
                    Position = new Vector3(-1.86f, -3.68f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("使用研磨器将咖啡粉研磨为粗咖啡粉", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-200f, 100f, 0f), new Vector3(0f,0f,180f)));
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
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("点击水槽位，会生成对应的材料卡", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(450f, 150f, 0f), new Vector3(0f, 0f, 90f)));
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
                    Position = new Vector3(2.96f, -3.74f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("使用加热器将水加热", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(270f, 100f, 0f), new Vector3(0f, 0f, 180f)));
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
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FrenchPress)
                {
                    Position = new Vector3(0.21f, -3.66f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("将热水和粗咖啡粉放置到滤纸漏斗中，生成浓缩咖啡", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(30f, 100f, 0f), new Vector3(0f, 0f, 180f)));
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
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("拖动生成的咖啡卡牌到左边的对应订单处", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-450f, 200f, 0f), new Vector3(0f, 0f, 270f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
                GameEntry.Event.Subscribe(OrderEventArgs.EventId, Guide1_7);
            }
        }
        public void Guide1_7(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (args.OrderData.NodeTag != NodeTag.Espresso)
                return;
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, Guide1_7);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide2_1);
        }
        //制作粗咖啡版本的拿铁
        public void Guide2_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("现在，制作一个新的浓缩咖啡吧", WorkTips.None));
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
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("点击牛奶槽位，会生成对应的材料卡", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-220f, -250f, 0f), Vector3.zero));
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
                    Position = new Vector3(5.26f, -3.73f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("使用搅拌器将牛奶打发", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(510f, 100f, 0f), new Vector3(0f, 0f, 180f)));
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
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("点击杯槽位，会生成对应的材料卡", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(450f, -25f, 0f), new Vector3(0f, 0f, 90f)));
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
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("使用杯子将浓缩咖啡和低泡牛奶合成为卡布奇诺", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
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
                if (baseCompenent.NodeTag != NodeTag.Kapuziner)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("使用卡布奇诺交单", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-450f, 200f, 0f), new Vector3(0f, 0f, 270f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
                GameEntry.Event.Subscribe(OrderEventArgs.EventId, Guide2_7);
            }
        }
        public void Guide2_7(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (args.OrderData.NodeTag != NodeTag.Kapuziner)
                return;
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, Guide2_7);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide3_1);
        }

        public void Guide3_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("现在制作一杯细浓缩咖啡，因此我们先制作细咖啡粉", WorkTips.None));
                GameEntry.Player.AddRecipe(1);
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide3_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                Invoke(nameof(Guide3_2), 2f);
            }
        }
        public void Guide3_2()
        {
            GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("所谓细咖啡粉，就是再研磨一次的粗咖啡粉", WorkTips.None));
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
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("继续完成细版本的冰卡布奇诺吧！\n<size=24>（对于配方有疑惑，可以点击右下角的制作手册查看配方）</size>", WorkTips.None));

                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
                Invoke(nameof(Guide3_4), 2f);
            }
        }

        public void Guide3_4()
        {
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(550f, -250f, 0f), Vector3.zero));
        }

        public void Guide3_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.IceKapuziner)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("那么将咖啡拖动到对应的订单上", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-450f, 200f, 0f), new Vector3(0f, 0f, 270f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
                GameEntry.Event.Subscribe(OrderEventArgs.EventId, Guide3_6);
            }
        }

        public void Guide3_6(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (args.OrderData.NodeTag != NodeTag.IceKapuziner)
                return;
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, Guide3_6);
        }
    }
}