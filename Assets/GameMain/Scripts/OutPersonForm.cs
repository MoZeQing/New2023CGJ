using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class OutPersonForm : MonoBehaviour
    {
        [SerializeField] private Image charImg;
        [SerializeField] private Text charText;
        [SerializeField] private Text charNameText;
        [SerializeField] private OutingSceneState sceneState;
        [SerializeField] private string charName;

        private List<DRShop> dRShops = new List<DRShop>();

        private void Start()
        {
            dRShops.Clear();
            int favor = GameEntry.Utils.GetFriends()[charName].friendFavor;
            foreach (DRShop shop in GameEntry.DataTable.GetDataTable<DRShop>().GetAllDataRows())
            {
                if (favor < shop.Favor)
                    continue;
                dRShops.Add(shop);
            }
            DRShop dRShop = dRShops[Random.Range(0, dRShops.Count)];
            //charImg.sprite = Resources.Load<CharSO>("CharData/"+charName).diffs[dRShop.Diff];
            charText.text = dRShop.Text;
            GameEntry.Utils.AddFriendFavor(charName, dRShop.AddFavor);
        }
        private void Update()
        {
            //可能有的更新对话
        }
    }
}

