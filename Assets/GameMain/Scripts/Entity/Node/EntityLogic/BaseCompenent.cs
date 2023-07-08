using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class BaseCompenent : Entity
    {
        public bool Follow
        {
            get;
            protected set;
        } = false;

        public bool Producing
        {
            get;
            set;
        } = false;

        public bool Completed
        {
            get;
            set;
        } = false;

        public NodeTag ProducingTool
        {
            get;
            set;
        }
    }
}
