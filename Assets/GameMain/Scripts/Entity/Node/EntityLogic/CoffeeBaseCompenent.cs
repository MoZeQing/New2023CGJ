using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class CoffeeBaseCompenent : Entity, IPointerDownHandler
    {
        public bool Follow
        {
            get;
            protected set;
        } = false;

        public AdsorbSlot M_AdsorbSlot
        {
            get;
            set;
        }

        public Transform ProgressBar
        {
            get;
            set;
        }

        public NodeTag NodeTag
        {
            get;
            private set;
        }

        public SpriteRenderer SpriteRenderer
        {
            get;
            set;
        }

        public NodeData M_NodeData
        {
            get;
            set;
        }
        public List<RecipeData> M_RecipeDatas
        {
            get;
            set;
        } = new List<RecipeData>();

        public float ProducingTime
        {
            get;
            set;
        }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CompenentData data = (CompenentData)userData;
            NodeTag = data.NodeData.NodeTag;
            SpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (!Input.GetMouseButton(0))
            {
                Follow = false;
            }
            if (Follow)
            {
                this.transform.position = MouseToWorld(Input.mousePosition);
            }
            if (M_AdsorbSlot != null)
            {
                //检测空卡片上的物体是否为空，制作途中拉开卡片可以重置时间和Bar
                
                    if (M_AdsorbSlot.Child == null)
                    {
                        ProgressBar.gameObject.SetActive(false);
                        ProducingTime = M_NodeData.ProducingTime;
                        ProgressBar.transform.SetLocalScaleX(1);
                    }
                foreach (RecipeData recipe in M_RecipeDatas)
                {
                    bool flag = true;
                    if (M_AdsorbSlot.Child.Child != null)
                        return;
                    if (!recipe.Materials.Contains(M_AdsorbSlot.Child.NodeTag))
                        flag = false;

                    if (flag)
                    {
                        ProgressBar.gameObject.SetActive(true);
                        ProgressBar.transform.SetLocalScaleX(1 - (1 - ProducingTime / M_NodeData.ProducingTime));
                        ProducingTime -= Time.deltaTime;

                        if (ProducingTime <= 0)
                        {

                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, recipe.Product)
                            {
                                Position = this.transform.position
                            });
                            BaseCompenent baseCompenent = M_AdsorbSlot.Child;
                            M_AdsorbSlot.Child = null;
                            baseCompenent.Remove();
                            ProducingTime = M_NodeData.ProducingTime;
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
            GameEntry.Sound.PlaySound("Assets/GameMain/Audio/Sounds/Pick_up.mp3", "Sound");
            Debug.LogFormat("点击事件，来源于{0}", this.gameObject.name);
            Follow = true;
        }
        /*public CoffeeBaseCompenent(AdsorbSlot adsorbSlot, Transform progressBar , SpriteRenderer spriteRenderer , float producingTime, List<RecipeData> m_RecipeDatas)
        {
            this.M_AdsorbSlot = adsorbSlot;
            this.ProgressBar = progressBar;
            this.SpriteRenderer = spriteRenderer;
            this.ProducingTime = producingTime;
            this.M_RecipeDatas = m_RecipeDatas;
        }*/
    }
}
