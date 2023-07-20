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

        public MainState MainState
        {
            get;
            set;
        }
        public LevelData LevelData
        {
            get;
            set;
        }

        public static LevelEventArgs Create(MainState mainState, LevelData levelData)
        {
            LevelEventArgs args = ReferencePool.Acquire<LevelEventArgs>();
            args.MainState = mainState;
            args.LevelData = levelData;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
