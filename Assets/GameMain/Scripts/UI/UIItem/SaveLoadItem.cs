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
        [SerializeField] private Text favorText;
        [SerializeField] private Text wisdomText;
        [SerializeField] private Text charmText;
        [SerializeField] private Text staminaText;
        [SerializeField] private Text systemText;
        [SerializeField] private Image backgroundImg;

        [Header("美术资源")]
        [SerializeField] private Sprite background;
        [SerializeField] private Sprite empty;

        private Action<int> mAction;
        private Action<int> mCancel;
        private int mIndex;

        private void OnEnable()
        {
            loadBtn.onClick.AddListener(OnClick);
            cancelBtn.onClick.AddListener(OnCancel);
        }

        private void OnDisable()
        {
            loadBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            mAction(mIndex);
        }

        public void OnCancel()
        { 
            mCancel(mIndex);
        }

        public void SetData(SaveLoadData saveLoadData,Action<int> action,int index)
        {
            dayText.text = string.Format("第{0}天", saveLoadData.playerData.day+1);
            systemText.text = saveLoadData.dataTime;
            favorText.text = $"信任：{saveLoadData.charData.favor}";
            wisdomText.text = $"智慧：{saveLoadData.charData.wisdom}";
            charmText.text = $"魅力：{saveLoadData.charData.charm}";
            staminaText.text = $"体魄：{saveLoadData.charData.stamina}";
            SetData(action, index);
        }

        public void SetSaveData(Action<int> action, int index)
        {
            mAction = action;
            mIndex = index;
            backgroundImg.sprite = empty;
            dayText.gameObject.SetActive(false);
            favorText.gameObject.SetActive(false);
            wisdomText.gameObject.SetActive(false);
            charmText.gameObject.SetActive(false);
            staminaText.gameObject.SetActive(false);
            systemText.gameObject.SetActive(false);
            cancelBtn.gameObject.SetActive(false);
        }

        public void SetData(Action<int> action,int index)
        {
            Display();
            mAction = action;
            mIndex = index;
        }

        public void SetCancel(Action<int> cancel, int index)
        { 
            mCancel= cancel;
            mIndex= index;
        }

        public void Display()
        {
            loadBtn.interactable = true;
            backgroundImg.sprite = background;
            dayText.gameObject.SetActive(true);
            favorText.gameObject.SetActive(true);
            wisdomText.gameObject.SetActive(true);
            charmText.gameObject.SetActive(true);
            staminaText.gameObject.SetActive(true);
            systemText.gameObject.SetActive(true);
            cancelBtn.gameObject.SetActive(true);
        }
        public void Hide()
        {
            loadBtn.interactable = false;
            backgroundImg.sprite = empty;
            dayText.gameObject.SetActive(false);
            favorText.gameObject.SetActive(false);
            wisdomText.gameObject.SetActive(false);
            charmText.gameObject.SetActive(false);
            staminaText.gameObject.SetActive(false);
            systemText.gameObject.SetActive(false);
            cancelBtn.gameObject.SetActive(false);
        }
    }
}
