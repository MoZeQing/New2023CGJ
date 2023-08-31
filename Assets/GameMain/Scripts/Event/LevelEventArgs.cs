using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class LevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LevelEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static LevelEventArgs Create()
        {
            LevelEventArgs args = ReferencePool.Acquire<LevelEventArgs>();
            return args;
        }

        public override void Clear()
        {

        }
    }
}
