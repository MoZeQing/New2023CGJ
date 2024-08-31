using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [System.Serializable]
    public class DialogData
    {
        public string DialogName { get; set; }
        private List<BaseData> m_DialogDatas = new List<BaseData>();
        public List<BaseData> DialogDatas 
        {
            get
            {
                return m_DialogDatas;
            }
            set
            {
                m_DialogDatas = value;                   
            }
        }

        public StartData GetStartData()
        {
            BaseData baseData = m_DialogDatas[0];
            string typeName = baseData.GetType().ToString();
            return m_DialogDatas[0] as StartData;
        }

        public void Add(BaseData baseData)
        { 

        }

        public void Remove(BaseData baseData) 
        { 
            
        }
    }
}

