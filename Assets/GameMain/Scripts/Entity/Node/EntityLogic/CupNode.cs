using DG.Tweening;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class CupNode : Entity, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private List<AdsorbSlot> m_AdsorbSlots = new List<AdsorbSlot>();//��λ1

        private Transform m_ProgressBar = null;
        private float m_ProducingTime = 0f;

        private List<RecipeData> m_RecipeDatas = new List<RecipeData>();

        private List<BaseCompenent> m_ChildDatas = new List<BaseCompenent>();

        private bool m_Follow = false;



        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CompenentData = (CompenentData)userData;
            m_NodeData = m_CompenentData.NodeData;
            GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);

            m_NodeData.ProducingTime = 5f;
            m_ProducingTime = m_NodeData.ProducingTime;

            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];

            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
            m_BoxCollider2D.size = m_SpriteRenderer.size;

            m_AdsorbSlots.Clear();
            m_AdsorbSlots.Add(this.transform.Find("Cup").GetComponent<AdsorbSlot>());

            m_ProgressBar = this.transform.Find("ProgressBar").transform;//��ȡ������
            m_ProgressBar.gameObject.SetActive(false);

            RecipeData recipe1 = new RecipeData();
            recipe1.Materials.Add(NodeTag.CoffeeLiquid);
            recipe1.Product = NodeTag.Espresso;
            recipe1.ProductTime = 10f;
            m_RecipeDatas.Add(recipe1);

            RecipeData recipe2 = new RecipeData();
            recipe2.Materials.Add(NodeTag.CoffeeLiquid);
            recipe2.Materials.Add(NodeTag.Water);
            recipe2.Product = NodeTag.CafeAmericano;
            recipe2.ProductTime = 10f;
            m_RecipeDatas.Add(recipe2);

            RecipeData recipe3 = new RecipeData();
            recipe3.Materials.Add(NodeTag.Espresso);
            recipe3.Materials.Add(NodeTag.Milk);
            recipe3.Product = NodeTag.WhiteCoffee;
            recipe3.ProductTime = 10f;
            m_RecipeDatas.Add(recipe3);

            RecipeData recipe4 = new RecipeData();
            recipe4.Materials.Add(NodeTag.Espresso);
            recipe4.Materials.Add(NodeTag.ChocolateSyrup);
            recipe4.Materials.Add(NodeTag.Milk);
            recipe4.Materials.Add(NodeTag.Cream);
            recipe4.Product = NodeTag.Mocha;
            recipe4.ProductTime = 10f;
            m_RecipeDatas.Add(recipe4);

            RecipeData recipe5 = new RecipeData();
            recipe5.Materials.Add(NodeTag.CoffeeLiquid);
            recipe5.Materials.Add(NodeTag.HotMilk);
            recipe5.Product = NodeTag.Latte;
            recipe5.ProductTime = 10f;
            m_RecipeDatas.Add(recipe5);

            RecipeData recipe6 = new RecipeData();
            recipe6.Materials.Add(NodeTag.Espresso);
            recipe6.Materials.Add(NodeTag.Cream);
            recipe6.Product = NodeTag.ConPanna;
            recipe6.ProductTime = 10f;
            m_RecipeDatas.Add(recipe6);
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
                m_Follow = false;
            }
            if (m_Follow)
            {
                this.transform.position = MouseToWorld(Input.mousePosition);
            }
            if (m_AdsorbSlots != null)
            {
                //检测空卡片上的物体是否为空，制作途中拉开卡片可以重置时间和Bar
                foreach (AdsorbSlot slot in m_AdsorbSlots)
                {
                    if (slot.Child == null)
                    {
                        m_ProgressBar.gameObject.SetActive(false);
                        m_ProducingTime = m_NodeData.ProducingTime;
                        m_ProgressBar.transform.SetLocalScaleX(1);
                    }
                }
                foreach (RecipeData recipe in m_RecipeDatas)
                {
                    bool flag = true;
                    //获得插槽的儿子的儿子等等
                    m_ChildDatas.Clear();
                    for (BaseCompenent child = m_AdsorbSlots[0].Child; child !=null; child =child.Child)
                    {
                        m_ChildDatas.Add(child);
                    }
                    /*foreach (AdsorbSlot slot in m_AdsorbSlots)
                    {
                        /*if (slot.Child.Child != null)
                            return;
                        if (!recipe.Materials.Contains(slot.Child.NodeTag))
                            flag = false;
                    }*/
                    if (m_ChildDatas.Count != recipe.Materials.Count)
                        return;
                    foreach (BaseCompenent materials in m_ChildDatas)
                    {
                        if (!recipe.Materials.Contains(materials.NodeTag))
                            flag = false;
                    }
                    if (flag)
                    {
                        m_ProgressBar.gameObject.SetActive(true);
                        m_ProgressBar.transform.SetLocalScaleX(1 - (1 - m_ProducingTime / m_NodeData.ProducingTime));
                        m_ProducingTime -= Time.deltaTime;

                        if (m_ProducingTime <= 0)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, recipe.Product)
                            {
                                Position = this.transform.position
                            });
                            foreach (AdsorbSlot slot in m_AdsorbSlots)
                            {
                                BaseCompenent baseCompenent;
                                /*BaseCompenent baseCompenent = slot.Child;
                                slot.Child = null;
                                baseCompenent.Remove();*/
                                for (int i = 0; i < m_ChildDatas.Count; i++)
                                {
                                    baseCompenent = m_ChildDatas[i];
                                    m_ChildDatas = null;
                                    baseCompenent.Remove();
                                }
                                m_ChildDatas.Clear();
                            }
                            m_ProducingTime = m_NodeData.ProducingTime;
                        }
                    }
                }
            }
        }
        protected Vector3 MouseToWorld(Vector3 mousePos)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.z = screenPosition.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            //Debug.LogFormat("����¼�����Դ��{1}", this.gameObject.name);
            m_Follow = true;
        }
        /*protected override void OnHide(bool isShutdown, object userData)
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
                if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.CoffeeLiquid 
                    && m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.Cream
                    && m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.ChocolateSyrup
                    && m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.GroundCoffee
                    && m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.CoffeeBean
                    && m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag != NodeTag.HotMilk)
                    return;

                m_AdsorbNode.ProducingTool = NodeTag.Cup;
                m_AdsorbNode.Producing = true;
                m_AdsorbNode.transform.DOMove(m_CupBoxCollider2D.transform.position, 0.1f);
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
                        if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.CoffeeLiquid)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso)
                            {
                                Position = this.transform.position
                            });
                            m_ProducingTime = m_NodeData.ProducingTime;
                        }
                        else if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.Cream)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Mocha)
                            {
                                Position = this.transform.position
                            });
                            m_ProducingTime = m_NodeData.ProducingTime;
                        }
                        else if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.ChocolateSyrup)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ConPanna)
                            {
                                Position = this.transform.position
                            });
                            m_ProducingTime = m_NodeData.ProducingTime;
                        }
                        else if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.CoffeeBean)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CafeAmericano)
                            {
                                Position = this.transform.position
                            });
                            m_ProducingTime = m_NodeData.ProducingTime;
                        }
                        else if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.GroundCoffee)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Latte)
                            {
                                Position = this.transform.position
                            });
                            m_ProducingTime = m_NodeData.ProducingTime;
                        }
                        else if (m_AdsorbNode.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == NodeTag.HotMilk)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.WhiteCoffee)
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
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/Pick_up.mp3", "Sound");

            Follow = true;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (!baseCompenent.Follow)
                    return;
                if (!m_CupBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
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
                if (m_CupBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
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
        }*/
    }
}