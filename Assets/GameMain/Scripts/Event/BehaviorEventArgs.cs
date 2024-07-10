using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class BehaviorEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BehaviorEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public BehaviorTag BehaviorTag
        {
            get;
            set;
        }

        public static BehaviorEventArgs Create(BehaviorTag behaviorTag)
        {
            BehaviorEventArgs args = ReferencePool.Acquire<BehaviorEventArgs>();
            args.BehaviorTag = behaviorTag;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
