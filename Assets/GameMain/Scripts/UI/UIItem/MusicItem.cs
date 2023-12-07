using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace GameMain
{
    public class MusicItem : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Text priceText;
        [SerializeField] private Text musicInfo;
        [SerializeField] private Text mAMInfo;
        [SerializeField] private Text warningText;
        [SerializeField] private Button purchaseButton;

        private MusicItemData mMusicItemData;

        void Start()
        {
            purchaseButton.onClick.AddListener(OnClick);
        }
        void Update()
        {
            if (GameEntry.Utils.Money >= mMusicItemData.price)
            {
                purchaseButton.interactable = true;
                warningText.gameObject.SetActive(false);
            }
            if (GameEntry.Utils.Money < mMusicItemData.price)
            {
                purchaseButton.interactable = false;
                warningText.gameObject.SetActive(true);
            }
        }

        public void SetData(MusicItemData itemData)
        {
            mMusicItemData = itemData;
            priceText.text = itemData.price.ToString();
            musicInfo.text = itemData.itemInfo.ToString();
            mAMInfo.text = itemData.AbilityModifier.ToString();
        }
        private void OnClick()
        {
            if (GameEntry.Utils.Money >= mMusicItemData.price)
            {
                GameEntry.Utils.Money -= mMusicItemData.price;
                GameEntry.Utils.Favor += mMusicItemData.favor;
                GameEntry.Utils.Mood += mMusicItemData.mood;
                GameEntry.Utils.Ability += mMusicItemData.ability;
                GameEntry.Dialog.PlayStory(mMusicItemData.itemTag.ToString());
            }
        }
    }
}
