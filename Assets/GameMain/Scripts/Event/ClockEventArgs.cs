using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ClockEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ClockEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool Clock
        {
            get;
            set;
        }

        public static ClockEventArgs Create(bool clock)
        {
            ClockEventArgs args = ReferencePool.Acquire<ClockEventArgs>();
            args.Clock = clock;
            return args;
        }

        public override void Clear()
        {

        }
    }
}

