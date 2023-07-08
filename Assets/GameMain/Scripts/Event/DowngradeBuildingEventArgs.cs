using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class DowngradeBuildingEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(DowngradeBuildingEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
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

        public static DowngradeBuildingEventArgs Create(Vector2 pos, BuildingData building)
        {
            DowngradeBuildingEventArgs args = ReferencePool.Acquire<DowngradeBuildingEventArgs>();
            args.BuildingPos = pos;
            args.BuildingData = building;
            return args;
        }

        public override void Clear()
        {

        }
    }
}

