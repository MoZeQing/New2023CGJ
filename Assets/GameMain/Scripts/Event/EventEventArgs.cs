using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class EventEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EventEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static EventEventArgs Create()
        {
            EventEventArgs args = ReferencePool.Acquire<EventEventArgs>();
            return args;
        }

        public override void Clear()
        {

        }
    }
}
