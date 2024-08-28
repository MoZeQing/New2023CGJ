using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [System.Serializable]
    public class ChatData : BaseData
    {
        public DialogPos chatPos;
        public string charName;
        public ActionData leftAction;
        public ActionData middleAction;
        public ActionData rightAction;
        public Sprite background;
        public string text;
        public string[] eventData;

        public ChatData() { }
    }
}

