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
            foreach (ItemTag itemTag in GameEntry.SaveLoad.glassItemDatas)
            {
                if (GameEntry.Utils.GetPlayerItem(itemTag) != null)
                {
                    if (!GameEntry.Utils.GetPlayerItem(itemTag).equiping)
                        continue;
                    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, (NodeTag)(int)itemTag)
                    {
                        Position = new Vector3(6.073708f, -6.272577f, 0),
                        Follow = false
                    });
                }
            }

            //if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.ElectricGrinder)!= null)
            //{
            //    if (!GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.ElectricGrinder).equiping)
            //        return;
            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ElectricGrinder)
            //    {
            //        Position = new Vector3(6.073708f, -6.272577f, 0),
            //        Follow = false
            //    });
            //}

            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ManualGrinder)
            //    {
            //        Position = new Vector3(6.073708f, -6.272577f, 0),
            //        Follow = false
            //    });


            //if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.Extractor) != null)
            //{
            //    if (!GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.Extractor).equiping)
            //        return;
            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Extractor)
            //    {
            //        Position = new Vector3(6.073708f, -6.272577f, 0),
            //        Follow = false
            //    });
            //}

            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Kettle)
            //    {
            //        Position = new Vector3(3.926076f, -6.51359f, 0),
            //        Follow = false
            //    });

            //if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.FilterBowl) != null)
            //{
            //    if (!GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.FilterBowl).equiping)
            //        return;
            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FilterBowl)
            //    {
            //        Position = new Vector3(-0.2130438f, -6.361422f, 0),
            //        Follow = false
            //    });
            //}

            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Stirrer)
            //    {
            //        Position = new Vector3(-3.477277f, -6.092597f, 0),
            //        Follow = false
            //    });

            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Heater)
            //    {
            //        Position = new Vector3(-3.477277f, -6.092597f, 0),
            //        Follow = false
            //    });

            //if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.Syphon) != null)
            //{
            //    if (!GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.Syphon).equiping)
            //        return;
            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Syphon)
            //    {
            //        Position = new Vector3(-3.477277f, -6.092597f, 0),
            //        Follow = false
            //    });
            //}

            //if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.FrenchPress) != null)
            //{
            //    if (!GameEntry.Utils.GetPlayerItem((ItemTag)(int)NodeTag.FrenchPress).equiping)
            //        return;
            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FrenchPress)
            //    {
            //        Position = new Vector3(-3.477277f, -6.092597f, 0),
            //        Follow = false
            //    });
            //}
        }
    }
}
