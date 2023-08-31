using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class OrderEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OrderEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public OrderData OrderData
        {
            get;
            set;
        }

        public int Income
        {
            get;
            set;
        }

        public static OrderEventArgs Create(OrderData orderData,int income)
        {
            OrderEventArgs args = ReferencePool.Acquire<OrderEventArgs>();
            args.OrderData = orderData;
            args.Income = income;
            return args;
        }

        public override void Clear()
        {

        }
    }
}