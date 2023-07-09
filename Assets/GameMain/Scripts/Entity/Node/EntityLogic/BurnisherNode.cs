using DG.Tweening;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace GameMain
{
    public class BurnisherNode : BaseCompenent, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private BoxCollider2D m_BurnishBoxCollider2D;

        private BaseCompenent m_AdsorbNode;

        private Transform m_ProgressBar = null;
        private float m_ProducingTime = 0f;



        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CompenentData = (CompenentData)userData;
            m_NodeData = m_CompenentData.NodeData;
            GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);

            //获取到表
            IDataTable<DRNode> dtNode = GameEntry.DataTable.GetDataTable<DRNode>();
            DRNode drNode = dtNode.GetDataRow(5);

            m_NodeData.ProducingTime = 5f;
            m_ProducingTime = m_NodeData.ProducingTime;

            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];
            m_SpriteRenderer.sortingLayerName = drNode.Layer;
            m_SpriteRenderer.sortingOrder = drNode.Layerint;

            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
            m_BoxCollider2D.size = m_SpriteRenderer.size;

            m_BurnishBoxCollider2D = this.transform.Find("Burnisher").GetComponent<BoxCollider2D>();

            m_ProgressBar = this.transform.Find("ProgressBar").transform;//获取进度条
            m_ProgressBar.gameObject.SetActive(false);

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!Input.GetMouseButton(0))
            {
                Follow = false;
            }
            if (Follow)
            {
                this.transform.position = MouseToWorld(Input.mousePosition);
            }
            if (m_AdsorbNode != null)
            {
                Debug.Log("吸附中");
                //吸附效果
                //多个吸附点竞争时，寻找最近的吸附点吸附
                if (m_AdsorbNode.Follow != false)
                    return;
                if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.CoffeeBean)
                    return;

                m_AdsorbNode.ProducingTool = NodeTag.Burnisher;
                m_AdsorbNode.Producing = true;
                m_AdsorbNode.transform.DOMove(m_BurnishBoxCollider2D.transform.position, 0.1f);
                Producing = true;
                if (Producing)
                {
                    //处理进度条
                    m_ProgressBar.gameObject.SetActive(true);
                    m_ProgressBar.transform.SetLocalScaleX(1 - (1 - m_ProducingTime / m_NodeData.ProducingTime));
                    m_ProducingTime -= Time.deltaTime;

                    Debug.Log(m_ProducingTime);
                    if (m_ProducingTime <= 0)
                    {
                        Producing = false;
                        m_AdsorbNode.Producing = false;
                        m_AdsorbNode.Completed = true;
                        if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.CoffeeBean)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.GroundCoffee)
                            {
                                Position = this.transform.position
                            });
                            m_ProducingTime = m_NodeData.ProducingTime;
                        }
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            Follow = true;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("检测到碰撞");
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (!baseCompenent.Follow)
                    return;
                if (!m_BurnishBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    return;
                Debug.Log("检测到吸附");
                m_AdsorbNode = baseCompenent;
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (!baseCompenent.Follow)
                    return;
                if (m_BurnishBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    return;
                Debug.Log("检测到离开吸附");
                m_AdsorbNode = null;
            }
        }

        private Vector3 MouseToWorld(Vector3 mousePos)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.z = screenPosition.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}