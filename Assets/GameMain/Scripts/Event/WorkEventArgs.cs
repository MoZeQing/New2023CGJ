using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class WorkEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WorkEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string Text
        {
            get;
            set;
        }

        public static WorkEventArgs Create(string text)
        {
            WorkEventArgs args = ReferencePool.Acquire<WorkEventArgs>();
            args.Text = text;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
