using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework.DataTable;
using UnityEngine.UI;

namespace GameMain
{
    public class levelManager : MonoBehaviour
    {
        private void Start()
        {
            
        }
    }
    [System.Serializable]
    public class LevelData
    {
        public CharSO charSO;
        public DialogueGraph foreWork;
        public DialogueGraph afterWork;
        public DialogueGraph failWork;
        public List<OrderData> orderDatas;
        public int favor;//整体订单好感度，每完成一单则按照比例增加对应的favor，对话也应该分配对应的好感度
        public int orderTime;
    }
}