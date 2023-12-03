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
            GameEntry.Entity.ShowComponent(data);
            //工厂模式
            //switch (NodeData.NodeTag)
            //{
            //    //原材料
            //    case NodeTag.CoffeeBean:
            //        GameEntry.Entity.ShowCoffeeBeanNode(data);
            //        break;
            //    case NodeTag.CoarseGroundCoffee:
            //        GameEntry.Entity.ShowCoarseGroundCoffeeNode(data);
            //        break;
            //    case NodeTag.MidGroundCoffee:
            //        GameEntry.Entity.ShowMidGroundCoffeeNode(data);
            //        break;
            //    case NodeTag.FineGroundCoffee:
            //        GameEntry.Entity.ShowFineGroundCoffeeNode(data);
            //        break;
            //    case NodeTag.Water:
            //        GameEntry.Entity.ShowWaterNode(data);
            //        break;
            //    case NodeTag.HotWater:
            //        GameEntry.Entity.ShowHotWaterNode(data);
            //        break;
            //    case NodeTag.Milk:
            //        GameEntry.Entity.ShowMilkNode(data);
            //        break;
            //    case NodeTag.HotMilk:
            //        GameEntry.Entity.ShowHotMilkNode(data);
            //        break;
            //    case NodeTag.Cream:
            //        GameEntry.Entity.ShowCreamNode(data);
            //        break;
            //    case NodeTag.ChocolateSyrup:
            //        GameEntry.Entity.ShowChocolateSyrupNode(data);
            //        break;
            //    case NodeTag.Ice:
            //        GameEntry.Entity.ShowIceNode(data);
            //        break;
            //    case NodeTag.Sugar:
            //        GameEntry.Entity.ShowSugarNode(data);
            //        break;
            //    case NodeTag.Salt:
            //        GameEntry.Entity.ShowSaltNode(data);
            //        break;
            //    case NodeTag.CondensedMilk:
            //        GameEntry.Entity.ShowCondensedMilkNode(data);
            //        break;
            //    case NodeTag.LowFoamingMilk:
            //        GameEntry.Entity.ShowLowFoamingMilkNode(data);
            //        break;
            //    case NodeTag.HighFoamingMilk:
            //        GameEntry.Entity.ShowHighFoamingMilkNode(data);
            //        break;
            //    //咖啡
            //    case NodeTag.Espresso:
            //        GameEntry.Entity.ShowEspressoNode(data);
            //        break;
            //    case NodeTag.HotCafeAmericano:
            //        GameEntry.Entity.ShowHotCafeAmericanoNode(data);
            //        break;
            //    case NodeTag.IceCafeAmericano:
            //        GameEntry.Entity.ShowIceCafeAmericanoNode(data);
            //        break;
            //    case NodeTag.HotLatte:
            //        GameEntry.Entity.ShowHotLatteNode(data);
            //        break;
            //    case NodeTag.IceLatte:
            //        GameEntry.Entity.ShowIceLatteNode(data);
            //        break;
            //    case NodeTag.HotMocha:
            //        GameEntry.Entity.ShowHotMochaNode(data);
            //        break;
            //    case NodeTag.IceMocha:
            //        GameEntry.Entity.ShowIceMochaNode(data);
            //        break;
            //    case NodeTag.Kapuziner:
            //        GameEntry.Entity.ShowKapuzinerNode(data);
            //        break;
            //    case NodeTag.FlatWhite:
            //        GameEntry.Entity.ShowFlatWhiteNode(data);
            //        break;
            //    //工具
            //    case NodeTag.ManualGrinder:
            //        GameEntry.Entity.ShowManualGrinderNode(data);
            //        break;
            //    case NodeTag.Extractor:
            //        GameEntry.Entity.ShowExtractorNode(data);
            //        break;
            //    case NodeTag.ElectricGrinder:
            //        GameEntry.Entity.ShowElectricGrinderNode(data);
            //        break;
            //    case NodeTag.Heater:
            //        GameEntry.Entity.ShowHeaterNode(data);
            //        break;
            //    case NodeTag.Syphon:
            //        GameEntry.Entity.ShowSyphonNode(data);
            //        break;
            //    case NodeTag.FrenchPress:
            //        GameEntry.Entity.ShowFrenchPressNode(data);
            //        break;
            //    case NodeTag.Kettle:
            //        GameEntry.Entity.ShowKettleNode(data);
            //        break;
            //    case NodeTag.FilterBowl:
            //        GameEntry.Entity.ShowFilterBowlNode(data);
            //        break;
            //    case NodeTag.Cup:
            //        GameEntry.Entity.ShowCupNode(data);
            //        break;
            //    case NodeTag.Stirrer:
            //        GameEntry.Entity.ShowStirrerNode(data);
            //        break;
                //冰咖啡
                //case NodeTag.IceEspresso:
                //    GameEntry.Entity.ShowIceEspressoNode(data);
                //    break;
                //case NodeTag.IceWhiteCoffee:
                //    GameEntry.Entity.ShowIceWhiteCoffeeNode(data);
                //    break;
                //case NodeTag.IceConPanna:
                //    GameEntry.Entity.ShowIceConPannaNode(data);
                //    break;
                //case NodeTag.IceLatte:
                //    GameEntry.Entity.ShowIceLatteNode(data);
                //    break;
                //case NodeTag.IceCafeAmericano:
                //    GameEntry.Entity.ShowIceCafeAmericanoNode(data);
                //    break;
                //case NodeTag.IceMocha:
                //    GameEntry.Entity.ShowIceMochaNode(data);
                //    break;
                ////加糖咖啡
                //case NodeTag.SweetEspresso:
                //    GameEntry.Entity.ShowSweetEspressoNode(data);
                //    break;
                //case NodeTag.SweetWhiteCoffee:
                //    GameEntry.Entity.ShowSweetWhiteCofffeeNode(data);
                //    break;
                //case NodeTag.SweetConPanna:
                //    GameEntry.Entity.ShowSweetConPannaNode(data);
                //    break;
                //case NodeTag.SweetLatte:
                //    GameEntry.Entity.ShowSweetLatteNode(data);
                //    break;
                //case NodeTag.SweetCafeAmericano:
                //    GameEntry.Entity.ShowSweetCafeAmericanoNode(data);
                //    break;
                //case NodeTag.SweetMocha:
                //    GameEntry.Entity.ShowSweetMochaNode(data);
                //    break;
            //}
        }
    }
}

