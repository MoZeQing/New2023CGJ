using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{ 
    public class MilkNode : BaseCompenent, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CompenentData = (CompenentData)userData;
            m_NodeData = m_CompenentData.NodeData;
            GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);

            //��ȡ����
            IDataTable<DRNode> dtNode = GameEntry.DataTable.GetDataTable<DRNode>();
            DRNode drNode = dtNode.GetDataRow(3);

            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];
            m_SpriteRenderer.sortingLayerName = drNode.Layer;
            m_SpriteRenderer.sortingOrder = drNode.Layerint;

            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
            m_BoxCollider2D.size = m_SpriteRenderer.size;

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.FireNow(this, MaterialEventArgs.Create(m_NodeData.NodeTag, -1));
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Completed)
            {
                Completed = false;
                GameEntry.Entity.HideEntity(m_NodeData.Id);
            }
        }
    }
}
