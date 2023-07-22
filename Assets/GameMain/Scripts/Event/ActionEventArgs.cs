using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ActionEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ActionEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ActionEventArgs Create()
        {
            ActionEventArgs args = ReferencePool.Acquire<ActionEventArgs>();
            return args;
        }

        public override void Clear()
        {

        }
    }
}
