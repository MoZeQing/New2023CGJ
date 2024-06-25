using GameMain;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleItem : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] private Text text;

    private bool mIsHideItem;

    public void SetData(int score,string comment,float time,bool isHideItem=true)
    {
        this.gameObject.SetActive(true);
        this.GetComponent<AudioSource>().Play();
        this.StopAllCoroutines();
        this.Invoke(nameof(CloseItem), time);
        this.mIsHideItem = isHideItem;
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i<score);
        }
        text.text = comment;
    }

    public void CloseItem()
    {
        if (mIsHideItem)
            return;
        this.GetComponent<Animator>().SetBool("Hide", true);
        this.Invoke(nameof(HideItem), 0.5f);
    }

    public void HideItem()
    { 
        this.gameObject.SetActive(false);
    }
}
