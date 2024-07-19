using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class GuideEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GuideEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int GuideID
        {
            get;
            set;
        }
        public static GuideEventArgs Create(int guideId)
        {
            GuideEventArgs args = ReferencePool.Acquire<GuideEventArgs>();
            args.GuideID = guideId;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
