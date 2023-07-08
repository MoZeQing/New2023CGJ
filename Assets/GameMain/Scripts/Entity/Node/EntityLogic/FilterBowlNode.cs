using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using GameFramework.DataTable;

namespace GameMain
{
    public class FilterBowlNode : BaseCompenent, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private BoxCollider2D m_FilterBowlBoxCollider2D;
        private BoxCollider2D m_FilterBowlBoxCollider2D2;

        private BaseCompenent m_AdsorbNode;
        private BaseCompenent m_AdsorbNode2;
        private bool m_Adsorb;

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


            DRNode drNode = dtNode.GetDataRow(10);
            m_NodeData.ProducingTime = drNode.ProducingTime;
            m_ProducingTime = m_NodeData.ProducingTime;

            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];

            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
            m_BoxCollider2D.size = m_SpriteRenderer.size;

            m_FilterBowlBoxCollider2D = this.transform.Find("Burnisher").GetComponent<BoxCollider2D>();
            m_FilterBowlBoxCollider2D2 = this.transform.Find("Burnisher").GetComponent<BoxCollider2D>();

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
                m_ProgressBar.gameObject.SetActive(false);
                m_ProgressBar.transform.SetLocalScaleX(1);
                m_ProducingTime = 0f;
                m_AdsorbNode = null;
                Producing = false;
            }
            if (m_AdsorbNode != null && m_AdsorbNode2 !=null)
            {
                Debug.Log("吸附中");
                //吸附效果
                //多个吸附点竞争时，寻找最近的吸附点吸附
                if (m_AdsorbNode.Follow != false)
                    return;
                if ((m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.GroundCoffee  && m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.HotWater)
                    || (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.HotWater && m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.GroundCoffee))
                    return;

                m_AdsorbNode2.ProducingTool = NodeTag.FilterBowl;
                m_AdsorbNode.ProducingTool = NodeTag.FilterBowl;
                m_AdsorbNode.Producing = true;
                m_AdsorbNode2.Producing = true;
                m_AdsorbNode.transform.DOMove(m_FilterBowlBoxCollider2D.transform.position, 0.1f);
                m_AdsorbNode2.transform.DOMove(m_FilterBowlBoxCollider2D2.transform.position, 0.1f);
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
                        m_AdsorbNode2.Producing = false;
                        m_AdsorbNode.Completed = true;
                        m_AdsorbNode2.Completed = true;
                        if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.Water)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.HotWater)
                            {
                                Position = this.transform.position
                            });
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
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (!baseCompenent.Follow)
                    return;
                if (!m_FilterBowlBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    return;
                if (m_FilterBowlBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = baseCompenent;
                }
                if (m_FilterBowlBoxCollider2D2.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode2 = baseCompenent;
                }
                Debug.Log("检测到吸附");
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (!baseCompenent.Follow)
                    return;
                if (m_FilterBowlBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    return;
                if (!m_FilterBowlBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = baseCompenent;
                }
                if (m_FilterBowlBoxCollider2D2.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode2 = baseCompenent;
                }
                Debug.Log("检测到离开吸附");

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
