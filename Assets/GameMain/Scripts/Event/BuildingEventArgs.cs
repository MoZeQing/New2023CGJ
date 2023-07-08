using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class BuildingEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BuildingEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool Upgrade
        {
            get;
            set;
        }

        public Vector2 BuildingPos
        {
            get;
            set;
        }

        public BuildingData BuildingData
        {
            get;
            set;
        }

        public static BuildingEventArgs Create(Vector2 pos,BuildingData building,bool upgrade)
        {
            BuildingEventArgs args = ReferencePool.Acquire<BuildingEventArgs>();
            args.BuildingPos = pos;
            args.BuildingData = building;
            args.Upgrade = upgrade;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
