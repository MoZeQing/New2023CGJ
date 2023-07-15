using DG.Tweening;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class BurnisherNode : Entity, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private List<AdsorbSlot> m_AdsorbSlots = new List<AdsorbSlot>();//��λ1

        private Transform m_ProgressBar = null;
        private float m_ProducingTime = 0f;

        private List<RecipeData> m_RecipeDatas = new List<RecipeData>();

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
            m_AdsorbSlots.Add(this.transform.Find("Burnisher").GetComponent<AdsorbSlot>());

            m_ProgressBar = this.transform.Find("ProgressBar").transform;//��ȡ������
            m_ProgressBar.gameObject.SetActive(false);

            RecipeData recipe1 = new RecipeData();
            recipe1.Materials.Add(NodeTag.CoffeeBean);
            recipe1.Product = NodeTag.GroundCoffee;
            recipe1.ProductTime = 10f;
            m_RecipeDatas.Add(recipe1);
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
                    foreach (AdsorbSlot slot in m_AdsorbSlots)
                    {
                        if (slot.Child.Child != null)
                            return;
                        if (!recipe.Materials.Contains(slot.Child.NodeTag))
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
                                BaseCompenent baseCompenent = slot.Child;
                                slot.Child = null;
                                baseCompenent.Remove();
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
            Debug.LogFormat("����¼�����Դ��{1}", this.gameObject.name);
            m_Follow = true;
        }
    }
}