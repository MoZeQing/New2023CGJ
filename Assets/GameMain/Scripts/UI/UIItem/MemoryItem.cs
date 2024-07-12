using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class MemoryItem : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text textText;
        [SerializeField] private Text optionText;

        public void SetData(ChatData chatData)
        {
            nameText.gameObject.SetActive(true);
            textText.gameObject.SetActive(true);
            optionText.gameObject.SetActive(false);

            nameText.text = chatData.charName=="0"?string.Empty: chatData.charName;
            textText.text = chatData.text;
        }

        public void SetData(List<OptionData> optionDatas,OptionData index)
        {
            nameText.gameObject.SetActive(false);
            textText.gameObject.SetActive(false);
            optionText.gameObject.SetActive(true);

            optionText.text = string.Empty;
            foreach (OptionData optionData in optionDatas)
            {
                if(index==optionData)
                    optionText.text += $"！！>{optionData.text}<！！\n";
                else
                    optionText.text += $"{optionData.text}\n";
            }
        }
    }
}
