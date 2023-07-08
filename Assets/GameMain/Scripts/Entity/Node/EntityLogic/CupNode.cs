using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMain
{
    public class CupNode : BaseCompenent
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CompenentData = (CompenentData)userData;
            m_NodeData = m_CompenentData.NodeData;
            GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);


        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }
    }
}
