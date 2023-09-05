using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //static InventoryManager instance;

    //public Inventory MyBag;
    //public GameObject EmptySlot;
    //public GameObject SlotGrid;
    ////public Slot SlotPrefab;
    //public Text ItemInfo;

    //public List<GameObject> slots = new List<GameObject>();
    // void Awake()
    //{
    //    if (instance != null)
    //        Destroy(this);
    //    instance = this;
    //}

    //private void OnEnable()
    //{
    //    RefreshItem();
    //    instance.ItemInfo.text = "";
    //}

    //public static void UpdateItemInfo(string ItemDes)
    //{
    //    instance.ItemInfo.text = ItemDes;
    //}
    ///*public static void CreateNewItem(Item item)
    //{
    //    Slot NewItem = Instantiate(instance.SlotPrefab, instance.SlotGrid.transform.position, Quaternion.identity);
    //    NewItem.gameObject.transform.SetParent(instance.SlotGrid.transform);
    //    NewItem.SlotItem = item;
    //    NewItem.SlotImage.sprite = item.ItemImage;
    //    NewItem.SlotNum.text = item.ItemHeld.ToString();
    //}*/

    //public static void RefreshItem()
    //{
    //    for(int i=0;i<instance.SlotGrid.transform.childCount;i++)
    //    {
    //        if (instance.SlotGrid.transform.childCount == 0)
    //            break;
    //        Destroy(instance.SlotGrid.transform.GetChild(i).gameObject);
    //        instance.slots.Clear();
    //    }
    //    for(int i=0;i<instance.MyBag.ItemList.Count;i++)
    //    {
    //        //CreateNewItem(instance.MyBag.ItemList[i]);
    //        instance.slots.Add(Instantiate(instance.EmptySlot));
    //        instance.slots[i].transform.SetParent(instance.SlotGrid.transform);
    //        instance.slots[i].GetComponent<Slot>().SlotID = i;
    //        instance.slots[i].GetComponent<Slot>().SetUpSlot(instance.MyBag.ItemList[i]);
    //    }
    //}
}
