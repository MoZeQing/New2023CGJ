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

        private List<BoxCollider2D> m_FilterBoxCollider2DList = new List<BoxCollider2D>(3);

        private List<BaseCompenent> m_AdsorbNodeList = new List<BaseCompenent>(3);

        private BaseCompenent m_AdsorbNode;
        private BaseCompenent m_AdsorbNode1;

        private BoxCollider2D m_FilterBoxCollider2D;
        private BoxCollider2D m_FilterBoxCollider2D1;

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
            DRNode drNode = dtNode.GetDataRow(7);

            m_NodeData.ProducingTime = 5f;
            m_ProducingTime = m_NodeData.ProducingTime;

            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];
            m_SpriteRenderer.sortingLayerName = drNode.Layer;
            m_SpriteRenderer.sortingOrder = drNode.Layerint;

            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
            m_BoxCollider2D.size = m_SpriteRenderer.size;

            m_ProgressBar = this.transform.Find("ProgressBar").transform;//获取进度条
            m_ProgressBar.gameObject.SetActive(false);

            m_AdsorbNodeList.Add(m_AdsorbNode);
            m_AdsorbNodeList.Add(m_AdsorbNode1);

            m_FilterBoxCollider2D = this.transform.Find("FliterBowl").GetComponent<BoxCollider2D>();
            Debug.Log(m_FilterBoxCollider2D);
            m_FilterBoxCollider2D1 = this.transform.Find("AnotherFliterBowl").GetComponent<BoxCollider2D>();
            Debug.Log(m_FilterBoxCollider2D1);

            m_FilterBoxCollider2DList.Add(m_FilterBoxCollider2D);
            m_FilterBoxCollider2DList.Add(m_FilterBoxCollider2D1);
        }

        private bool Contain (NodeTag nodeTag)
        {
            foreach (BaseCompenent item in m_AdsorbNodeList)
            {
                if (item.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == nodeTag)
                    return true;
            }
            return false;
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
            if (m_AdsorbNode != null || m_AdsorbNode1 !=null)
            {
                Debug.Log("过滤杯吸附中");
                //吸附效果
                //多个吸附点竞争时，寻找最近的吸附点吸附
                ReLoad();
                foreach (var item in m_AdsorbNodeList)
                {
                    if (item.Follow != false)
                        return;
                }
                ReLoad();
                foreach (var item in m_AdsorbNodeList)
                {
                    item.ProducingTool = NodeTag.FilterBowl;
                    item.Producing = true;
                    if (item == m_AdsorbNode)
                    {
                        FilReLoad();
                        item.transform.DOMove(m_FilterBoxCollider2D.transform.position, 0.1f);
                    }
                    if (item == m_AdsorbNode1)
                    {
                        FilReLoad();
                        item.transform.DOMove(m_FilterBoxCollider2D1.transform.position, 0.1f);
                    }
                }

                if (!(Contain(NodeTag.HotWater)&&Contain(NodeTag.GroundCoffee)))
                    return;

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
                        ReLoad();
                        foreach (var item in m_AdsorbNodeList)
                        {
                            item.Producing = false;
                            item.Completed = true;
                        }
                        if (Contain(NodeTag.HotWater)||Contain(NodeTag.GroundCoffee))
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeLiquid)
                            {
                                Position = this.transform.position
                            });
                            m_ProducingTime = m_NodeData.ProducingTime;
                            ReLoad();
                            m_AdsorbNode = null;
                            m_AdsorbNode1 = null;
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
                /*foreach (var item in m_FilterBoxCollider2DList)
                {
                    if (!item.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    {
                        return;
                    }
                }*/
                Debug.Log(m_FilterBoxCollider2DList[0]);
                FilReLoad();
                if (m_FilterBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = baseCompenent;
                    return;
                }
                if (m_FilterBoxCollider2D1.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode1 = baseCompenent;
                    return;
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
                /*foreach (var item in m_FilterBoxCollider2DList)
                {
                    if (item.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    {
                        return;
                    }
                }*/
                FilReLoad();
                Debug.Log(m_FilterBoxCollider2D);
                if (m_FilterBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = null;
                    return;
                }
                if (m_FilterBoxCollider2D1.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode1 = null;
                    return;
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
        private void ReLoad()
        {
            m_AdsorbNodeList.Clear();
            m_AdsorbNodeList.Add(m_AdsorbNode);
            m_AdsorbNodeList.Add(m_AdsorbNode1);
        }
        private void FilReLoad()
        {
            m_FilterBoxCollider2DList.Clear();
            m_FilterBoxCollider2DList.Add(m_FilterBoxCollider2D);
            m_FilterBoxCollider2DList.Add(m_FilterBoxCollider2D1);
        }
    }
}
