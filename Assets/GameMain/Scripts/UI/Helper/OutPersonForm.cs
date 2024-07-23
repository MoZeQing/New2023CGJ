using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameMain
{
    public class OutPersonForm : MonoBehaviour
    {
        //[SerializeField] private CharSO charSO;
        //[SerializeField] private Text charText;
        //[SerializeField] private Text charNameText;
        //[SerializeField] private OutingSceneState sceneState;
        //[SerializeField] private Button button;
        //[SerializeField, Range(0, 0.1f)] private float charSpeed;

        //private void OnEnable()
        //{
        //    ShowDialog();
        //    button.onClick.AddListener(ShowDialog);
        //}

        //private void OnDisable()
        //{
        //    button.onClick.RemoveAllListeners();
        //}

        //public void ShowDialog()
        //{
        //    List<DRShop> dRShops = new List<DRShop>();
        //    int favor = GameEntry.Utils.GetFriends()[charSO.name];
        //    foreach (DRShop shop in GameEntry.DataTable.GetDataTable<DRShop>().GetAllDataRows())
        //    {
        //        if (favor < shop.Favor)
        //            continue;
        //        dRShops.Add(shop);
        //    }
        //    DRShop dRShop = dRShops[Random.Range(0, dRShops.Count)];
        //    charNameText.text = charSO.charName;
        //    charText.text = string.Empty;
        //    charText.DOText(dRShop.Text, charSpeed * dRShop.Text.Length, true);
        //    GameEntry.Utils.AddFriendFavor(charSO.name, dRShop.AddFavor);
        //}
    }
}

