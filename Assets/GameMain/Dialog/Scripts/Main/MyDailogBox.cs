using Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDailogBox : DialogBox
{
    [SerializeField] protected Transform canvas;

    protected bool IsCG { get; set; }
    protected override void Next()
    {
        if (optionFlag)
            return;
        if (IsCG)
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
                        InitDailog(startData);
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

    protected virtual void ShowCG(CGData cgGData)
    {
        stage.SetBackground(Resources.Load<Sprite>($"Dialog/CG/{cgGData.cgName}"));
        canvas.gameObject.SetActive(false);
        IsCG= true;
        Invoke(nameof(HideDialogBox), cgGData.cgTime);
    }

    protected virtual void HideDialogBox()
    {
        canvas.gameObject.SetActive(true);
        IsCG=false;
    }
}
