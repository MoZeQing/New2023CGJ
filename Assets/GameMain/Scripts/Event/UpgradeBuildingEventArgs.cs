using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class UpgradeBuildingEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UpgradeBuildingEventArgs).GetHashCode();

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

        public static UpgradeBuildingEventArgs Create(Vector2 pos, BuildingData building)
        {
            UpgradeBuildingEventArgs args = ReferencePool.Acquire<UpgradeBuildingEventArgs>();
            args.BuildingPos = pos;
            args.BuildingData = building;
            return args;
        }

        public override void Clear()
        {

        }
    }
}

