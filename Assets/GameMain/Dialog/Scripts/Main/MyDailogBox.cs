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
        if (optionFlag || IsCG || IsBackground || !IsNext || m_Data == null)
            return;

        if (m_Data.After.Count != 0)
        {
            foreach (BaseData after in m_Data.After)
            {
                string dialogTag = after.GetType().Name;
                switch (dialogTag)
                {
                    case "StartData":
                        InitDailog((StartData)after);
                        break;
                    case "ChatData":
                        ShowChat((ChatData)after);
                        break;
                    case "OptionData":
                        ShowOption((OptionData)after);
                        break;
                    case "CGData":
                        ShowCG((CGData)after);
                        break;
                    case "BackgroundData":
                        ShowBackground((BackgroundData)after);
                        break;
                    case "BlackData":
                        ShowBlackChat((BlackData)after); // 直接调用父类的ShowBlack方法
                        break;
                }
            }
        }
        else
        {
            CompleteDialog();
        }
    }

    public virtual void ShowBackground(BackgroundData backgroundData)
    {
        background.SetBackground(backgroundData, this);
        IsBackground = true;
        m_Data = backgroundData;
    }

    protected virtual void ShowCG(CGData cgData)
    {
        IsCG = true;
        m_Data = cgData;
    }

    protected virtual void HideDialogBox()
    {
        canvas.gameObject.SetActive(true);
        IsCG = false;
    }
}
