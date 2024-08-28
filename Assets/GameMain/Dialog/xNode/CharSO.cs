using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu]
    public class CharSO : ScriptableObject
    {
        public bool isMain;
        public bool friend;
        public Vector3 offset;
        public float scale;
        public string charName;
        public int favor;
        public Sprite sprite;
        public Sprite orderSprite;
        public NodeTag favorCoffee;
        public List<Sprite> diffs = new List<Sprite>();
        public List<int> audios = new List<int>();
        [TextArea(5,10)]
        public string text;
        public CharAward[] charAwards= new CharAward[4];
    }
    [System.Serializable]
    public class CharAward
    {
        public int favor;//目标信赖
        public string text;//描述
        public List<EventData> awards= new List<EventData>();//效果
    }
}

