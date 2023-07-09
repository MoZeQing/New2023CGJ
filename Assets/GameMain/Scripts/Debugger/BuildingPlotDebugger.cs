using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class BuildingPlotDebugger : MonoBehaviour
    {      
        void Start()
        {
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeBean)
            { 
                Position= new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Water)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cream)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ChocolateSyrup)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Burnisher)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Kettle)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FilterBowl)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CafeAmericano)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Kettle)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FilterBowl)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cup)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Water)
            {
                Position = new Vector3(0, -4.8f, 0)
            });
        }
    }
}
