using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [System.Serializable]
    public class DialogData
    {
        private string m_DialogName;
        private List<BaseData> m_DialogDatas = new List<BaseData>();
        public string DialogName => m_DialogName;
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

        public DialogData(IDialogSerializeHelper helper, object data)
        {
            helper.Serialize(this,data);
        }
    }
}

