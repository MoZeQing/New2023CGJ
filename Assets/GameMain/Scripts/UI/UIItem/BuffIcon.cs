using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Button iconBtn;
    private DRBuff dRBuff;
    public void SetData(DRBuff dRBuff)
    {
        this.dRBuff= dRBuff;
        //iconImg.sprite = Resources.Load<Sprite>(dRBuff.IconPath);
        iconBtn.onClick.AddListener(()=>GameEntry.UI.OpenUIForm(UIFormId.OkTips,dRBuff.BuffText));
    }
}
