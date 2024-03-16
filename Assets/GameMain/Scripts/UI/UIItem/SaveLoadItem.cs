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
        [SerializeField] private Button cancelBtn;
        [SerializeField] private Text dayText;
        [SerializeField] private Text systemText;
        [SerializeField] private Image backgroundImg;

        [Header("美术资源")]
        [SerializeField] private Sprite background;
        [SerializeField] private Sprite empty;

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
            dayText.text = string.Format("第{0}天", saveLoadData.day);
            systemText.text = saveLoadData.dataTime;
            SetData(action, index);
        }

        public void SetSaveData(Action<int> action, int index)
        {
            mAction = action;
            mIndex = index;
            backgroundImg.sprite = empty;
            dayText.gameObject.SetActive(false);
            systemText.gameObject.SetActive(false);
            cancelBtn.gameObject.SetActive(false);
        }

        public void SetData(Action<int> action,int index)
        {
            Display();
            mAction = action;
            mIndex = index;
        }

        public void Display()
        {
            loadBtn.interactable = true;
            backgroundImg.sprite = background;
            dayText.gameObject.SetActive(true);
            systemText.gameObject.SetActive(true);
            cancelBtn.gameObject.SetActive(true);
        }
        public void Hide()
        {
            loadBtn.interactable = false;
            backgroundImg.sprite = empty;
            dayText.gameObject.SetActive(false);
            systemText.gameObject.SetActive(false);
            cancelBtn.gameObject.SetActive(false);
        }
    }
}
