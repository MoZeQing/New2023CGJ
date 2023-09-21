using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class LittleCatEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LittleCatEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static LittleCatEventArgs Create()
        {
            LittleCatEventArgs args = ReferencePool.Acquire<LittleCatEventArgs>();
            return args;
        }

        public override void Clear()
        {

        }
    }
}
