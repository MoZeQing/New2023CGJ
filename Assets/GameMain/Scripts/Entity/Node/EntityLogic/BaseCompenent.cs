using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class BaseCompenent : Entity, IPointerDownHandler
    {
        public bool Follow
        {
            get;
            protected set;
        } = false;

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

        public bool Completed
        {
            get;
            set;
        } = false;

        public NodeTag NodeTag
        {
            get;
            private set;
        }
        public NodeTag ProducingTool
        {
            get;
            set;
        }
        public SpriteRenderer SpriteRenderer
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
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!Input.GetMouseButton(0))
            {
                Follow = false;
            }
            if (Follow)
            {
                this.transform.position = MouseToWorld(Input.mousePosition);
                Producing = false;
            }
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(this.transform.position.y, -8f, -1.6f), 0);//限制范围
            if (Parent == null)
                SpriteRenderer.sortingOrder = 0;
            if (Parent != null && !Follow)
            {
                this.transform.DOMove(Parent.transform.position+Vector3.up*0.5f, 0.1f);//吸附节点
                SpriteRenderer.sortingOrder = Parent.SpriteRenderer.sortingOrder++;
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
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Follow)
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            if (Parent != null|| baseCompenent.Child != null)
                return;
            //避免出现循环
            BaseCompenent parent = baseCompenent;
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

            Parent = baseCompenent;
            baseCompenent.Child = this;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("检测到离开");
            if (!Follow) 
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            if (Parent == baseCompenent)
            {   
                Parent.Child= null;
                Parent = null; 
            }
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
}
