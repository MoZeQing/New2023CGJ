using GameMain;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaterialsPlots : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private NodeTag nodeTag;
    [SerializeField] private bool IsGuide;
    [SerializeField] private Text text;

    public void Start()
    {
        if (IsGuide)
        {
            text.text = "¡Þ";
        }
        else
        {
            if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag) == null)
            {
                text.text = "0";
                return;
            }    
            if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag).itemNum <= 99)
                text.text = GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag).itemNum.ToString();
            else
                text.text = "99+";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(nodeTag==NodeTag.Cup)
        {
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, nodeTag)
            {
                Position = this.transform.position,
                Jump = true
            });
            text.text = "¡Þ";
            return;
        }
        else if (!IsGuide)
        {
            if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag) == null)
            {
                text.text = "0";
                return;
            }
            if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag).itemNum <= 0)
            {
                text.text = GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag).itemNum.ToString();
                text.color = Color.red;
                return;
            }
        }
        GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag).itemNum--;
        text.text = GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag).itemNum.ToString();
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, nodeTag)
        {
            Position = this.transform.position,
            Jump = true
        }); 
    }
}
