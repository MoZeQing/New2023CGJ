using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class OutEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OutEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public OutingSceneState OutingSceneState
        {
            get;
            set;
        }

        public static OutEventArgs Create(OutingSceneState outingSceneState)
        {
            OutEventArgs args = ReferencePool.Acquire<OutEventArgs>();
            args.OutingSceneState = outingSceneState;
            return args;
        }

        public override void Clear()
        {

        }
    }
}