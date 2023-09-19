using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class Closet : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            GameEntry.UI.OpenUIForm(UIFormId.ClosetForm, this);
        }
    }
}
