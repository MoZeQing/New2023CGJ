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
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            NodeData = (NodeData)userData;

            AttachNode();
        }

        private void AttachNode()
        {
            CompenentData data = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10001,this.Id, NodeData);
            //工厂模式
            switch (NodeData.NodeTag)
            {
                //原材料
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
                case NodeTag.Sugar:
                    GameEntry.Entity.ShowSugarNode(data);
                    break;
                case NodeTag.Ice:
                    GameEntry.Entity.ShowIceNode(data);
                    break;
                //咖啡
                case NodeTag.CafeAmericano:
                    CompenentData CafeAmericano = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10007, this.Id, NodeData);
                    GameEntry.Entity.ShowCafeAmericanoNode(CafeAmericano);
                    break;
                case NodeTag.WhiteCoffee:
                    CompenentData WhiteCoffee = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10007, this.Id, NodeData);
                    GameEntry.Entity.ShowWhiteCoffeeNode(WhiteCoffee);
                    break;
                case NodeTag.Latte:
                    CompenentData Latte = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10007, this.Id, NodeData);
                    GameEntry.Entity.ShowLatteNode(Latte);
                    break;
                case NodeTag.Mocha:
                    CompenentData Mocha = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10007, this.Id, NodeData);
                    GameEntry.Entity.ShowMochaNode(Mocha);
                    break;
                case NodeTag.ConPanna:
                    CompenentData ConPanna = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10007, this.Id, NodeData);
                    GameEntry.Entity.ShowConPannaNode(ConPanna);
                    break;
                    //工具
                case NodeTag.Burnisher:
                    CompenentData burnisher = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10003, this.Id, NodeData);
                    GameEntry.Entity.ShowBurnisherNode(burnisher);
                    break;
                case NodeTag.Kettle:
                    CompenentData kettle = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10002, this.Id, NodeData);
                    GameEntry.Entity.ShowKettleNode(kettle);
                    break;
                case NodeTag.FilterBowl:
                    CompenentData filterBowl = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10004, this.Id, NodeData);
                    GameEntry.Entity.ShowFilterBowlNode(filterBowl);
                    break;
                case NodeTag.Espresso:
                    CompenentData espresso = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10006, this.Id, NodeData);
                    GameEntry.Entity.ShowEspressoNode(espresso);
                    break;
                case NodeTag.Cup:
                    CompenentData cup = new CompenentData(GameEntry.Entity.GenerateSerialId(), 10005, this.Id, NodeData);
                    GameEntry.Entity.ShowCupNode(cup);
                    break;
                //冰咖啡
                case NodeTag.IceEspresso:
                    GameEntry.Entity.ShowIceEspressoNode(data);
                    break;
                case NodeTag.IceWhiteCoffee:
                    GameEntry.Entity.ShowIceWhiteCoffeeNode(data);
                    break;
                /*case NodeTag.IceConPanna:
                    GameEntry.Entity.ShowIceConPannaNode(data);
                    break;
                case NodeTag.IceLatte:
                    GameEntry.Entity.ShowIceLatteNode(data);
                    break;*/
                case NodeTag.IceCafeAmericano:
                    GameEntry.Entity.ShowIceCafeAmericanoNode(data);
                    break;
                /*case NodeTag.IceMocha:
                    GameEntry.Entity.ShowIceMochaNode(data);
                    break;*/
                //加糖咖啡
                case NodeTag.SweetEspresso:
                    GameEntry.Entity.ShowSweetEspressoNode(data);
                    break;
                case NodeTag.SweetWhiteCoffee:
                    GameEntry.Entity.ShowSweetWhiteCofffeeNode(data);
                    break;
                /*case NodeTag.SweetConPanna:
                    GameEntry.Entity.ShowSweetConPannaNode(data);
                    break;
                case NodeTag.SweetLatte:
                    GameEntry.Entity.ShowSweetLatteNode(data);
                    break;*/
                case NodeTag.SweetCafeAmericano:
                    GameEntry.Entity.ShowSweetCafeAmericanoNode(data);
                    break;
               /* case NodeTag.SweetMocha:
                    GameEntry.Entity.ShowSweetMochaNode(data);
                    break;*/


            }
        }
    }
}

