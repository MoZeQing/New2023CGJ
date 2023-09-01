using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class SaveGameEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SaveGameEventArgs).GetHashCode();

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
            set;
        }

        public static SaveGameEventArgs Create(SaveLoadData saveLoadData)
        {
            SaveGameEventArgs args = ReferencePool.Acquire<SaveGameEventArgs>();
            args.SaveLoadData = saveLoadData;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
