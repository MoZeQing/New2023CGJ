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
    public class BaseCompenent : Entity, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler,IPointerClickHandler
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
        //加料
        public bool Sugar
        {
            get;
            protected set;
        } = false;
        public bool CondensedMilk
        {
            get;
            protected set;
        } = false;
        public bool Salt
        {
            get;
            protected set;
        } = false;
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
        protected SpriteRenderer mSpriteRenderer = null;
        protected SpriteRenderer mShader = null;
        protected Image mProgressBarRenderer = null;
        protected BoxCollider2D mBoxCollider2D = null;
        protected Rigidbody2D mRigidbody2D = null;
        //内部数据
        protected NodeData mNodeData = null;//卡牌的初始数据
        protected CompenentData mCompenentData = null;//卡牌的组件数据
        protected Transform mProgressBar = null;//进度条
        
        protected List<BaseCompenent> mCompenents = new List<BaseCompenent>();//储存被多个碰撞箱体碰撞时的所有碰撞箱体
        protected RecipeData mRecipeData;//目前的启动的配方
        protected List<NodeTag> mChildMaterials = new List<NodeTag>();//目前的子节点的全部标签
        protected NodeTag tool = NodeTag.None;
        protected float mProducingTime = 0f;
        protected float mTime = 0f;
        protected DRRecipe drRecipe = null;
        //标识的组件
        private SpriteRenderer mCondensedMilkPoint = null;
        private SpriteRenderer mSugarPoint = null;
        private SpriteRenderer mSaltPoint = null;
        //标识组件
        private SpriteRenderer mIcePoint = null;
        private SpriteRenderer mGrindPoint = null;
        private SpriteRenderer mHotPoint = null;
        private SpriteRenderer mMediumPoint = null;

        private float mAddMaterialsTime = 0f;
        private float mAddTime = 0f;
        private bool flag = false;
        private bool isCoffee = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;
            NodeTag = mCompenentData.NodeData.NodeTag;
            mRigidbody2D = this.GetComponent<Rigidbody2D>();
            mSpriteRenderer = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader = this.transform.Find("Shader").GetComponent<SpriteRenderer>();
            mProgressBar = this.transform.Find("ProgressBar").GetComponent<Transform>();
            mProgressBarRenderer = this.transform.Find("ProgressBar").GetComponent<Image>();

            mBoxCollider2D = this.GetComponent<BoxCollider2D>();

            mCondensedMilkPoint = mSpriteRenderer.gameObject.transform.Find("CondensedMilk").GetComponent<SpriteRenderer>();
            mSugarPoint = mSpriteRenderer.gameObject.transform.Find("Sugar").GetComponent<SpriteRenderer>();
            mSaltPoint = mSpriteRenderer.gameObject.transform.Find("Salt").GetComponent<SpriteRenderer>();

            mIcePoint= mSpriteRenderer.gameObject.transform.Find("Ice").GetComponent<SpriteRenderer>();
            mGrindPoint = mSpriteRenderer.gameObject.transform.Find("Grind").GetComponent<SpriteRenderer>();
            mHotPoint= mSpriteRenderer.gameObject.transform.Find("Hot").GetComponent<SpriteRenderer>();
            mMediumPoint = mSpriteRenderer.gameObject.transform.Find("Medium").GetComponent<SpriteRenderer>();

            mAddMaterialsTime = 5f;

            Materials = mNodeData.Materials;
            GameEntry.Entity.AttachEntity(this.Id, mCompenentData.OwnerId);
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;
            NodeTag = mCompenentData.NodeData.NodeTag;

            Lock = false;
            Salt = false;
            Sugar= false;
            Grind = mNodeData.Grind;
            Ice = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mNodeData.NodeTag).Ice;
            CondensedMilk = false;

            UpdateIcon();

            Producing = false;
            mSpriteRenderer.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mNodeData.NodeTag).SpritePath);
            mProgressBarRenderer.gameObject.SetActive(false);

            mCondensedMilkPoint.gameObject.SetActive(false);
            mSugarPoint.gameObject.SetActive(false);
            mSaltPoint.gameObject.SetActive(false);
            GameEntry.Entity.AttachEntity(this.Id, mCompenentData.OwnerId);
            this.transform.position = mNodeData.Position;
            //处理特殊情况
            if (mNodeData.RamdonJump)
            {
                Vector3 newPos = UnityEngine.Random.insideUnitCircle;
                this.transform.DOMove(mNodeData.Position + newPos * 2f, 0.5f).SetEase(Ease.OutExpo);
            }
            if (mNodeData.Jump)
            {
                ExecuteEvents.Execute<IPointerDownHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                Vector3 newPos = -(mNodeData.Position - Vector3.down * 4.2f).normalized;
            }
            if (mNodeData.Adsorb != null)
            { 
                Parent=mNodeData.Adsorb;
                mNodeData.Adsorb.Child=this;
                mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
                mSpriteRenderer.sortingLayerName = "GamePlay";
            }
        }

        private void UpdateIcon()
        {
            if (mNodeData.IsCoffee)
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
            if (Parent != null)
            {
                mBoxCollider2D.isTrigger = true;

            }
            else
            {
                if (!GameEntry.Utils.PickUp)
                    mBoxCollider2D.isTrigger = false;
            }
            if (Child == null)
            {
                mBoxCollider2D.size = mSpriteRenderer.size;
                mBoxCollider2D.offset = new Vector2(0, 0.04449272f);
            }
            if (Child != null)
            {
                mBoxCollider2D.size = new Vector2(1.36f, 0.47594f);
                mBoxCollider2D.offset = new Vector2(0f, -0.7279919f);
            }
            if (!Input.GetMouseButton(0))
            {
                mNodeData.Follow = false;
                GameEntry.Utils.PickUp = false;
                if (mNodeData.Jump)
                {
                    ExecuteEvents.Execute<IPointerUpHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
                    mNodeData.Jump = false;
                }
            }
            if (mNodeData.Follow)//跟随鼠标
            {
                this.transform.DOMove(MouseToWorld(Input.mousePosition), 0.05f);
                Producing = false;
                tool = NodeTag.None;
                mProducingTime = 0;
                mTime = 0f;
                mRecipeData = null;
                mProgressBar.gameObject.SetActive(false);
                mProgressBar.GetComponent<Image>().fillAmount = 1;
            }
            //范围限制
            if (GameEntry.Utils.isClose)
            {
                BoundData boundData = GameEntry.Utils.closeBoundData;
                this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, boundData.leftLimit, boundData.rightLimit), Mathf.Clamp(this.transform.position.y, boundData.downLimit, boundData.upLimit), 0);//���Ʒ�Χ
                //this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8f, 8f), Mathf.Clamp(this.transform.position.y, -10f, -4f), 0);//���Ʒ�Χ
            }
            else
            {
                BoundData boundData = GameEntry.Utils.openBoundData;
                this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, boundData.leftLimit, boundData.rightLimit), Mathf.Clamp(this.transform.position.y, boundData.downLimit, boundData.upLimit), 0);//���Ʒ�Χ
            }
            if (Parent != null && !mNodeData.Follow)//跟随父卡牌
            {
                this.transform.DOMove(Parent.transform.position + Vector3.up * 0.5f, 0.1f);//�����ڵ�
            }
            //if (Parent == null && !mNodeData.Follow)//如果没有
            //{
            //    this.transform.DOMove(Parent.transform.position + Vector3.up * 0.5f, 0.1f);//�����ڵ�
            //}
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
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            GameEntry.Sound.PlaySound("Assets/GameMain/Audio/Sounds/Pick_up.mp3", "Sound");

            if (Parent != null)
            {
                Parent.Child = null;
                Parent = null;
            }
            mNodeData.Follow = true;
            GameEntry.Utils.PickUp = true;
            mBoxCollider2D.isTrigger = true;
            mShader.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "Controller";
            mShader.sortingLayerName = "Controller";

            PickUp();
        }
        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            Parent = null;
            Child = null;
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader.sortingLayerName = "GamePlay";
            mBoxCollider2D.isTrigger = true;
            PitchOn();
            if (mCompenents.Count == 0)
                return;
            if (Parent != null)
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
                    return;
                block--;
                if (block < 0)
                    return;
            }
            if(bestCompenent.Child==null)
            {
                Parent = bestCompenent;
                Parent.Child = this;
            }
           
        }
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.PickUp)
                return;
            if (Parent != null)
                return;
            PitchOn();
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.PickUp)
                return;
            if (Parent != null)
                return;
            PutDown();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!mNodeData.Follow)
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
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
        /// ����״̬
        /// </summary>
        public void PickUp()
        {
            mSpriteRenderer.gameObject.transform.DOPause();
            mShader.gameObject.transform.DOPause();
            mSpriteRenderer.gameObject.transform.DOLocalMove(Vector3.up * 0.16f, 0.2f);
            mShader.gameObject.transform.DOLocalMove(Vector3.down * 0.08f, 0.2f);
            mShader.sortingOrder = -99;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "Controller";
            mShader.sortingLayerName = "Controller";
            if (Child != null)
                Child.PickUp();
        }
        /// <summary>
        /// ѡ��
        /// </summary>
        public void PitchOn()
        {
            mSpriteRenderer.gameObject.transform.DOPause();
            mShader.gameObject.transform.DOPause();
            mSpriteRenderer.gameObject.transform.DOLocalMove(Vector3.up * 0.08f, 0.2f);
            mShader.gameObject.transform.DOLocalMove(Vector3.down * 0.04f, 0.2f);
            mShader.sortingOrder = -99;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader.sortingLayerName = "GamePlay";
            if (Child != null)
                Child.PitchOn();
        }
        /// <summary>
        /// ����
        /// </summary>
        public void PutDown()
        {
            mSpriteRenderer.gameObject.transform.DOPause();
            mShader.gameObject.transform.DOPause();
            mSpriteRenderer.gameObject.transform.DOLocalMove(Vector3.zero, 0.016f);
            mShader.gameObject.transform.DOLocalMove(Vector3.zero, 0.08f);
            mShader.sortingOrder = -99;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader.sortingLayerName = "GamePlay";
            if (Child != null)
                Child.PutDown();
        }
        public void Remove()
        {
            if (Parent != null)
                Parent.Child = null;
            if (Child != null)
                Child.Parent = null;
            GameEntry.Entity.HideEntity(mNodeData.Id);
        }
        public NodeTag TransToEnum(string value)
        {
            return (NodeTag)Enum.Parse(typeof(NodeTag), value);
        }

        public List<NodeTag> TransToEnumList(List<string> valueList)
        {
            List<NodeTag> temp = new List<NodeTag>();
            foreach (var VarIAble in valueList)
            {
                temp.Add((NodeTag)Enum.Parse(typeof(NodeTag), VarIAble));
            }
            return temp;
        }
        public void Compound()
        {
            //层级刷新
            mProgressBarRenderer.GetComponent<Canvas>().sortingOrder = mSpriteRenderer.sortingOrder + 1;
            mProgressBarRenderer.GetComponent<Canvas>().sortingLayerName = mSpriteRenderer.sortingLayerName;
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
                            if (CheckList<NodeTag>(mRecipeData.materials,mChildMaterials))
                            {
                                Producing = true;
                                drRecipe = recipe;
                                float power = (float)(1f-((float)GameEntry.Utils.CharData.WisdomLevel - 1f) / 6f);
                                mProducingTime = drRecipe.ProducingTime * power;
                                mTime = drRecipe.ProducingTime;
                                isCoffee = drRecipe.IsCoffee;
                                mProgressBarRenderer.gameObject.SetActive(true);
                                return;
                            }
                        }
                    }
                }
            }
            else//如果正在制作中
            {
                mProgressBar.gameObject.SetActive(true);
                mProgressBar.GetComponent<Image>().fillAmount=1 - (1 - mProducingTime / mTime);
                mProducingTime -= Time.deltaTime;
                if (!CheckList<NodeTag>(mRecipeData.materials, mChildMaterials))
                {
                    tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mProgressBar.gameObject.SetActive(false);
                    mProgressBar.GetComponent<Image>().fillAmount=1;
                    Producing = false;
                    return;
                }

                if (mProducingTime <= 0)//如果完成制作
                {
                    Grind = GetChildGrind();
                    if (Child != null)//删除全部的子节点
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
                    }//根据完成品进行产出
                    for (int i = 0; i < mRecipeData.products.Count; i++)
                    {
                        if (mRecipeData.products[i] == NodeTag.Espresso)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso,true)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true,
                                Grind = false
                            });
                        }
                        else if (mRecipeData.products[i]==NodeTag.EspressoG)
                        {
                            Grind = true;
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso,true)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true,
                                Grind = true
                            });
                        }
                        else if (mRecipeData.IsCoffee)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mRecipeData.products[i], true)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true,
                                Grind = this.Grind,
                            });
                        }
                        else
                        {
                            //if (mRecipeData.products[i] == NodeTag.CoarseGroundCoffee ||
                            //    mRecipeData.products[i] == NodeTag.MidGroundCoffee ||
                            //    mRecipeData.products[i] == NodeTag.LowFoamingMilk)
                            //{
                            //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mRecipeData.products[i])
                            //    {
                            //        Position = this.transform.position + new Vector3(0.5f, 0, 0),
                            //        Adsorb = this,
                            //        Grind = this.Grind
                            //    });
                            //}
                            //else
                            //{
                                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mRecipeData.products[i])
                                {
                                    Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                    RamdonJump = true,
                                    Grind = this.Grind
                                });
                            //}
                        }
                    }
                    Materials.Clear();
                    if (this.NodeTag == NodeTag.Cup)
                    {
                        this.Remove();
                    }
                    tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipeData = null;
                    mProgressBar.gameObject.SetActive(false);
                    Producing = false;
                    return;

                }
            }
        }
        public bool GetChildGrind()
        {
            BaseCompenent child = Child;
            while (child != null)
            {
                if (child.Grind)
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
        #region
        //public void AddMaterials()
        //{
        //    if(Child!=null)
        //    {
        //        mCondensedMilkPoint.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
        //        mSaltPoint.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
        //        mSugarPoint.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
        //    }
        //    if(Child==null)
        //    {
        //        mCondensedMilkPoint.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        //        mSaltPoint.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        //        mSugarPoint.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        //    }
        //    mCondensedMilkPoint.sortingLayerName = mSpriteRenderer.sortingLayerName;
        //    mSaltPoint.sortingLayerName = mSpriteRenderer.sortingLayerName;
        //    mSugarPoint.sortingLayerName = mSpriteRenderer.sortingLayerName;
        //    if (Parent == null && Child != null && Child.Child == null && flag == false)
        //    {
        //        if (Child.NodeTag == NodeTag.CondensedMilk)
        //        {
        //            flag = true;
        //            CondensedMilk = true;
        //            mAddTime = mAddMaterialsTime;
        //        }
        //        else if (Child.NodeTag == NodeTag.Sugar)
        //        {
        //            flag = true;
        //            Sugar = true;
        //            mAddTime = mAddMaterialsTime;
        //        }
        //        else if (Child.NodeTag == NodeTag.Salt)
        //        {
        //            flag = true;
        //            Salt = true;
        //            mAddTime = mAddMaterialsTime;
        //        }
        //    }
        //    if (flag == true)
        //    {
        //        if (CondensedMilk ==true&&!mCondensedMilkPoint.gameObject.activeSelf)
        //        {
        //            mProgressBar.gameObject.SetActive(true);
        //            mProgressBar.GetComponent<Image>().fillAmount=1 - (1 - mAddTime / mAddMaterialsTime);
        //            mAddTime -= Time.deltaTime;
        //            if (Parent != null || Child == null||Child.Child!=null)
        //            {
        //                flag = false;
        //                CondensedMilk = false;
        //                mProgressBar.gameObject.SetActive(false);
        //                mProgressBar.GetComponent<Image>().fillAmount=1;
        //                return;
        //            }
        //            if (mAddTime <= 0)
        //            {
        //                mProgressBar.gameObject.SetActive(false);
        //                mCondensedMilkPoint.gameObject.SetActive(true);
        //                if (Child != null)
        //                {
        //                    Child.Remove();
        //                }
        //                CondensedMilk = true;
        //                flag = false;
        //            }
        //        }
        //        else if (Sugar==true&&!mSugarPoint.gameObject.activeSelf)
        //        {
        //            mProgressBar.gameObject.SetActive(true);
        //            mProgressBar.GetComponent<Image>().fillAmount=1 - (1 - mAddTime / mAddMaterialsTime);
        //            mAddTime -= Time.deltaTime;
        //            if (Parent != null || Child == null || Child.Child != null)
        //            {
        //                flag = false;
        //                Sugar = false; ;
        //                mProgressBar.gameObject.SetActive(false);
        //                mProgressBar.GetComponent<Image>().fillAmount = 1;
        //                return;
        //            }
        //            if (mAddTime <= 0)
        //            {
        //                mProgressBar.gameObject.SetActive(false);
        //                mSugarPoint.gameObject.SetActive(true);
        //                if (Child != null)
        //                {
        //                    Child.Remove();
        //                }
        //                Sugar = true;
        //                flag = false;
        //            }
        //        }
        //        else if (Salt == true && !mSaltPoint.gameObject.activeSelf)
        //        {
        //            mProgressBar.gameObject.SetActive(true);
        //            mProgressBar.GetComponent<Image>().fillAmount = 1 - (1 - mAddTime / mAddMaterialsTime);
        //            mAddTime -= Time.deltaTime;
        //            if (Parent != null || Child == null || Child.Child != null)
        //            {
        //                flag = false;
        //                Salt = false; ;
        //                mProgressBar.gameObject.SetActive(false);
        //                mProgressBar.GetComponent<Image>().fillAmount = 1;
        //                return;
        //            }
        //            if (mAddTime <= 0)
        //            {
        //                mProgressBar.gameObject.SetActive(false);
        //                mSaltPoint.gameObject.SetActive(true);
        //                if (Child != null)
        //                {
        //                    Child.Remove();
        //                }
        //                Salt = true;
        //                flag = false;
        //            }
        //        }
        //    }
        //}
        #endregion
        /// <summary>
        /// 检查队列是否相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">队列1</param>
        /// <param name="b">队列2</param>
        /// <returns></returns>
        public bool CheckList<T>(List<T> a1, List<T> b1)
        {
            List<T> list1= new List<T>(a1);
            List<T> list2= new List<T>(b1);
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
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Lock = !Lock;
                if (Lock)
                {
                    mRigidbody2D.bodyType = RigidbodyType2D.Static;
                    mSpriteRenderer.color = Color.red;
                } 
                else
                {
                    mRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    mSpriteRenderer.color = Color.white;
                } 
            }
        }
    }

    [System.Serializable]
    public class BoundData
    {
        public float upLimit;
        public float downLimit;
        public float leftLimit;
        public float rightLimit;
    }
    public enum NodeState
    {
        //δ����
        InActive,
        //����
        Idle,
        //������
        PickUp,
        //��ѡ��
        PitchOn
    }
}


