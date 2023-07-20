using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ChangeEventArgs Create()
        {
            ChangeEventArgs args = ReferencePool.Acquire<ChangeEventArgs>();
            return args;
        }

        public override void Clear()
        {

        }
    }
}
