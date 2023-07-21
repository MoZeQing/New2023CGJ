using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger 
{
    public List<Trigger> OR = new List<Trigger>();
    public List<Trigger> And = new List<Trigger>();

    public TriggerTag key;//什么标签
    public bool not;//是否取反
    public bool equals;
    public string value;//至少的数值
}
