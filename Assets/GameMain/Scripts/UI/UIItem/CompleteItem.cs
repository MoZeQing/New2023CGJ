using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteItem : MonoBehaviour
{
    [SerializeField] private Image valueTagImg;
    [SerializeField] private Text valueTagText;
    public void SetData(ValueTag valueTag, int value)
    {
        valueTagText.text = value > 0 ? $"+{value}" : $"{value}";
    }
}
