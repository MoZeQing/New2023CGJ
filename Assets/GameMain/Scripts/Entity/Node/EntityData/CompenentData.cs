using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CompenentData : AccessoryObjectData
    {
        public NodeData NodeData
        {
            get;
            set;
        }

        public CompenentData(int entityId, int typeId, int ownerId, NodeData nodeData)
            : base(entityId, typeId, ownerId)
        { 
            NodeData= nodeData;
        }
    }
}
