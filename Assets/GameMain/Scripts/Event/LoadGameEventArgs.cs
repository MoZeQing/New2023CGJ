using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class LoadGameEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadGameEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public SaveLoadData SaveLoadData
        {
            get;
            private set;
        }

        public static LoadGameEventArgs Create(SaveLoadData saveLoadData)
        {
            LoadGameEventArgs args = ReferencePool.Acquire<LoadGameEventArgs>();
            args.SaveLoadData = saveLoadData;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
