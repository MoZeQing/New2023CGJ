using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cupborad : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        GameEntry.UI.OpenUIForm(UIFormId.CupboradForm, this);
    }
}
