using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour
{
    [SerializeField] private Text text; 

    private object data;
    private EventHandler handler;

    public void OnInit(object data,EventHandler handler)
    {
        this.data = data;
        this.handler = handler;

        OptionData optionData = (OptionData)data;
        text.text = optionData.text;

        this.GetComponent<Button>().onClick.AddListener(Onclick);
    }

    private void Onclick()
    {
        handler(data, EventArgs.Empty);
    }
}
