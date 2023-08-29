using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class MainStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(MainStateEventArgs).GetHashCode();

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
        public static MainStateEventArgs Create(MainState mainState)
        {
            MainStateEventArgs args = ReferencePool.Acquire<MainStateEventArgs>();
            args.MainState = mainState;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
