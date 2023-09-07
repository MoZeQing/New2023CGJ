using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class MainFormEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(MainFormEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public MainFormTag MainFormTag
        {
            get;
            set;
        }

        public static MainFormEventArgs Create(MainFormTag tag)
        {
            MainFormEventArgs args = ReferencePool.Acquire<MainFormEventArgs>();
            args.MainFormTag = tag;
            return args;
        }

        public override void Clear()
        {

        }
    }

    public enum MainFormTag
    { 
        Lock,
        Unlock
    }
}
