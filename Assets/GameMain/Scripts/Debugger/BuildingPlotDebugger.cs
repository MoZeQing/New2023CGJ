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
            //foreach (ItemTag itemTag in GameEntry.SaveLoad.glassItemDatas)
            //{
            //    if (GameEntry.Player.GetPlayerItem(itemTag) != null)
            //    {
            //        if (!GameEntry.Player.GetPlayerItem(itemTag).equiping)
            //            continue;
            //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, (NodeTag)(int)itemTag)
            //        {
            //            Position = new Vector3(6.073708f, -6.272577f, 0),
            //            Follow = false
            //        });
            //    }
            //}

            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, (NodeTag)(int)GameEntry.Utils.PlayerData.heaterID)
            {
                Position = new Vector3(6.073708f, -6.272577f, 0),
                Follow = false
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, (NodeTag)(int)GameEntry.Utils.PlayerData.stirrerID)
            {
                Position = new Vector3(6.073708f, -6.272577f, 0),
                Follow = false
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, (NodeTag)(int)GameEntry.Utils.PlayerData.pressID)
            {
                Position = new Vector3(6.073708f, -6.272577f, 0),
                Follow = false
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, (NodeTag)(int)GameEntry.Utils.PlayerData.burnisherID)
            {
                Position = new Vector3(6.073708f, -6.272577f, 0),
                Follow = false
            });
        }
    }
}
