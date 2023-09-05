using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemImage;
    public int itemNum;
    public int price;
    [TextArea]
    public string itemInfo;
}
