using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [System.Serializable]
    public class OptionData:BaseData
    {
        public ParentTrigger trigger;
        public string text;
        public string eventData;
    }
}
