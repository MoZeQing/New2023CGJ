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

        public NodeTag NodeTag
        {
            get;
            private set;
        }

        public NodeTag ProducingTool
        {
            get;
            set;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CompenentData data = (CompenentData)userData;
            NodeTag = data.NodeData.NodeTag;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(this.transform.position.y, -8f, -1.6f), 0);
        }
    }
}
