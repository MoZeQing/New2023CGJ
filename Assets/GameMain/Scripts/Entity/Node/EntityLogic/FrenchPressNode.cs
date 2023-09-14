using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class FrenchPressNode : CoffeeBaseCompenent, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CompenentData = (CompenentData)userData;
            m_NodeData = m_CompenentData.NodeData;
            GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);

            mSpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.FireNow(this, MaterialEventArgs.Create(m_NodeData.NodeTag, -1));
        }
    }
}
