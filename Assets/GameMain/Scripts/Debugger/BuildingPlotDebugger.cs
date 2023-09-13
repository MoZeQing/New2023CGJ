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
            /*GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Burnisher)
            {
                Position = new Vector3(-6, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FilterBowl)
            {
                Position = new Vector3(-2, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Kettle)
            {
                Position = new Vector3(2, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cup)
            {
                Position = new Vector3(6, -4.8f, 0)
            });
            /*GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoarseGroundCoffee)
            {
                Position = new Vector3(6, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.MidGroundCoffee)
            {
                Position = new Vector3(6, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FineGroundCoffee)
            {
                Position = new Vector3(6, -4.8f, 0)
            }); GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ManualGrinder)
            {
                Position = new Vector3(6, -4.8f, 0)
            }); GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ElectricGrinder)
            {
                Position = new Vector3(6, -4.8f, 0)
            });*/
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ElectricGrinder)
            {
                Position = new Vector3(6.073708f, -6.272577f, 0),
                Follow = false
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Kettle)
            {
                Position = new Vector3(3.926076f, -6.51359f, 0),
                Follow = false
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FilterBowl)
            {
                Position = new Vector3(-0.2130438f, -6.361422f, 0),
                Follow = false
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Stirrer)
            {
                Position = new Vector3(-3.477277f, -6.092597f, 0),
                Follow = false
            });
            /*GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso)
            {
                Position = new Vector3(-3.477277f, -6.092597f, 0),
                Follow = false
            });*/
        }
    }
}
