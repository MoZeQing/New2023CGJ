using DG.Tweening;
using Dialog;
using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyDailogBox : DialogBox
{
    [SerializeField] protected Transform canvas;
    [SerializeField] protected BaseBackground background;

    public bool IsCG { get; set; }
    public bool IsBackground { get; set; }

    // 黑屏和白色文本相关
    [Header("黑屏与白色文本")]
    [SerializeField] private CanvasGroup blackScreenCanvasGroup; // 黑屏 CanvasGroup
    [SerializeField] private Text blackScreenText; // 显示黑屏下的白色文字
    [SerializeField] private bool isBlackScreenActive = false; // 标记当前是否处于黑屏状态

    public override void Next()
    {
        if (optionFlag || IsCG || IsBackground || !IsNext || m_Data == null)
            return;

        if (m_Data.After.Count != 0)
        {
            // 输出当前 BaseData 节点的信息
            Debug.Log($"Current BaseData ID: {m_Data.Id}");
            Debug.Log($"Fore Count: {m_Data.Fore.Count}, After Count: {m_Data.After.Count}");
            foreach (BaseData after in m_Data.After)
            {
                Debug.Log($"After BaseData ID: {after.Id}, Type: {after.GetType().Name}");
            }
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
                    case "VoiceData":
                        ShowVoice((VoiceData)after);
                        break;
                }
            }
        }
        else
        {
            CompleteDialog();
        }
    }

    protected override void Option_Onclick(object sender, EventArgs e)
    {
        base.Option_Onclick(sender, e);
        OptionData optionData = sender as OptionData;
        GameEntry.Utils.RunEvent(optionData.eventData);
    }

    public override void CompleteDialog()
    {
        Debug.Log($"CompleteDialog called for BaseData ID: {m_Data?.Id ?? -1}");
        ClearButtons();
        nameText.text = string.Empty;
        dialogText.text = string.Empty;
        blackScreenText.text = string.Empty; // 清空黑屏文字
        blackScreenCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            blackScreenCanvasGroup.gameObject.SetActive(false);
            isBlackScreenActive = false; // 结束黑屏状态
        });
        mIndex = 0;
        m_DialogData = null;
        m_Data = null;
        OnComplete?.Invoke();
        OnComplete = null;
    }

    protected override void ShowChat(ChatData chatData)
    {
        try
        {
            if (isBlackScreenActive)
            {
                // 如果当前是黑屏状态，需要退出黑屏
                blackScreenCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
                {
                    blackScreenCanvasGroup.gameObject.SetActive(false);
                    isBlackScreenActive = false;
                    DisplayChat(chatData);
                });
            }
            else
            {
                DisplayChat(chatData);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing chat: {ex.Message}\nChatData identifier: {chatData.Id}");
        }
    }
    public virtual void ShowVoice(VoiceData voiceData)
    {
        m_Data = voiceData;
        GameEntry.Sound.PlaySound(voiceData.voice);
        Next();
    }
    public virtual void ShowBackground(BackgroundData backgroundData)
    {
        m_Data = backgroundData;
        IsBackground = true;
        background.SetBackground(backgroundData, this);
    }

    protected virtual void ShowCG(CGData cgData)
    {
        m_Data = cgData;
        GameEntry.SaveLoad.AddCGFlag(cgData.cgFlag);
        BackgroundData backgroundData = new BackgroundData
        {
            backgroundTag = BackgroundTag.Fade,
            parmOne = cgData.parmOne,
            parmTwo = cgData.parmTwo,
            parmThree = cgData.parmThree,
            backgroundSpr = cgData.cgSpr
        };
        IsBackground = true;
        background.SetBackground(backgroundData, this);
    }

    protected virtual void HideDialogBox()
    {
        canvas.gameObject.SetActive(true);
        IsCG = false;
    }
    protected virtual void ShowBlackChat(BlackData blackData)
    {
        // 清空普通对话框的内容，防止黑屏结束后显示之前的内容
        dialogText.text = string.Empty;
        nameText.text = string.Empty;

        if (!isBlackScreenActive)
        {
            // 初次进入黑屏状态，执行黑屏渐入效果
            blackScreenCanvasGroup.gameObject.SetActive(true);
            blackScreenCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                // 黑屏完全显示后，开始显示文字
                UpdateBlackScreenText(blackData);
            });
            isBlackScreenActive = true;
        }
        else
        {
            // 如果已经处于黑屏状态，直接更新文字
            UpdateBlackScreenText(blackData);
        }

        m_Data = blackData;
    }



    private void UpdateBlackScreenText(BlackData blackData)
    {
        blackScreenText.DOKill(); // 确保没有之前的动画干扰
        blackScreenText.text = string.Empty;

        // 使用 DOText 方法逐字显示文字
        blackScreenText.DOText(blackData.text, charSpeed * blackData.text.Length, true)
            .OnComplete(() =>
            {
                // 文本播放完毕，设置标识
                isTextComplete = true;
            });


    }
}
