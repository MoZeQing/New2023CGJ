using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class BaseCompenent : Entity, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler
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
            private set;
        }
        protected SpriteRenderer mSpriteRenderer = null;
        protected SpriteRenderer mShader = null;
        protected BoxCollider2D mBoxCollider2D = null;
        //当抓取时鼠标与中心点的差距
        protected Vector3 mMouseGap;
        protected NodeData mNodeData = null;
        protected CompenentData mCompenentData = null;

        protected float mLength = 2f;

        protected List<BaseCompenent>  mCompenents= new List<BaseCompenent>();
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;

            NodeTag = mCompenentData.NodeData.NodeTag;
            mSpriteRenderer = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            mShader = this.transform.Find("Shader").GetComponent<SpriteRenderer>();

            mBoxCollider2D = this.GetComponent<BoxCollider2D>();
            mBoxCollider2D.size = mSpriteRenderer.size;

            if (mNodeData.Follow)
            {
                GameEntry.Utils.pickUp = true;
                mBoxCollider2D.isTrigger = true;
                mShader.sortingOrder = GameEntry.Utils.CartSort;
                mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;

                this.transform.position = MouseToWorld(Input.mousePosition);
                mMouseGap = Vector3.zero;
                PickUp();
            }
            if (mNodeData.Jump)
            {
                Vector3 newPos = (Vector3)Random.insideUnitCircle;
                this.transform.DOMove(mNodeData.Position + newPos * mLength, 0.5f).SetEase(Ease.OutExpo);
            }
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Parent != null)
                mBoxCollider2D.isTrigger = true;
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
            }
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(this.transform.position.y, -8f, -1.6f), 0);//限制范围
            //if (Parent == null)
            //    SpriteRenderer.sortingOrder = 0;
            if (Parent != null && !mNodeData.Follow)
            {
                this.transform.DOMove(Parent.transform.position+Vector3.up*0.5f, 0.1f);//吸附节点
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

            mNodeData.Follow = true;
            GameEntry.Utils.pickUp = true;
            mBoxCollider2D.isTrigger = true;
            mShader.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "Controller";
            mShader.sortingLayerName = "Controller";
            //播放拿起的声音
            //抬高卡片
            mMouseGap = MouseToWorld(Input.mousePosition)-this.transform.position;
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
                    bestCompenent= baseCompenent;
                }
            }
            mCompenents.Clear();
            if (bestCompenent.Child != null)
                return;
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
            Parent.Child= this;
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
                Parent.Child= null;
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
            if(Parent!=null)
                Parent.Child=null;
            if(Child!=null)
                Child.Parent=null;
            GameEntry.Entity.HideEntity(this.transform.parent.GetComponent<BaseNode>().NodeData.Id);
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
