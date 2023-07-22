using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu]
    public class CharSO : ScriptableObject
    {
        [SerializeField]
        public CharData charData;
    }

    [System.Serializable]
    public class CharData
    {
        public string charName;//角色名称
        public int favour;//好感度
        public List<Sprite> Diffs = new List<Sprite>();//差分 
    }
}

