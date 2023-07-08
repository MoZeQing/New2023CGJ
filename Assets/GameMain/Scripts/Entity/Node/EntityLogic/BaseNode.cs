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
                case NodeTag.Burnisher:
                    GameEntry.Entity.ShowBurnisherNode(data);
                    break;
                case NodeTag.Cup:
                    GameEntry.Entity.ShowCupNode(data);
                    break;
                case NodeTag.Espresso:
                    GameEntry.Entity.ShowEspressoNode(data);
                    break;
                case NodeTag.FilterBowl:
                    break;
                case NodeTag.GroundCoffee:
                    GameEntry.Entity.ShowGroundCoffeeNode(data);
                    break;
                case NodeTag.Kettle:
                    break;
                case NodeTag.Milk:
                    break;
                case NodeTag.Sugar:
                    break;
                case NodeTag.Water:
                    GameEntry.Entity.ShowWaterNode(data);
                    break;
            }
        }
    }
}

