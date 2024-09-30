using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class BuildingPlotDebugger : MonoBehaviour
    {      
        void Start()
        {
            if (!GameEntry.Utils.CheckFlag("trust_2"))
                return;
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cat)
            {
                Position = new Vector3(0f, -6.5f, 0f),
                Follow = false
            });
        }
    }
}
