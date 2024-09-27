using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ArrowEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ArrowEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public bool Enable
        {
            get;
            set;
        }
        public Vector3 ArrowPos
        {
            get;
            set;
        }
        public Vector3 ArrowRot
        {
            get;
            set;
        }
        public static ArrowEventArgs Create(bool enable)
        {
            return Create(enable, Vector3.zero, Vector3.zero);
        }
        public static ArrowEventArgs Create(bool enable,Vector3 arrowPos,Vector3 arrowRot)
        {
            ArrowEventArgs args = ReferencePool.Acquire<ArrowEventArgs>();
            args.Enable = enable;
            args.ArrowPos = arrowPos;
            args.ArrowRot = arrowRot;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
