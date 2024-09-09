using DG.Tweening;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace GameMain
{
    public class BaseCompenent : Entity, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        /// <summary>
        /// 父卡牌
        /// </summary>
        public BaseCompenent Parent
        {
            get;
            set;
        }
        /// <summary>
        /// 子卡牌
        /// </summary>
        public BaseCompenent Child
        {
            get;
            set;
        } = null;
        /// <summary>
        /// 是否正在制造中
        /// </summary>
        public bool Producing
        {
            get;
            set;
        } = false;
        /// <summary>
        /// 卡牌的NodeTag
        /// </summary>
        public NodeTag NodeTag
        {
            get;
            protected set;
        }
        //基础特征
        public bool Ice
        {
            get;
            protected set;
        }
        public bool Grind
        {
            get;
            protected set;
        }
        public bool Tool
        {
            get;
            protected set;
        }
        public bool Check
        {
            get;
            protected set;
        }
        /// <summary>
        /// 卡牌是否处于锁定状态
        /// </summary>
        public bool Lock
        {
            get;
            protected set;
        }
        /// <summary>
        /// 卡牌制造过程中的原材料
        /// </summary>
        public List<NodeTag> Materials 
        {   get; 
            protected set; 
        } = new List<NodeTag>();
        //卡牌身上的组件
        protected SpriteRenderer mIconSprite = null;
        protected SpriteRenderer mBackgroundSprite = null;
        protected SpriteRenderer mBoundSprite= null;
        protected SpriteRenderer mCoverSprite = null;
        protected SpriteRenderer mHoldSprite = null;
        protected SpriteRenderer mShaderSprite = null;
        protected Image mProgressBarRenderer = null;
        protected BoxCollider2D mBoxCollider2D = null;
        protected Rigidbody2D mRigidbody2D = null;
        protected Text mTextText= null;
        //内部数据
        protected NodeData mNodeData = null;//卡牌的初始数据
        protected CompenentData mCompenentData = null;//卡牌的组件数据
        protected List<BaseCompenent> mCompenents = new List<BaseCompenent>();//储存被多个碰撞箱体碰撞时的所有碰撞箱体
        protected DRNode mDRNode = null;
        //标识组件
        protected SpriteRenderer mIcePoint = null;
        protected SpriteRenderer mGrindPoint = null;
        protected SpriteRenderer mHotPoint = null;
        protected SpriteRenderer mMediumPoint = null;
        //合成
        protected RecipeData mRecipeData;//目前的启动的配方
        protected float mProducingTime = 0f;
        protected float mTime = 0f;
        protected List<NodeTag> mChildMaterials = new List<NodeTag>();//目前的子节点的全部标签
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            NodeTag = mCompenentData.NodeData.NodeTag;
            mRigidbody2D = this.GetComponent<Rigidbody2D>();
            mBackgroundSprite = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            mProgressBarRenderer = this.transform.Find("ProgressBar").GetComponent<Image>();
            mCoverSprite = mBackgroundSprite.transform.Find("Cover").GetComponent<SpriteRenderer>();
            mTextText = mBackgroundSprite.transform.Find("Text").GetComponent<Text>();
            mHoldSprite = mBackgroundSprite.transform.Find("Mask").transform.Find("Hold").GetComponent<SpriteRenderer>();
            mIconSprite = mBackgroundSprite.transform.Find("Icon").GetComponent<SpriteRenderer>();
            mBoundSprite = mBackgroundSprite.transform.Find("Bound").GetComponent<SpriteRenderer>();
            mShaderSprite= this.transform.Find("Shader").GetComponent<SpriteRenderer>();
            mBoxCollider2D = this.GetComponent<BoxCollider2D>();

            mIcePoint= this.transform.Find("Tag").Find("Ice").GetComponent<SpriteRenderer>();
            mGrindPoint = this.transform.Find("Tag").Find("Grind").GetComponent<SpriteRenderer>();
            mHotPoint= this.transform.Find("Tag").Find("Hot").GetComponent<SpriteRenderer>();
            mMediumPoint = this.transform.Find("Tag").Find("Medium").GetComponent<SpriteRenderer>();

            GameEntry.Entity.AttachEntity(this.Id, mCompenentData.OwnerId);
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            mHoldSprite.transform.localScale = Vector3.zero;

            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;
            NodeTag = mCompenentData.NodeData.NodeTag;

            mDRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mNodeData.NodeTag);

            Lock = false;
            Grind = mNodeData.Grind;
            Ice = mDRNode.Ice;
            Tool= mDRNode.Tool;

            if (Tool)
            {
                Lock = true;
                mRigidbody2D.bodyType = RigidbodyType2D.Static;
            }

            UpdateIcon();

            Producing = false;
            mIconSprite.sprite = Resources.Load<Sprite>(mDRNode.IconPath);
            mBackgroundSprite.sprite= Resources.Load<Sprite>(mDRNode.BackgroundPath);
            mBoundSprite.sprite = Resources.Load<Sprite>(mDRNode.BoundPath);
            mTextText.text = mDRNode.Name;
            mProgressBarRenderer.gameObject.SetActive(false);

            GameEntry.Entity.AttachEntity(this.Id, mCompenentData.OwnerId);
            this.transform.position = mNodeData.Position;
            //处理特殊情况
            if (mNodeData.RamdonJump)
            {
                Vector3 newPos = UnityEngine.Random.insideUnitCircle;
                this.transform.DOMove(mNodeData.Position + newPos * 2f, 0.5f).SetEase(Ease.OutExpo);
            }
            if (mNodeData.FirstFollow)
            {
                mCoverSprite.gameObject.SetActive(true);
                ExecuteEvents.Execute<IPointerDownHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                Vector3 newPos = -(mNodeData.Position - Vector3.down * 4.2f).normalized;
            }
            if (mNodeData.Adsorb != null)
            { 
                Parent=mNodeData.Adsorb;
                mNodeData.Adsorb.Child=this;
            }
        }
        protected virtual void UpdateCard(string layerName)
        {
            mBackgroundSprite.sortingLayerName = layerName;
            mHoldSprite.GetComponent<SpriteRenderer>().sortingLayerName = layerName;
            mIconSprite.sortingLayerName = layerName;
            mTextText.GetComponent<Canvas>().sortingLayerName = layerName;
            mCoverSprite.GetComponent<SpriteRenderer>().sortingLayerName = layerName;
            mBoundSprite.sortingLayerName = layerName;

            mBackgroundSprite.sortingOrder = GameEntry.Utils.CartSort;
            mHoldSprite.GetComponent<SpriteRenderer>().sortingOrder = GameEntry.Utils.CartSort;
            mIconSprite.sortingOrder = GameEntry.Utils.CartSort;
            mTextText.GetComponent<Canvas>().sortingOrder= GameEntry.Utils.CartSort;
            mCoverSprite.GetComponent<SpriteRenderer>().sortingOrder= GameEntry.Utils.CartSort;
            mBoundSprite.sortingOrder= GameEntry.Utils.CartSort;
        }
        protected virtual void UpdateIcon()
        {
            if (GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)NodeTag).Coffee)
            {
                mIcePoint.gameObject.SetActive(Ice);
                mHotPoint.gameObject.SetActive(!Ice);
                mMediumPoint.gameObject.SetActive(Grind);
                mGrindPoint.gameObject.SetActive(!Grind);
            }
            else
            {
                mIcePoint.gameObject.SetActive(false);
                mHotPoint.gameObject.SetActive(false);
                mMediumPoint.gameObject.SetActive(false);
                mGrindPoint.gameObject.SetActive(false);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Child == null)
            {
                mBoxCollider2D.size = mBackgroundSprite.size;
                mBoxCollider2D.offset = new Vector2(0, 0.04449272f);
            }
            if (Child != null)
            {
                mBoxCollider2D.size = new Vector2(1.36f, 0.47594f);
                mBoxCollider2D.offset = new Vector2(0f, -0.7279919f);
            }
            if (!Input.GetMouseButton(0))
            {
                if (mNodeData.FirstFollow)
                {
                    ExecuteEvents.Execute<IPointerUpHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
                    mNodeData.FirstFollow = false;
                }
            }
            if (mNodeData.Follow)//跟随鼠标
            {
                this.transform.DOMove(MouseToWorld(Input.mousePosition), 0.1f);
                Producing = false;
                mProducingTime = 0;
                mTime = 0f;
                mRecipeData = null;
                mProgressBarRenderer.gameObject.SetActive(false);
                mProgressBarRenderer.fillAmount = 1;
            }
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8f, 8f), Mathf.Clamp(this.transform.position.y, -10f, -4f), 0);//���Ʒ�Χ
            if (Parent != null && !mNodeData.Follow)//跟随父卡牌
            {
                mBoxCollider2D.isTrigger = true;
                this.transform.DOMove(Parent.transform.position + Vector3.up * 0.5f, 0.1f);//�����ڵ�
            }
            //刷新子集
            mChildMaterials = GenerateMaterialList();
            Compound();
        }
        protected Vector3 MouseToWorld(Vector3 mousePos)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.z = screenPosition.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            Parent = null;
            Child = null;
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (Tool)
                return;
            mNodeData.Follow = true;

            //刷新子节点
            if (Parent != null)
            {
                Parent.Child = null;
                Parent = null;
            }
            PickUp();

            if (mDRNode.ClickSound != string.Empty)
                GameEntry.Sound.PlaySound(mDRNode.ClickSound, "Sound");
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            if (Tool)
                return;
            mNodeData.Follow = false;
            if (Parent != null)
                return;
            PutDown();
            //刷新子节点
            if (mCompenents.Count == 0)
                return;
            BaseCompenent bestCompenent = mCompenents[0];
            foreach (BaseCompenent baseCompenent in mCompenents)
            {
                if ((baseCompenent.transform.position - this.transform.position).magnitude < (bestCompenent.transform.position - this.transform.position).magnitude)
                {
                    if (baseCompenent.Child != null)
                        continue;
                    bestCompenent = baseCompenent;
                }
            }
            mCompenents.Clear();         
            BaseCompenent parent = bestCompenent;

            int block = 1000;
            while (parent != null)
            {
                parent = parent.Parent;
                if (parent == this)
                    break;
                block--;
                if (block < 0)
                    break;
            }
            if(bestCompenent.Child==null)
            {
                Parent = bestCompenent;
                Parent.Child = this;
            }
        }
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            Check = true;
            if (Tool)
                return;
            PitchOn();

            if (mDRNode.HoldSound != string.Empty)
                GameEntry.Sound.PlaySound(mDRNode.HoldSound, "Sound");
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            Check = false;
            if (Tool)
                return;
            PitchOut();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!mNodeData.Follow)
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            if (!baseCompenent.Check)
                return;
            if (!mCompenents.Contains(baseCompenent))
            {
                mCompenents.Add(baseCompenent);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!mNodeData.Follow)
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            if (Parent == baseCompenent)
            {
                Parent.Child = null;
                Parent = null;
            }
            if (mCompenents.Contains(baseCompenent))
            {
                mCompenents.Remove(baseCompenent);
            }
        }
        /// <summary>
        /// 鼠标点击效果
        /// </summary>
        protected virtual void PickUp()
        {
            //修改层级为Controller，卡牌向上移动并播放圆环特效
            UpdateCard("Controller");
            mBoxCollider2D.isTrigger = true;
            mHoldSprite.transform.localScale = Vector3.zero;
            mHoldSprite.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.2f).SetEase(Ease.OutExpo)
                .OnComplete(()=>
                {
                    mBackgroundSprite?.gameObject.transform.DOPause();
                    mBackgroundSprite?.gameObject.transform.DOLocalMove(Vector3.up * 0.08f, 0.2f);
                });
            mHoldSprite?.transform.DOScale(0.75f, 0.4f).SetEase(Ease.OutExpo);//圆环特效
            Child?.PickUp();
        }
        /// <summary>
        /// 鼠标放下效果
        /// </summary>
        protected virtual void PutDown()
        {
            UpdateCard("GamePlay");
            mBoxCollider2D.isTrigger = false;
            mBackgroundSprite?.gameObject.transform.DOPause();
            mBackgroundSprite?.gameObject.transform.DOLocalMove(Vector3.zero, 0.2f);
            mHoldSprite?.GetComponent<SpriteRenderer>().DOColor(Color.clear, 0.2f).SetEase(Ease.OutExpo);
            Child?.PutDown();
        }
        /// <summary>
        /// 鼠标进入效果
        /// </summary>
        protected virtual void PitchOn()
        {
            mCoverSprite?.gameObject.SetActive(true);
            Child?.PitchOn();
        }
        /// <summary>
        /// 鼠标退出效果
        /// </summary>
        protected virtual void PitchOut()
        {
            mCoverSprite?.gameObject.SetActive(false);
            Child?.PitchOut();
        }
        public void Remove()
        {
            if (Parent != null)
                Parent.Child = null;
            if (Child != null)
                Child.Parent = null;
            GameEntry.Entity.HideEntity(mNodeData.Id);
        }
        public void RemoveChildren()
        {
            List<BaseCompenent> mMaterialBaseCompenet = new List<BaseCompenent>();
            BaseCompenent child = Child;
            while (child != null)
            {
                mMaterialBaseCompenet.Add(child);
                child = child.Child;
            }
            for (int i = 0; i < mMaterialBaseCompenet.Count; i++)
            {
                mMaterialBaseCompenet[i].Remove();
            }
        }

        public bool GetChildGrind()
        {
            BaseCompenent child = Child;
            while (child != null)
            {
                if (child.Grind || child.NodeTag==NodeTag.EspressoG)
                    return true;
                child = child.Child;
            }
            return false;
        }
        public List<NodeTag> GenerateMaterialList()
        {
            List<NodeTag> Material = new List<NodeTag>();
            BaseCompenent child = Child;
            while (child != null)
            {
                Material.Add(child.NodeTag);
                child = child.Child;
            }
            return Material;
        }
        /// <summary>
        /// 检查队列是否相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">队列1</param>
        /// <param name="b">队列2</param>
        /// <returns></returns>
        public bool CheckList<T>(List<T> a1, List<T> b1)
        {
            List<T> list1 = new List<T>(a1);
            List<T> list2 = new List<T>(b1);
            if (list1.Count != list2.Count)
                return false;
            else
            {
                foreach (T a in list1)
                {
                    if (!list2.Remove(a)) return false;
                }
            }
            return true;
        }
        protected virtual void Compound()
        {
            //层级刷新
            mProgressBarRenderer.GetComponent<Canvas>().sortingOrder = mIconSprite.sortingOrder + 1;
            mProgressBarRenderer.GetComponent<Canvas>().sortingLayerName = mIconSprite.sortingLayerName;
            //如果不在制作中，开始检查是否开始制作
            if (!Producing)
            {
                //开始筛选配方
                foreach (DRRecipe recipe in GameEntry.DataTable.GetDataTable<DRRecipe>().GetAllDataRows())
                {
                    if (!GameEntry.Player.HasRecipe(recipe.Id))
                        continue;

                    mRecipeData = new RecipeData(recipe);
                    if (Parent == null && Child != null)
                    {
                        if (NodeTag == mRecipeData.tool)
                        {
                            //比较逻辑
                            if (CheckList<NodeTag>(mRecipeData.materials, mChildMaterials))
                            {
                                Producing = true;
                                float power = (float)(1f - ((float)GameEntry.Cat.WisdomLevel - 1f) / 6f);
                                mProducingTime = recipe.ProducingTime * power;
                                mTime = recipe.ProducingTime * power;
                                mProgressBarRenderer.gameObject.SetActive(true);
                                return;
                            }
                        }
                    }
                }
            }
            else//如果正在制作中
            {
                mProgressBarRenderer.gameObject.SetActive(true);
                mProgressBarRenderer.fillAmount = 1 - (1 - mProducingTime / mTime);
                mProducingTime -= Time.deltaTime;
                if (!CheckList<NodeTag>(mRecipeData.materials, mChildMaterials))
                {
                    mProducingTime = 0;
                    mTime = 0f;
                    mProgressBarRenderer.gameObject.SetActive(false);
                    mProgressBarRenderer.fillAmount = 1;
                    Producing = false;
                    return;
                }
                if (mProducingTime <= 0)//如果完成制作
                {
                    RemoveChildren();//删除全部的子节点
                    for (int i = 0; i < mRecipeData.products.Count; i++)
                    {
                        if (mRecipeData.products[i] == NodeTag.EspressoG)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true,
                                Grind = true
                            });
                        }
                        else
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mRecipeData.products[i])
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true,
                                Grind = GetChildGrind()
                            });
                        }
                    }
                    if (this.NodeTag == NodeTag.Cup)
                    {
                        this.Remove();
                    }
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipeData = null;
                    mProgressBarRenderer.gameObject.SetActive(false);
                    Producing = false;
                    return;
                }
            }
        }
    }
}


