using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterManager : MonoBehaviour
{
    static InventoryManager instance;

    public Inventory MyBag;
    public GameObject EmptySlot;
    public GameObject SlotGrid;
    //public Slot SlotPrefab;
    public Text ItemInfo;
}
