using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMain
{
    public class BaseNode : Entity
    {
        public NodeData NodeData { get; set; }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            NodeData = (NodeData)userData;

            AttachNode();
        }

        private void AttachNode()
        {
            CompenentData data = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10001,this.Id, NodeData);
            //工厂模式
            switch (NodeData.NodeTag)
            {
                case NodeTag.CoffeeBean:
                    GameEntry.Entity.ShowCoffeeBeanNode(data);
                    break;
                case NodeTag.GroundCoffee:
                    GameEntry.Entity.ShowGroundCoffeeNode(data);
                    break;
                case NodeTag.Water:
                    GameEntry.Entity.ShowWaterNode(data);
                    break;
                case NodeTag.HotWater:
                    GameEntry.Entity.ShowHotWaterNode(data);
                    break;
                case NodeTag.Milk:
                    GameEntry.Entity.ShowMilkNode(data);
                    break;
                case NodeTag.HotMilk:
                    GameEntry.Entity.ShowHotMilkNode(data);
                    break;
                case NodeTag.Cream:
                    GameEntry.Entity.ShowCreamNode(data);
                    break;
                case NodeTag.ChocolateSyrup:
                    GameEntry.Entity.ShowChocolateSyrupNode(data);
                    break;
                case NodeTag.CoffeeLiquid:
                    GameEntry.Entity.ShowCoffeeLiquidNode(data);
                    break;
                case NodeTag.Burnisher:
                    GameEntry.Entity.ShowBurnisherNode(data);
                    break;
                case NodeTag.Kettle:
                    GameEntry.Entity.ShowKettleNode(data);
                    break;
                case NodeTag.FilterBowl:
                    GameEntry.Entity.ShowFilterBowlNode(data);
                    break;
                case NodeTag.Cup:
                    GameEntry.Entity.ShowCupNode(data);
                    break;
                case NodeTag.Espresso:
                    GameEntry.Entity.ShowEspressoNode(data);
                    break;
                case NodeTag.CafeAmericano:
                    GameEntry.Entity.ShowCafeAmericanoNode(data);
                    break;
                case NodeTag.WhiteCoffee:
                    GameEntry.Entity.ShowWhiteCoffeeNode(data);
                    break;
                case NodeTag.Latte:
                    GameEntry.Entity.ShowLatteNode(data);
                    break;
                case NodeTag.Mocha:
                    GameEntry.Entity.ShowMochaNode(data);
                    break;
                case NodeTag.ConPanna:
                    GameEntry.Entity.ShowConPannaNode(data);
                    break;
            }
        }
    }
}

