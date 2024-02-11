using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using DG.Tweening;
using UnityEngine.UI;

namespace GameMain
{
    public class LittleCat : MonoBehaviour
    {
        //[SerializeField] private List<Image> mLittleCats1 = new List<Image>();
        //[SerializeField] private List<Image> mLittleCats2 = new List<Image>();
        //[SerializeField] private List<Image> mLittleCats3 = new List<Image>();

        //[SerializeField] private float switchTime=10f;

        //private float nowtime;
        //private int index;
        //private bool autoEnable=true;

        //private List<Image> mLittleCats = new List<Image>();

        //private void Start()
        //{
        //    mLittleCats = mLittleCats1;
        //    switch (GameEntry.Utils.closet - 100)
        //    {
        //        case 1:
        //            mLittleCats = mLittleCats1;
        //            break;
        //        case 2:
        //            mLittleCats= mLittleCats2;
        //            break; 
        //        case 3:
        //            mLittleCats = mLittleCats3;
        //            break;
        //    }
        //    ShowLittleCat();
        //}
        //private void Update()
        //{
        //    if (!autoEnable)
        //        return;
        //    nowtime -= Time.deltaTime;
        //    if (nowtime <= 0)
        //    {
        //        nowtime = switchTime;
        //        ShowLittleCat();
        //    }

        //}
        //public void ShowLittleCat()
        //{
        //    HideLittleCat();
        //    autoEnable = true;
        //    switch (GameEntry.Utils.closet - 100)
        //    {
        //        case 1:
        //            mLittleCats = mLittleCats1;
        //            break;
        //        case 2:
        //            mLittleCats = mLittleCats2;
        //            break;
        //        case 3:
        //            mLittleCats = mLittleCats3;
        //            break;
        //    }
        //    int block = 100;
        //    int newIndex = Random.Range(0, mLittleCats.Count);
        //    while (newIndex == index)
        //    {
        //        newIndex = Random.Range(0, mLittleCats.Count);
        //        block--;
        //        if (block < 0)
        //            break;
        //    }
        //    index = newIndex;
        //    mLittleCats[index].DOKill();
        //    mLittleCats[index].gameObject.SetActive(true);
        //    mLittleCats[index].DOColor(Color.white, 0.5f);
        //}
        //public void HideLittleCat()
        //{
        //    autoEnable= false ;
        //    foreach (Image sprite in mLittleCats)
        //    {
        //        mLittleCats[index].color = Color.clear;
        //        mLittleCats[index].gameObject.SetActive(false);
        //    }
        //}

        //private void GameStateEvent(object sender, GameEventArgs e)
        //{
        //    MainStateEventArgs args = (MainStateEventArgs)e;
        //    if (args.MainState == MainState.Teach)
        //        ShowLittleCat();
        //    else
        //        HideLittleCat();
        //}
    }
}
