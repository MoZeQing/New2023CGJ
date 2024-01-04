using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using DG.Tweening;

namespace GameMain
{
    public class PropertyTips : UIFormLogic
    {
        [SerializeField] private GameObject propertyItem;
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform endPos;

        private Queue<PropertyItem> properties = new Queue<PropertyItem>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            properties.Clear();
            GameEntry.Event.Subscribe(ValueEventArgs.EventId, AddItem);
        }

        public void AddItem(object sender,GameEventArgs e)
        {
            ValueEventArgs value = (ValueEventArgs)e;
            GameObject go = Instantiate(propertyItem, startPos.position, Quaternion.identity, canvas);
            if (properties.Count != 0)
            {
                go.transform.DOMove(properties.Peek().transform.position - Vector3.up * 100 * properties.Count, 0.5f).SetEase(Ease.OutExpo);
            }
            else
            {
                go.transform.DOMove(canvas.position, 1f).SetEase(Ease.OutExpo);
            }
            PropertyItem property = go.GetComponent<PropertyItem>();
            property.SetData(PropertyTag.Energy,value.Value);
            properties.Enqueue(property);
            Invoke(nameof(RemoveItem), 4f);
        }

        public void RemoveItem()
        {
            PropertyItem property = properties.Dequeue();
            property.transform.DOMove(endPos.position, 0.5f).OnComplete(()=>Destroy(property.gameObject));
        }

        /// <summary>
        /// 清除所有的属性提示UI
        /// </summary>
        public void ClearItems()
        {
            while (properties.Count != 0)
            {
                PropertyItem property = properties.Dequeue();
                Destroy(property.gameObject);
            }
        }
    }

    public enum PropertyTag
    { 
        Energy,
        MaxEnergy,
        Money,

    }
}