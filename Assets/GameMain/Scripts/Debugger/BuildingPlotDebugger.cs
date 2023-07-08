using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class BuildingPlotDebugger : MonoBehaviour
    {      
        void Start()
        {
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeBean));
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Burnisher));
        }
    }
}
