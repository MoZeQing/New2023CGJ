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

        public WorkTips WorkTips
        {
            get;
            set;
        }

        public static WorkEventArgs Create(string text,WorkTips workTips)
        {
            WorkEventArgs args = ReferencePool.Acquire<WorkEventArgs>();
            args.Text = text;
            args.WorkTips = workTips;
            return args;
        }

        public override void Clear()
        {

        }
    }

    public enum WorkTips
    { 
        None,
        Tips
    }
}
