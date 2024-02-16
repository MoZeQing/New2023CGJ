using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;


namespace GameMain
{
    public class LoadingEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadingEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static LoadingEventArgs Create()
        {
            LoadingEventArgs args = ReferencePool.Acquire<LoadingEventArgs>();
            return args;
        }

        public override void Clear()
        {

        }
    }
}
