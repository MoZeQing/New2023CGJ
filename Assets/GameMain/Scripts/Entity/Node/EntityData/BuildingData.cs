using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;


namespace GameMain
{
    public class BuildingData : EntityData
    {
        /// <summary>
        /// 建筑的种类
        /// </summary>
        public BuildingTag BuildingTag
        {
            get;
            set;
        }

        /// <summary>
        /// 建筑的二维位置
        /// </summary>
        public Vector2 Pos 
        { 
            get; 
            set; 
        }

        public int Dorm
        {
            get;
            set;
        }

        public int Eletricity
        {
            get;
            set;
        }

        public int Garden
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Produre
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public BuildingData(int entityId,int typeId,BuildingTag builing,Vector2 pos)
            :base(entityId,typeId)
        { 
            this.BuildingTag= builing;
            Pos= pos;
        }
    }

    public enum BuildingTag
    {
        Unavailable,
        Empty,
        Elevator,
        Electricity1,
        Garden1,
        Dorm1,
        Workbench1,
        Training1,
        Electricity2,
        Garden2,
        Dorm2,
        Workbench2,
        Training2,
        Electricity3,
        Garden3,
        Workbench3,
        Training3
    }
}
