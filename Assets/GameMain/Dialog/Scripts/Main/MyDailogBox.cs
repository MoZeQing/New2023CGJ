using Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDailogBox : DialogBox
{
    [SerializeField] protected Transform canvas;
    [SerializeField] protected BaseBackground background;

    public bool IsCG { get; set; }
    public bool IsBackground { get; set; }
    public override void Next()
    {
        if (optionFlag)
            return;
        if (IsCG)
            return;
        if (IsBackground)
            return;
        if (IsNext == false)
            return;
        if (m_Data == null)
            return;
        if (m_Data.After.Count != 0)
        {
            foreach (BaseData after in m_Data.After)
            {
                string dialogTag = after.GetType().Name;
                switch (dialogTag)
                {
                    case "StartData":
                        StartData startData = (StartData)after;
                        InitDialog(startData);
                        break;
                    case "ChatData":
                        ChatData chatData = (ChatData)after;
                        ShowChat(chatData);
                        break;
                    case "OptionData":
                        OptionData optionData = (OptionData)after;
                        ShowOption(optionData);
                        break;
                    case "CGData":
                        CGData cgData= (CGData)after;
                        ShowCG(cgData);
                        break;
                    case "BackgroundData":
                        BackgroundData backgroundData = (BackgroundData)after;
                        ShowBackground(backgroundData);
                        break;
                }
            }
        }
        else
        {
            ClearButtons();
            nameText.text = string.Empty;
            dialogText.text = string.Empty;
            mIndex = 0;
            m_DialogData = null;
            m_Data = null;
            if (OnComplete != null)
                OnComplete();
            OnComplete = null;
        }
    }
    public virtual void ShowBackground(BackgroundData backgroundData)
    {
        IsBackground = true;
        m_Data = backgroundData;
        background.SetBackground(backgroundData,this);
    }
    protected virtual void ShowCG(CGData cgGData)
    {
        IsCG = true;
        m_Data= cgGData;
    }

    protected virtual void HideDialogBox()
    {
        canvas.gameObject.SetActive(true);
        IsCG=false;
    }
}
