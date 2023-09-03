using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class SaveLoadItem : MonoBehaviour
    {
        [SerializeField] private Button loadBtn;
        [SerializeField] private Text dayText;
        [SerializeField] private Text systemText;

        private Action<int> mAction;
        private int mIndex;

        private void OnEnable()
        {
            loadBtn.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            loadBtn.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            mAction(mIndex);
        }

        public void SetData(SaveLoadData saveLoadData,Action<int> action,int index)
        {
            dayText.text = string.Format("µÚ{0}Ìì", saveLoadData.day);
            systemText.text = saveLoadData.dataTime;
            mAction = action;
            mIndex = index;
        }

        public void SetData(Action<int> action,int index)
        {
            mAction = action;
            mIndex = index;
        }
    }
}
