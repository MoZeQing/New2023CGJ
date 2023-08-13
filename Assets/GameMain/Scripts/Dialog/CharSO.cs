using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu]
    public class CharSO : ScriptableObject
    {
        [SerializeField]
        public CharEntityData charData;
    }
    [System.Serializable]
    public class CharData
    {
        public int favor;//好感度
        public int mood;//心情
        public int hope;//希望
        public int love;//爱情
        public int family;//亲情

        public CharData() { }
        public CharData(int favor, int mood, int hope, int love, int family)
        { 
            this.favor = favor;
            this.mood = mood;
            this.hope = hope;
            this.love = love;
            this.family = family;
        }
    }
    [System.Serializable]
    public class CharEntityData
    {
        public string charName;//角色名称
        public List<Sprite> diffs = new List<Sprite>();//差分
        public List<int> audios = new List<int>();
    }
}

