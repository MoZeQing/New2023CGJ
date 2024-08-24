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
            DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)NodeData.NodeTag);
            if (dRNode.Tool)
            {
                GameEntry.Entity.ShowToolComponent(data);
            }
            else
            {
                GameEntry.Entity.ShowComponent(data);
            }
        }
    }
}

