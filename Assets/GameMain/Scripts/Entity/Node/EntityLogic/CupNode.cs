using DG.Tweening;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class CupNode : BaseCompenent, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private List<BoxCollider2D> m_CupBoxCollider2DList = new List<BoxCollider2D>();

        private List<BaseCompenent> m_AdsorbNodeList = new List<BaseCompenent>();

        private BaseCompenent m_AdsorbNode;
        private BaseCompenent m_AdsorbNode1;
        private BaseCompenent m_AdsorbNode2;
        private BaseCompenent m_AdsorbNode3;

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

            m_ProgressBar = this.transform.Find("ProgressBar").transform;//获取进度条
            m_ProgressBar.gameObject.SetActive(false);

            m_CupBoxCollider2DList.AddRange(this.transform.Find("Cup").GetComponents<BoxCollider2D>());

            m_AdsorbNodeList.Add(m_AdsorbNode);
            m_AdsorbNodeList.Add(m_AdsorbNode1);
            m_AdsorbNodeList.Add(m_AdsorbNode2);
            m_AdsorbNodeList.Add(m_AdsorbNode3);

        }

        private bool Contain(NodeTag nodeTag)
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
                m_ProgressBar.gameObject.SetActive(false);
                m_ProgressBar.transform.SetLocalScaleX(1);
                m_ProducingTime = 0f;
                m_AdsorbNode = null;
                m_AdsorbNode1 = null;
                m_AdsorbNode2 = null;
                m_AdsorbNode3 = null;
                Producing = false;
            }
            if (m_AdsorbNode != null || m_AdsorbNode1 != null || m_AdsorbNode2 != null || m_AdsorbNode3 != null)
            {
                Debug.Log("吸附中");
                //吸附效果
                //多个吸附点竞争时，寻找最近的吸附点吸附
                foreach (var item in m_AdsorbNodeList)
                {
                    if (item.Follow != false)
                        return;
                }

                foreach (var item in m_AdsorbNodeList)
                {
                    item.ProducingTool = NodeTag.FilterBowl;
                    item.Producing = true;
                    if (item == m_AdsorbNode)
                    {
                        item.transform.DOMove(m_CupBoxCollider2DList[0].transform.position, 0.1f);
                    }
                    if (item == m_AdsorbNode1)
                    {
                        item.transform.DOMove(m_CupBoxCollider2DList[1].transform.position, 0.1f);
                    }
                    if (item == m_AdsorbNode2)
                    {
                        item.transform.DOMove(m_CupBoxCollider2DList[0].transform.position, 0.1f);
                    }
                    if (item == m_AdsorbNode3)
                    {
                        item.transform.DOMove(m_CupBoxCollider2DList[1].transform.position, 0.1f);
                    }
                }

                if ((!(Contain(NodeTag.CoffeeLiquid)))
                    && (!(Contain(NodeTag.CoffeeLiquid) && Contain(NodeTag.Water)))
                    && (!(Contain(NodeTag.CoffeeLiquid) && Contain(NodeTag.Milk)))
                    && (!(Contain(NodeTag.CoffeeLiquid) && Contain(NodeTag.HotMilk)))
                    && (!(Contain(NodeTag.CoffeeLiquid) && Contain(NodeTag.ChocolateSyrup) && Contain(NodeTag.Cream) && Contain(NodeTag.Milk)))
                    && (!(Contain(NodeTag.CoffeeLiquid) && Contain(NodeTag.Cream))))
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
                        foreach (var item in m_AdsorbNodeList)
                        {
                            item.Producing = false;
                            item.Completed = true;
                        }
                        ShowCoffee();
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
                foreach (var item in m_CupBoxCollider2DList)
                {
                    if (!item.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    {
                        return;
                    }
                }
                if (m_CupBoxCollider2DList[0].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = baseCompenent;
                }
                if (m_CupBoxCollider2DList[1].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode1 = baseCompenent;
                }
                if (m_CupBoxCollider2DList[2].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode2 = baseCompenent;
                }
                if (m_CupBoxCollider2DList[3].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode3 = baseCompenent;
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
                foreach (var item in m_CupBoxCollider2DList)
                {
                    if (!item.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    {
                        return;
                    }
                }
                if (m_CupBoxCollider2DList[0].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = null;
                }
                if (m_CupBoxCollider2DList[1].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode1 = null;
                }
                if (m_CupBoxCollider2DList[2].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode2 = null;
                }
                if (m_CupBoxCollider2DList[3].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode3 = null;
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
        /// <summary>
        /// 根据六种情况生成咖啡
        /// </summary>
        private void ShowCoffee()
        {
            if (Contain(NodeTag.CoffeeLiquid) && (!Contain(NodeTag.Milk)) && (!Contain(NodeTag.Water)) && (!Contain(NodeTag.Cream)) && (!Contain(NodeTag.HotMilk)) && (!Contain(NodeTag.ChocolateSyrup)))
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso)
                {
                    Position = this.transform.position
                });
            }
            else if (Contain(NodeTag.CoffeeLiquid) && (!Contain(NodeTag.Milk)) && (Contain(NodeTag.Water)) && (!Contain(NodeTag.Cream)) && (!Contain(NodeTag.HotMilk)) && (!Contain(NodeTag.ChocolateSyrup)))
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CafeAmericano)
                {
                    Position = this.transform.position
                });
            }
            else if (Contain(NodeTag.CoffeeLiquid) && (Contain(NodeTag.Milk)) && (!Contain(NodeTag.Water)) && (!Contain(NodeTag.Cream)) && (!Contain(NodeTag.HotMilk)) && (!Contain(NodeTag.ChocolateSyrup)))
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.WhiteCoffee)
                {
                    Position = this.transform.position
                });
            }
            else if (Contain(NodeTag.CoffeeLiquid) && (Contain(NodeTag.Milk)) && (!Contain(NodeTag.Water)) && (Contain(NodeTag.Cream)) && (!Contain(NodeTag.HotMilk)) && (Contain(NodeTag.ChocolateSyrup)))
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Mocha)
                {
                    Position = this.transform.position
                });
            }
            else if (Contain(NodeTag.CoffeeLiquid) && (!Contain(NodeTag.Milk)) && (!Contain(NodeTag.Water)) && (!Contain(NodeTag.Cream)) && (Contain(NodeTag.HotMilk)) && (!Contain(NodeTag.ChocolateSyrup)))
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Latte)
                {
                    Position = this.transform.position
                });
            }
            else if (Contain(NodeTag.CoffeeLiquid) && (!Contain(NodeTag.Milk)) && (!Contain(NodeTag.Water)) && (Contain(NodeTag.Cream)) && (!Contain(NodeTag.HotMilk)) && (!Contain(NodeTag.ChocolateSyrup)))
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ConPanna)
                {
                    Position = this.transform.position
                });
            }
        }
    }
}