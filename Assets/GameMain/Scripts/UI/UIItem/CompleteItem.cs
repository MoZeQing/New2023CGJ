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
        if (value > 0)
        {
            valueTagText.text = $"+{value}";
        }
        else
        {
            valueTagText.text = $"{value}";
        }
    }
}
