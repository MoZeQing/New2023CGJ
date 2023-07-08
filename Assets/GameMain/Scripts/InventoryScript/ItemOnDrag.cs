using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform OriginalParent;
    public Inventory MyBag;
    public int CurrentID;
     
    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalParent = transform.parent;
        CurrentID = OriginalParent.GetComponent<Slot>().SlotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

    }

    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "ItemImage")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerPressRaycast.gameObject.transform.parent.parent.position;
                var Temp = MyBag.ItemList[CurrentID];
                MyBag.ItemList[CurrentID] = MyBag.ItemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().SlotID];
                MyBag.ItemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().SlotID] = Temp;
                eventData.pointerPressRaycast.gameObject.transform.parent.position = OriginalParent.position;
                eventData.pointerPressRaycast.gameObject.transform.parent.SetParent(OriginalParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerPressRaycast.gameObject.transform.position;
                MyBag.ItemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().SlotID] = MyBag.ItemList[CurrentID];
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SlotID != CurrentID)
                    MyBag.ItemList[CurrentID] = null;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }
        transform.SetParent(OriginalParent);
        transform.position = OriginalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
