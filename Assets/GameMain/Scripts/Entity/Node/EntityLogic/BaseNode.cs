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
            DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)NodeData.NodeTag);
            CompenentData data = new CompenentData(GameEntry.Entity.GenerateSerialId(), dRNode.Tool? 10012:10001,this.Id, NodeData);
            if ((NodeTag)dRNode.Id == NodeTag.Cat)
            {
                GameEntry.Entity.ShowCatComponent(data);
            }
            else if (dRNode.Tool)
            {
                switch ((NodeTag)dRNode.Id)
                {
                    case NodeTag.FrenchPress:
                        GameEntry.Entity.ShowPressComponent(data);
                        break;
                    default:
                        GameEntry.Entity.ShowToolComponent(data);
                        break;
                }
            }
            else
            {
                GameEntry.Entity.ShowComponent(data);
            }
        }
    }
}

