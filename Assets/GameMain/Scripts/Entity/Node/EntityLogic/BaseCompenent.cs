using DG.Tweening;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class BaseCompenent : Entity, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        public BaseCompenent Parent
        {
            get;
            set;
        }
        public BaseCompenent Child
        {
            get;
            set;
        } = null;
        public bool Producing
        {
            get;
            set;
        } = false;
        public NodeTag NodeTag
        {
            get;
            protected set;
        }
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

        public int Level
        {
            get;
            protected set;
        }
        public bool Ice
        {
            get;
            protected set;
        } = false;
        public List<NodeTag> Materials { get; protected set; } = new List<NodeTag>();

        protected SpriteRenderer mSpriteRenderer = null;
        protected SpriteRenderer mShader = null;
        protected SpriteRenderer mProgressBarRenderer = null;
        protected BoxCollider2D mBoxCollider2D = null;
        //当抓取时鼠标与中心点的差距
        protected Vector3 mMouseGap;
        protected NodeData mNodeData = null;
        protected CompenentData mCompenentData = null;
        protected Transform mProgressBar = null;
        protected float mLength = 2f;
        protected List<BaseCompenent> mCompenents = new List<BaseCompenent>();
        protected IDataTable<DRRecipe> dtRecipe = GameEntry.DataTable.GetDataTable<DRRecipe>();
        protected List<NodeTag> mMaterials = new List<NodeTag>();
        protected List<NodeTag> mRecipe = new List<NodeTag>();
        protected List<NodeTag> mProduct = new List<NodeTag>();
        protected List<NodeTag> mCheckRecipe = new List<NodeTag>();
        protected NodeTag tool = NodeTag.None;
        protected float mProducingTime = 0f;
        protected float mTime = 0f;
        protected DRRecipe drRecipe = null;

        private SpriteRenderer mIcePoint = null;
        private SpriteRenderer mSugarPoint = null;
        private SpriteRenderer mSaltPoint = null;
        private float mAddMaterialsTime = 0f;
        private float mAddTime = 0f;
        private bool flag = false;
        private int mLevel = 0;
        private int mEspressoLevel = 0;
        private bool isCoffee = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;

            NodeTag = mCompenentData.NodeData.NodeTag;
            mSpriteRenderer = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader = this.transform.Find("Shader").GetComponent<SpriteRenderer>();
            mProgressBar = this.transform.Find("ProgressBar").GetComponent<Transform>();
            mProgressBarRenderer = this.transform.Find("ProgressBar").GetComponent<SpriteRenderer>();

            mBoxCollider2D = this.GetComponent<BoxCollider2D>();

            mIcePoint = mSpriteRenderer.gameObject.transform.Find("Ice").GetComponent<SpriteRenderer>();
            mSugarPoint = mSpriteRenderer.gameObject.transform.Find("Sugar").GetComponent<SpriteRenderer>();
            mSaltPoint = mSpriteRenderer.gameObject.transform.Find("Salt").GetComponent<SpriteRenderer>();
            mAddMaterialsTime = 5f;
            Level = mNodeData.MLevel;
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            if (mNodeData.Follow)
            {
                GameEntry.Utils.pickUp = true;
                mBoxCollider2D.isTrigger = true;
                mShader.sortingOrder = GameEntry.Utils.CartSort;
                mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;

                this.transform.position = MouseToWorld(Input.mousePosition);
                mMouseGap = Vector3.zero;
                mSpriteRenderer.sortingLayerName = "Controller";
                mShader.sortingLayerName = "Controller";
                PickUp();


            }
            if (mNodeData.Jump)
            {
                Vector3 newPos = (Vector3)UnityEngine.Random.insideUnitCircle;
                this.transform.DOMove(mNodeData.Position + newPos * mLength, 0.5f).SetEase(Ease.OutExpo);
            }
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Parent != null)
            {
                mBoxCollider2D.isTrigger = true;

            }
            if (Child == null)
            {
                mBoxCollider2D.size = mSpriteRenderer.size;
                mBoxCollider2D.offset = new Vector2(0, 0.04449272f);
            }
            if (Child != null)
            {
                mBoxCollider2D.size = new Vector2(1.366413f, 0.583478f);
                mBoxCollider2D.offset = new Vector2(-0.005561709f, -0.6995664f);
            }

            if (!Input.GetMouseButton(0))
            {
                mNodeData.Follow = false;
                GameEntry.Utils.pickUp = false;
            }
            if (mNodeData.Follow)
            {
                //缓动本身没有问题，但现在需要计算鼠标移动来跟踪了
                this.transform.DOMove(MouseToWorld(Input.mousePosition) - mMouseGap, 0.05f);
                //this.transform.position=MouseToWorld(Input.mousePosition);
                //卡牌的移动和卡牌被拿起来的效果是放在不一样的层级上面的
                Producing = false;
                tool = NodeTag.None;
                mProducingTime = 0;
                mTime = 0f;
                mRecipe.Clear();
                mProduct.Clear();
                mCheckRecipe.Clear();
                mProgressBar.gameObject.SetActive(false);
                mProgressBar.transform.SetLocalScaleX(1);
            }

            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(this.transform.position.y, -8f, -1.6f), 0);//限制范围
            //if (Parent == null)
            //    SpriteRenderer.sortingOrder = 0;
            if (Parent != null && !mNodeData.Follow)
            {
                this.transform.DOMove(Parent.transform.position + Vector3.up * 0.5f, 0.1f);//吸附节点
            }

            mMaterials = GenerateMaterialList();
            Compound();
            AddMaterials();
            ShowMyLevel();
            mProgressBarRenderer.sortingOrder = mSpriteRenderer.sortingOrder + 1;
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
            GameEntry.Utils.pickUp = true;
            mBoxCollider2D.isTrigger = true;
            mShader.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "Controller";
            mShader.sortingLayerName = "Controller";
            //播放拿起的声音
            //抬高卡片
            mMouseGap = MouseToWorld(Input.mousePosition) - this.transform.position;
            PickUp();
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader.sortingLayerName = "GamePlay";
            mBoxCollider2D.isTrigger = false;
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

            //避免出现循环
            BaseCompenent parent = bestCompenent;
            //避免出现死循环
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
            Parent = bestCompenent;
            Parent.Child = this;
        }
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.pickUp)
                return;
            if (Parent != null)
                return;
            PitchOn();
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.pickUp)
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
        /// 拿起状态
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
        /// 选中
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
        /// 放下
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
            GameEntry.Entity.HideEntity(this.transform.parent.GetComponent<BaseNode>().NodeData.Id);
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
            if (!Producing)
            {
                for (int i = 0; i < 19; i++)
                {
                    drRecipe = dtRecipe.GetDataRow(i);
                    mRecipe = TransToEnumList(drRecipe.Recipe);
                    tool = TransToEnum(drRecipe.Tool);
                    if (Parent == null && Child != null)
                    {
                        if (NodeTag == tool)
                        {
                            if (mRecipe.SequenceEqual(mMaterials))
                            {
                                Producing = true;
                                mProduct = TransToEnumList(drRecipe.Product);
                                mCheckRecipe = mRecipe;
                                mProducingTime = drRecipe.ProducingTime;
                                mTime = drRecipe.ProducingTime;
                                mLevel = drRecipe.CoffeeLevel;
                                isCoffee = drRecipe.IsCoffee;
                            }
                        }
                    }
                }
            }
            else
            {
                mProgressBar.gameObject.SetActive(true);
                mProgressBar.transform.SetLocalScaleX(1 - (1 - mProducingTime / mTime));
                mProducingTime -= Time.deltaTime;
                if (mCheckRecipe.Count != mMaterials.Count)
                {
                    tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mCheckRecipe.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    mProgressBar.transform.SetLocalScaleX(1);
                    Producing = false;
                    return;
                }
                if (!(Parent == null && Child != null))
                {
                    tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mCheckRecipe.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    mProgressBar.transform.SetLocalScaleX(1);
                    Producing = false;
                    return;
                }

                if (mProducingTime <= 0)
                {
                    for (int i = 0; i < mProduct.Count; i++)
                    {
                        Debug.Log(mProduct.Count);
                        if (mProduct[i] == NodeTag.Espresso)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mProduct[i],mLevel)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                Follow = false
                            });
                        }
                        else if(isCoffee)
                        {
                            FindMyEspressoLevel();
                            GenerateCoffeeLevel();
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mProduct[i], mLevel)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                Follow = false
                            });
                        }
                        else
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mProduct[i])
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                Follow = false
                            });
                        }
                    }
                    if (Child != null)
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
                    Materials.Clear();
                    if (this.NodeTag == NodeTag.Cup)
                    {
                        this.Remove();
                    }
                    tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    Producing = false;
                    return;

                }
            }
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

        public void GenerateCoffeeLevel()
        {
            if (LuckyDraw(1000, 5))
            {
                mLevel = 4;
            }
            else
            {
                if (mLevel==1)
                {
                    if (LuckyDraw(100, 5))
                    {
                        if (LuckyDraw(100, 2))
                        {
                            mLevel = 3;
                        }
                        else
                        {
                            mLevel = 2;
                        }
                    }
                    else
                    {
                        mLevel = 1;
                    }
                }
                else if (mLevel == 2)
                {
                    if (LuckyDraw(100, 2))
                    {
                        mLevel = 3;

                    }
                    else
                    {
                        mLevel = 2;
                    }
                }
                else if (mLevel == 3)
                {
                    mLevel = 3;
                }
            }
        }
        public void AddMaterials()
        {

            if (Parent == null && Child != null && Child.Child == null && flag == false)
            {
                if (Child.NodeTag == NodeTag.Ice)
                {
                    flag = true;
                    Ice = true;
                    mAddTime = mAddMaterialsTime;
                }
                else if (Child.NodeTag == NodeTag.Sugar)
                {
                    flag = true;
                    Sugar = true;
                    mAddTime = mAddMaterialsTime;
                }
            }
            if (flag == true)
            {
                if (Ice==true&&!mIcePoint.gameObject.activeSelf)
                {
                    mProgressBar.gameObject.SetActive(true);
                    mProgressBar.transform.SetLocalScaleX(1 - (1 - mAddTime / mAddMaterialsTime));
                    mAddTime -= Time.deltaTime;
                    if (Parent != null || Child == null||Child.Child!=null)
                    {
                        flag = false;
                        Ice = false;
                        mProgressBar.gameObject.SetActive(false);
                        mProgressBar.transform.SetLocalScaleX(1);
                        return;
                    }
                    if (mAddTime <= 0)
                    {
                        mProgressBar.gameObject.SetActive(false);
                        mIcePoint.gameObject.SetActive(true);
                        if (Child != null)
                        {
                            Child.Remove();
                        }
                        Ice = true;
                        flag = false;
                    }
                }
                else if (Sugar==true&&!mSugarPoint.gameObject.activeSelf)
                {
                    mProgressBar.gameObject.SetActive(true);
                    mProgressBar.transform.SetLocalScaleX(1 - (1 - mAddTime / mAddMaterialsTime));
                    mAddTime -= Time.deltaTime;
                    if (Parent != null || Child == null)
                    {
                        flag = false;
                        Sugar = false; ;
                        mProgressBar.gameObject.SetActive(false);
                        mProgressBar.transform.SetLocalScaleX(1);
                        return;
                    }
                    if (mAddTime <= 0)
                    {
                        mProgressBar.gameObject.SetActive(false);
                        mSugarPoint.gameObject.SetActive(true);
                        if (Child != null)
                        {
                            Child.Remove();
                        }
                        Sugar = true;
                        flag = false;
                    }
                }
            }
        }
        public bool LuckyDraw(int rangeNum, int checkNum)
        {
            int randomNum = 0;
            randomNum = UnityEngine.Random.Range(0, rangeNum);
            if (randomNum < checkNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public void MixMaterials()
        {
            for (int i = 0; i < mMaterials.Count; i++)
            {
                Materials.Add(mMaterials[i]);
            }
            BaseCompenent child = Child;
            while (child != null)
            {
                for (int i = 0; i < child.mNodeData.MDeliverMaterials.Count; i++)
                {
                    Materials.Add(child.mNodeData.MDeliverMaterials[i]);
                }
                child = child.Child;
            }
        }*/
        public void FindMyEspressoLevel()
        {
            BaseCompenent child = Child;
            List<int> levelList = new List<int>();
            while (child != null)
            {
                if(child.NodeTag==NodeTag.Espresso)
                {
                    levelList.Add(child.Level);
                }
                child = child.Child;
            }
            levelList.Sort();
            mLevel = levelList[0];
        }

        public void ShowMyLevel()
        {
            Debug.Log(Level);
        }
    }

    public enum NodeState
    {
        //未激活
        InActive,
        //激活
        Idle,
        //被拿起
        PickUp,
        //被选中
        PitchOn
    }
}


