using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class WhiteCoffeeNode : CoffeeBaseCompenent, IPointerDownHandler
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



/*protected override void OnInit(object userData)
{
    base.OnInit(userData);
    m_CompenentData = (CompenentData)userData;
    M_NodeData = m_CompenentData.NodeData;
    GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);

    M_NodeData.ProducingTime = 2.5f;
    ProducingTime = M_NodeData.ProducingTime;

    SpriteRenderer = this.GetComponent<SpriteRenderer>();
    SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)M_NodeData.NodeTag];

    m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
    m_BoxCollider2D.size = SpriteRenderer.size;

    M_AdsorbSlot = this.transform.Find("Coffee").GetComponent<AdsorbSlot>();

    ProgressBar = this.transform.Find("ProgressBar").transform;//��ȡ������
    ProgressBar.gameObject.SetActive(false);

    RecipeData recipe1 = new RecipeData();
    recipe1.Materials.Add(NodeTag.Ice);
    recipe1.Product = NodeTag.IceWhiteCoffee;
    recipe1.ProductTime = 10f;
    M_RecipeDatas.Add(recipe1);

    RecipeData recipe2 = new RecipeData();
    recipe2.Materials.Add(NodeTag.Sugar);
    recipe2.Product = NodeTag.SweetWhiteCoffee;
    recipe2.ProductTime = 10f;
    M_RecipeDatas.Add(recipe2);
}
}
}*/