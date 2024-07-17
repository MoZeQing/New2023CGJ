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
    [System.Serializable]
    public class CharData
    {
        public int favor;//好感度
        public int stamina;//体能
        public int wisdom;//智慧
        public int charm;//魅力
        public int staminaLevel;//体能
        public int wisdomLevel;//智慧
        public int charmLevel;//魅力

        public CharData() { }

        public Dictionary<ValueTag, int> GetValueTag(Dictionary<ValueTag, int> dic)
        {
            if (favor != 0)
                dic.Add(ValueTag.Favor, favor);
            if (stamina != 0)
                dic.Add(ValueTag.Stamina, stamina);
            if (wisdom != 0)
                dic.Add(ValueTag.Wisdom, wisdom);
            if (charm != 0)
                dic.Add(ValueTag.Charm, charm);
            return dic;
        }
    }
}

