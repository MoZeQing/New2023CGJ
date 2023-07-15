using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class DialogEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(DialogEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string DialogTag
        {
            get;
            set;
        }

        public static DialogEventArgs Create(string dialogTag)
        {
            DialogEventArgs args = ReferencePool.Acquire<DialogEventArgs>();
            args.DialogTag = dialogTag;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
