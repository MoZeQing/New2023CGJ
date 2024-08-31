using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class CGData : BaseData
    {
        public CGTag cgTag;
        public int parmOne;
        public int parmTwo;
        public string parmThree;
        public Sprite cgSpr;
    }

    public enum CGTag
    { 
        None,
        HideDialogBox
    }
}
